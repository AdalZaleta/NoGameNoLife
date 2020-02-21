using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlataformMovement : MonoBehaviour
{
    public Vector3 a, b;
    public float travelTime, restTime;

    Rigidbody rigi;
    string action;
    Vector3 vel;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        rigi.useGravity = false;
        transform.position = a;
        Go();
    }

    void FixedUpdate()
    {
        rigi.velocity = vel;
    }

    void Go()
    {
        vel = (b - a) / travelTime;
        rigi.velocity = vel;
        action = "Come";
        Invoke("Rest", travelTime);
    }

    void Come()
    {
        vel = (a - b) / travelTime;
        rigi.velocity = vel;
        action = "Go";
        Invoke("Rest", travelTime);
    }

    void Rest()
    {
        vel = Vector3.zero;
        rigi.velocity = vel;
        Invoke(action, restTime);
    }
}
