using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {

    [Header("Setup")]
    CharacterController characterController;
    public Vector3[] positionsCamera;
    [Header("Settings")]
    public float topSpeed = 8f;
    public float acceleration = 0.8f;
    public float friction = 20f;
    public float excessFriction = 30f;
    public float inAirSpeedModifier;
    public float airAcceleration = 5f;
    public float airDrift;
    public float airFriction;
    public float airExcessFriction;
    public float airRotationSpeed;
    public float sphereCastDistance = 0.05f;
    public LayerMask groundCheckMask;
    [Tooltip("™")]
    public float jumpForce = 5;
    public float gravityModifier = 1f;
    public Vector3 sphereCastCenter;
    public float sphereCastRadius;

    private Player player; 
    private Rigidbody rigi;

    private float currentVel = 0;
    private float currentAirVel = 0;
    private Vector3 groundVel;

    [SerializeField] private bool isGrounded = true;
    private bool usedJump = false;
    [SerializeField] private Vector3 groundNormal;
    private Transform groundTransform;
    private Vector3 prevGroundPos;
    private Vector3 groundPosOffset;

    private int actualPosCamera = 0;

    [Header("Debug")]
    public bool altMove = true;
    public Vector3 altMoveVel;
    public Vector3 gPos, pgPos, gVel;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        player = ReInput.players.GetPlayer(0);
    }

    void FixedUpdate()
    {
        CheckGround();  //TODO: hacer tooltips de variables, borrar las que no se usan.
                        // Que se pegue al piso en las rampas, arreglar bug que no te puedes mover hasta que brinques
                        // Que hacer con la velocidad vertical si se golpea en una esquina
        rigi.velocity += Physics.gravity * gravityModifier * Time.fixedDeltaTime;
    }

    void Move(float _x, float _y)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up); //Vector de enfrente proyectado en un plano con normal (0,1,0)
        Vector3 cameraRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up); //Vector de derecha proyectado

        Vector3 verticalVelocity = Vector3.Scale(rigi.velocity, Vector3.up);
        Vector3 horizontalDir = new Vector3();

        //Checando que fricción es la que se debe de usar
        float tempFriction = 0;
        if(isGrounded)
        {
            if (currentVel > topSpeed)
                tempFriction = excessFriction;
            else
                tempFriction = friction;
        }
        else
        {
            currentAirVel = Vector3.ProjectOnPlane(rigi.velocity, Vector3.up).magnitude; //Agarrando la velocidad actual como airVel, esto para mantener momento
            if (currentAirVel > topSpeed)
                tempFriction = airExcessFriction;
            else
                tempFriction = airFriction;
        }

        float velChange = 0;
        bool notMoving = false;
        if (Mathf.Abs(_x) < float.Epsilon && Mathf.Abs(_y) < float.Epsilon) //Epsilon es casi igual a 0, pero comparar con esto evita problemas de redondeos de flotantes
        {
            //Si entra aquí, el jugador no se esta intentando mover
            notMoving = true;
            horizontalDir = Vector3.ProjectOnPlane(rigi.velocity, groundNormal).normalized;
            velChange = -1f * tempFriction * Time.fixedDeltaTime;
        }
        else
        {
            horizontalDir = (cameraRight.normalized * _x + cameraForward.normalized * _y); //Usando los vectores de la camara + vectores de los axis para calcular la dirección final
            Rotate(horizontalDir.x, horizontalDir.z); //Rotando al jugador en la dirección de el movimiento
            horizontalDir = Vector3.ProjectOnPlane(horizontalDir, groundNormal).normalized; //Projectandolo despues de usarlo para rotar

            if(currentAirVel > topSpeed || currentVel > topSpeed)
            {
                //Se necesita bajar la velocidad con la fricción, tempFriction ya tiene el valor de la fricción adecuada
                velChange = -1 * tempFriction * Time.fixedDeltaTime;
            }
            else
            {
                float tempAccel = isGrounded ? acceleration : airAcceleration;
                velChange = tempAccel * tempFriction * Time.fixedDeltaTime; //Se aumenta la velocidad dependiendo de cuanto tiempo se este moviendo, sin importa la dirección
            }
        }
        
        if (isGrounded)
            currentVel += velChange;
        else
            currentAirVel += velChange;

        currentVel = Mathf.Max(currentVel, 0);
        currentAirVel = Mathf.Max(currentAirVel, 0);

        //Applying vel and accel
        if (isGrounded)
        {
            groundVel = Vector3.zero;
            if (groundTransform)
            {
                gPos = groundTransform.position;
                pgPos = prevGroundPos;
                groundVel = (groundTransform.position - prevGroundPos) / Time.fixedDeltaTime;
                gVel = groundVel;
            }

            rigi.velocity = horizontalDir * currentVel;
            
            //Añadiendole la velocidad de lo que este pisando
            if(groundVel.magnitude == 0)
            {
                Debug.Log("GROUND VEL IS 0");
            }
            rigi.velocity += groundVel;
        }
        else
        {
            //Cambio de velocidad si esta en el aire
            Vector3 horizontalAcceleration = horizontalDir * airDrift * Time.fixedDeltaTime;
            Vector3 horizontalVelocity = 
                (Vector3.ProjectOnPlane(rigi.velocity, Vector3.up) + horizontalAcceleration).normalized * currentAirVel;
            //La velocidad tope siempre decrese si es necesario, pero la dirección cambia segun horizontalAcceleration,

            rigi.velocity = horizontalVelocity + verticalVelocity;
        }

        altMoveVel = rigi.velocity;

        if (groundTransform)
        {
            prevGroundPos = groundTransform.position;
        }
    }

    public void Rotate(float _x, float _y)
    {
        rigi.angularVelocity = Vector3.zero; //Le quito la velocidad angular porque si no da vueltas solo

        Vector3 dir = new Vector3(_x, 0, _y);

        if (isGrounded)
        {
            transform.LookAt(transform.position + dir);
        }
        else
        {
            if (Vector3.Dot(transform.forward, dir) < 0.9f)
            {
                float q = Vector3.Dot(transform.right, dir);
                int mod = q > 0 ? 1 : -1;

                transform.rotation = Quaternion.AngleAxis(mod * airRotationSpeed * Time.fixedDeltaTime, Vector3.up) * transform.rotation;
            }
        }
    }

    public void Jump()
    {
        if(altMove)
        {
            if(isGrounded)
            {
                Debug.Log("Velocity before jump = " + rigi.velocity);

                rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                usedJump = true;

                Debug.Log("Velocity on jump = " + rigi.velocity);

                //currentAirVel = rigi.velocity.magnitude; //Mantiene su momentum

                isGrounded = false;
                //Debug.Break();
            }
            return;
        }

        if (isGrounded) //todo //Sacar el currentAirVel del if o cambiar como funciona el brinco completamente
        {
            Debug.Log("Velocity before jump = " + rigi.velocity);

            rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            usedJump = true;

            Debug.Log("Velocity on jump = " + rigi.velocity);

            currentAirVel = rigi.velocity.magnitude; //Mantiene su momentum

            isGrounded = false;
        }
    }

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + sphereCastCenter, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, layerMask: groundCheckMask))
        {
            #region Debug draws
            string namae = hit.collider ? hit.collider.gameObject.name : "unknown";
            //Debug.Log(hit.normal + " " + namae);
            Debug.DrawLine(transform.position + sphereCastCenter, transform.position + sphereCastCenter + Vector3.down * sphereCastDistance, Color.red, Time.fixedDeltaTime, false);
            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
            #endregion

            bool isRamp = false, wasGrounded = isGrounded;
            float dot = Vector3.Dot(hit.normal, Vector3.up);
            isGrounded = false;
            if(dot >= 0.5f) //Si esta tocando piso no muy inclinado
            {
                isGrounded = true;
                if(rigi.velocity.magnitude > float.Epsilon)
                {
                    if(Vector3.Dot(rigi.velocity.normalized, hit.normal) > 0.05f) //Si la velocidad es en dirección contraria a la normal (con un poco de tolerancia)
                    {
                        isGrounded = false; //entonces esta brincando y no deberia de entrar a isGrounded
                    }
                }
            }

            if (isGrounded) //Si concluimos que si esta en el piso
            {
                usedJump = false;
                groundNormal = hit.normal;

                if(groundTransform != hit.collider.gameObject.transform)
                {
                    groundTransform = hit.collider.gameObject.transform;
                    prevGroundPos = groundTransform.position;
                }
                
                if (!wasGrounded) //Si el ciclo anterior no lo estaba, entonces acaba de caer
                {
                    OnLanding(hit.normal);
                }
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnLanding(Vector3 _landNormal)
    {
        //TODO: cambiar a que sea relativo a la normal del piso
        Debug.Log("Landing to " + _landNormal);
        Vector3 vel = rigi.velocity;
        if(groundTransform)
        {
            vel -= groundVel;
        }
        currentVel = Vector3.ProjectOnPlane(vel, groundNormal).magnitude;
    }

    public void ChangeCameraPosition(int _dir)
    {
        if (_dir > 0)
        {
            actualPosCamera++;
            if (actualPosCamera > 3)
            {
                actualPosCamera = 0;
            }
            Camera.main.transform.LookAt(transform);
            //Personaje.transform.Rotate(new Vector3(0, 90 * actualPosCamera, 0));
        }
        else
        {
            actualPosCamera--;
            if (actualPosCamera < 0)
            {
                actualPosCamera = 3;
            }
            Camera.main.transform.LookAt(transform);
            //Personaje.transform.Rotate(new Vector3(0, 90 * actualPosCamera, 0));
        }
        Debug.Log("Pos Actual: " + actualPosCamera);
    }

    public void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position + sphereCastCenter, sphereCastRadius);
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

    public void OnCollisionExit(Collision collision)
    {
        
    }

}
