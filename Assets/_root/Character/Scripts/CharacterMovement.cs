using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {

    [Header("Settings")]
    public float topSpeed = 10f;
    public float acceleration = 15f;
    public float slowDown = 20f;
    public float inAirSpeedModifier;
    [Tooltip("™")]
    public float jumpForce = 5;
    public float gravityModifier = 1f;

    private Rigidbody rigi;
    private float currentVel = 0;
    private bool isJumping = false;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        

        //Testing
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        rigi.velocity += Physics.gravity * gravityModifier * Time.fixedDeltaTime;
    }

    public void Move(float _x, float _y)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up); //Vector de enfrente proyectado en un plano con normal (0,1,0)
        Vector3 cameraRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up); //Vector de derecha proyectado

        Vector3 verticalVelocity = Vector3.Scale(rigi.velocity, Vector3.up);
        Vector3 horizontalVelocity = (cameraRight.normalized * _x + cameraForward.normalized * _y) * acceleration; //Usando los vectores de la camara + vectores de los axis para calcular la dirección final

        float rotationLerp = isJumping ? 0.25f : 0.9f; todo //To check later

        float modifier = isJumping ? inAirSpeedModifier : 1f;

        if (Mathf.Abs(_x) < float.Epsilon && Mathf.Abs(_y) < float.Epsilon) //Epsilon es casi igual a 0, pero comparar con esto evita problemas de redondeos de flotantes
        {
            //Si entra aquí, el jugador no se esta intentando mover
            currentVel -= slowDown * modifier * Time.fixedDeltaTime;

            if (currentVel < 0)
                currentVel = 0;
        }
        else
        {
            Rotate(horizontalVelocity.x, horizontalVelocity.z, rotationLerp); //Función para que rote el jugador

            currentVel += acceleration * modifier * Time.fixedDeltaTime; //Se aumenta la velocidad dependiendo de cuanto tiempo se este moviendo, sin importa la dirección

            if (currentVel > topSpeed)
                currentVel = topSpeed;
        }

        rigi.velocity = (horizontalVelocity.normalized * currentVel) + verticalVelocity; //Aplico las velocidades
    }

    public void Rotate(float _x, float _y, float _lerp)
    {
        rigi.angularVelocity = Vector3.zero; //Le quito la velocidad angular porque si no da vueltas solo

        Vector3 nextDir = (transform.position + new Vector3(_x, 0, _y)).normalized;

        float dot = Vector3.Dot(transform.right, nextDir);

        float up = 0f;

        if (dot > 0f)
            up = 1f;
        else
            up = -1f;

        transform.Rotate(Vector3.up * up, Vector3.Angle(transform.forward, nextDir * _lerp));

        //transform.LookAt(transform.position + new Vector3(_x, 0, _y));
    }

    public void Jump()
    {
        rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isJumping = true;
    }
}
