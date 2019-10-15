using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2 : MonoBehaviour
{

    [Header("Setup")]
    public CharacterController characterController;
    [Header("Settings")]
    public float topSpeed = 10f;
    public float acceleration = 15f;
    public float slowDown = 20f;
    public float inAirSpeedModifier;
    [Tooltip("™")]
    public float jumpForce = 5;
    public float gravity = 9.81f;

    private float currentVel = 0;
    public Vector3 velocity;

    void Start()
    {
        //rigi = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Testing
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //rigi.velocity += Physics.gravity * gravityModifier * Time.fixedDeltaTime;
    }

    public void Move(float _x, float _y)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up); //Vector de enfrente proyectado en un plano con normal (0,1,0)
        Vector3 cameraRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up); //Vector de derecha proyectado

        Vector3 verticalVelocity = Vector3.Scale(velocity, Vector3.up);

        if (characterController.isGrounded)
        {
            verticalVelocity = Vector3.zero;
        }
        else
        {
            verticalVelocity.y -= gravity * Time.fixedDeltaTime;
        }


        Vector3 horizontalVelocity;

        float modifier = !characterController.isGrounded ? inAirSpeedModifier : 1f;

        if (Mathf.Abs(_x) < float.Epsilon && Mathf.Abs(_y) < float.Epsilon) //Epsilon es casi igual a 0, pero comparar con esto evita problemas de redondeos de flotantes
        {
            //Si entra aquí, el jugador no se esta intentando mover
            horizontalVelocity = velocity;//Usando los vectores de la camara + vectores de los axis para calcular la dirección final

            currentVel -= slowDown * modifier * Time.fixedDeltaTime;

            if (currentVel < 0)
                currentVel = 0;
        }
        else
        {
            horizontalVelocity = (cameraRight.normalized * _x + cameraForward.normalized * _y); //Usando los vectores de la camara + vectores de los axis para calcular la dirección final

            Rotate(horizontalVelocity.x, horizontalVelocity.z); //Función para que rote el jugador

            currentVel += acceleration * modifier * Time.fixedDeltaTime; //Se aumenta la velocidad dependiendo de cuanto tiempo se este moviendo, sin importa la dirección

            if (currentVel > topSpeed)
                currentVel = topSpeed;
        }

        velocity = (horizontalVelocity.normalized * currentVel) + verticalVelocity;
        characterController.Move(velocity * Time.fixedDeltaTime);
        //rigi.velocity = (horizontalVelocity.normalized * currentVel) + verticalVelocity; //Aplico las velocidades
    }

    public void Rotate(float _x, float _y)
    {
        //rigi.angularVelocity = Vector3.zero; //Le quito la velocidad angular porque si no da vueltas solo

        if (!characterController.isGrounded)
            transform.LookAt(transform.position + new Vector3(_x, 0, _y));
    }

    public void Jump()
    {
        if(characterController.isGrounded)
            velocity.y += 8.0f;
        //rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
