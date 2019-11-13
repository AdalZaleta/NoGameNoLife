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
    [Tooltip("™")]
    public float jumpForce = 5;
    public float gravityModifier = 1f;

    private Player player; 
    private Rigidbody rigi;
    private float currentVel = 0;
    private float currentAirVel = 0;
    private bool isGrounded = true;
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

        if (Mathf.Abs(_x) < float.Epsilon && Mathf.Abs(_y) < float.Epsilon) //Epsilon es casi igual a 0, pero comparar con esto evita problemas de redondeos de flotantes
        {
            //Si entra aquí, el jugador no se esta intentando mover
            horizontalVelocity = rigi.velocity;//Usando los vectores de la camara + vectores de los axis para calcular la dirección final

            velChange = -1f * slowDown * modifier * Time.fixedDeltaTime;
        }
        else
        {
            horizontalVelocity = (cameraRight.normalized * _x + cameraForward.normalized * _y); //Usando los vectores de la camara + vectores de los axis para calcular la dirección final

            Rotate(horizontalVelocity.x, horizontalVelocity.z); //Función para que rote el jugador

            velChange = acceleration * modifier * Time.fixedDeltaTime; //Se aumenta la velocidad dependiendo de cuanto tiempo se este moviendo, sin importa la dirección
        }

        if(isGrounded)
        {
            currentVel += velChange;
        }
        else
        {
            currentAirVel += velChange;
        }

        if (currentVel < 0)
            currentVel = 0;

        if (currentAirVel < 0)
            currentAirVel = 0;

        if (currentVel > topSpeed)
            currentVel = topSpeed;

        if (currentAirVel > topSpeed)
            currentAirVel = topSpeed;

        if (isGrounded)
        {
            rigi.velocity = (horizontalVelocity.normalized * currentVel) + verticalVelocity; //Aplico las velocidades
        }
        else
        {
            Vector3 finalHorizontalVelocity = (horizontalVelocity.normalized * currentAirVel * airDrift * Time.deltaTime);
            rigi.velocity = new Vector3(rigi.velocity.x + finalHorizontalVelocity.x, 0, rigi.velocity.z + finalHorizontalVelocity.z);
            rigi.velocity = Vector3.ClampMagnitude(rigi.velocity, topSpeed);
            rigi.velocity += Vector3.up * verticalVelocity.y;
        }
        
    }

    public void Rotate(float _x, float _y)
    {
        rigi.angularVelocity = Vector3.zero; //Le quito la velocidad angular porque si no da vueltas solo

        if (isGrounded)
        {
            transform.LookAt(transform.position + new Vector3(_x, 0, _y));
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.position + new Vector3(_x, 0, _y), Vector3.up), 10f);
            todo: //Hacer que no rote como cj
        }
    }

    public void Jump()
    {
        if(isGrounded)
            rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        currentAirVel = currentVel;

        isGrounded = false;
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

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            currentVel = Vector3.Scale(rigi.velocity, Vector3.forward + Vector3.right).magnitude;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Stoped touching floor");
        }
    }
}
