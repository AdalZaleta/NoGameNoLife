﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {

    [Header("Setup")]
    CharacterController characterController;
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
    private bool isGrounded = true;

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
        Vector3 horizontalVelocity;

        float modifier = !isGrounded ? inAirSpeedModifier : 1f;

        if (Mathf.Abs(_x) < float.Epsilon && Mathf.Abs(_y) < float.Epsilon) //Epsilon es casi igual a 0, pero comparar con esto evita problemas de redondeos de flotantes
        {
            //Si entra aquí, el jugador no se esta intentando mover
            horizontalVelocity = rigi.velocity;//Usando los vectores de la camara + vectores de los axis para calcular la dirección final

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

        rigi.velocity = (horizontalVelocity.normalized * currentVel) + verticalVelocity; //Aplico las velocidades
    }

    public void Rotate(float _x, float _y)
    {
        rigi.angularVelocity = Vector3.zero; //Le quito la velocidad angular porque si no da vueltas solo

        if(isGrounded)
            transform.LookAt(transform.position + new Vector3(_x, 0, _y));
    }

    public void Jump()
    {
        if(isGrounded)
            rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
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
