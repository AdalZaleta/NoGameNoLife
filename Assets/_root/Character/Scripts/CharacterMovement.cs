using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {

    [Header("Settings")]
    public float topSpeed;
    public float acceleration;
    public float slowDown;

    [Header("Debug")]
    public float xAxis, yAxis;

    private Rigidbody rigi;
    private float currentVel = 0;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Testing
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
    }

    public void Move(float _x, float _y)
    {
        xAxis = _x;
        yAxis = _y;
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
        Vector3 cameraRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up);

        Vector3 verticalVelocity = Vector3.Scale(rigi.velocity, Vector3.up);
        Vector3 horizontalVelocity = (cameraRight.normalized * _x + cameraForward.normalized * _y) * acceleration;

        Rotate(horizontalVelocity.x, horizontalVelocity.z);


        if(Mathf.Abs(_x) < float.Epsilon && Mathf.Abs(_y) < float.Epsilon)
        {
            currentVel -= slowDown * Time.deltaTime;

            if (currentVel < 0)
                currentVel = 0;
        }
        else
        {
            currentVel += acceleration * Time.deltaTime;

            if (currentVel > topSpeed)
                currentVel = topSpeed;
        }

        rigi.velocity = transform.forward * currentVel;

        if (rigi.velocity.magnitude > topSpeed)
        {
            rigi.velocity = Vector3.ClampMagnitude(rigi.velocity, topSpeed);
        }
    }

    public void Rotate(float _x, float _y)
    {
        rigi.angularVelocity = Vector3.zero;
        transform.LookAt(transform.position + new Vector3(_x, 0, _y));
    }
}
