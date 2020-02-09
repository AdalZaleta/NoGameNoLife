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
    public float topSpeed = 10f;
    public float acceleration = 15f;
    public float slowDown = 20f;
    public float inAirSpeedModifier;
    public float airDrift;
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

    [SerializeField] private bool isGrounded = true;
    private bool usedJump = false;
    [SerializeField] private Vector3 groundNormal;
    private Transform groundTransform;
    private Vector3 prevGroundPos;
    private Vector3 groundPosOffset;

    private int actualPosCamera = 0;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        player = ReInput.players.GetPlayer(0);
    }

    void FixedUpdate()
    {
        //Testing
        /*Move(player.GetAxis("Move Horizontal"), player.GetAxis("Move Vertical"));
        
        if (player.GetButtonDown("Jump"))
            Jump();*/

        CheckGround();
        rigi.velocity += Physics.gravity * gravityModifier * Time.fixedDeltaTime;
    }

    public void Move(float _x, float _y)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up); //Vector de enfrente proyectado en un plano con normal (0,1,0)
        Vector3 cameraRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up); //Vector de derecha proyectado

        Vector3 verticalVelocity = Vector3.Scale(rigi.velocity, Vector3.up);
        Vector3 horizontalVelocity = new Vector3();

        float modifier = !isGrounded ? inAirSpeedModifier : 1f;
        float velChange = 0;
        bool notMoving = false;
        if (Mathf.Abs(_x) < float.Epsilon && Mathf.Abs(_y) < float.Epsilon) //Epsilon es casi igual a 0, pero comparar con esto evita problemas de redondeos de flotantes
        {
            //Si entra aquí, el jugador no se esta intentando mover
            notMoving = true;
            horizontalVelocity = Vector3.Scale(rigi.velocity, Vector3.forward + Vector3.right);

            /*if(isGrounded)
                Rotate(horizontalVelocity.x, horizontalVelocity.z); //Función para que rote el jugador*/
            
            velChange = -1f * slowDown * modifier * Time.fixedDeltaTime;
        }
        else
        {
            horizontalVelocity = (cameraRight.normalized * _x + cameraForward.normalized * _y); //Usando los vectores de la camara + vectores de los axis para calcular la dirección final

            Rotate(horizontalVelocity.x, horizontalVelocity.z); //Función para que rote el jugador

            if (!isGrounded && rigi.velocity.magnitude > topSpeed) //Si esta en el aire y trae mucha velocidad
            {
                velChange = -1f * slowDown * modifier * Time.fixedDeltaTime;
            }
            else
            {
                velChange = acceleration * modifier * Time.fixedDeltaTime; //Se aumenta la velocidad dependiendo de cuanto tiempo se este moviendo, sin importa la dirección
            }
        }

        float prevVel = currentVel;

        if (isGrounded)
        {
            currentVel += velChange;
        }
        else
        {
            currentAirVel += velChange;
        }

        //Clamping
        currentVel = Mathf.Clamp(currentVel, 0, topSpeed);
        //currentAirVel = Mathf.Clamp(currentAirVel, 0, topSpeed);

        if (isGrounded)
        {
            Vector3 groundVel = Vector3.zero;
            if(groundTransform)
            {
                groundVel = (groundTransform.position - prevGroundPos) / Time.fixedDeltaTime;
            }

            if (notMoving && currentVel == 0)
            {
                /*if(prevVel > 0)
                {
                    Debug.Log("It just stopped!!");
                    groundPosOffset = transform.position - groundTransform.position;
                }
                Debug.Log("Should sit still");
                transform.position = groundTransform.position + groundPosOffset;*/
                rigi.velocity = Vector3.zero;
            }
            else
            {
                rigi.velocity = Vector3.ProjectOnPlane(horizontalVelocity, groundNormal).normalized * currentVel;
            }

            //Añadiendole la velocidad de lo que este pisando
            rigi.velocity += groundVel;

            Debug.Log("Velocity after grounded calc = " + rigi.velocity);
            //Previa manera de aplicar velocidad
            //rigi.velocity = (horizontalVelocity.normalized * currentVel) + verticalVelocity; //Aplico las velocidades
        }
        else
        {
            Debug.Log("Vel on move before accel = " + rigi.velocity);
            Vector3 horizontalAcceleration = horizontalVelocity.normalized * currentAirVel * airDrift * Time.fixedDeltaTime;
            rigi.velocity = new Vector3(rigi.velocity.x + horizontalAcceleration.x, 0, rigi.velocity.z + horizontalAcceleration.z);
            rigi.velocity = Vector3.ClampMagnitude(rigi.velocity, topSpeed);
            rigi.velocity += Vector3.up * verticalVelocity.y;
        }

        prevGroundPos = groundTransform.position;
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
        currentVel = Vector3.Scale(rigi.velocity, Vector3.forward + Vector3.right).magnitude;
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
