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

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        rigi.useGravity = false;
        transform.position = a;
        Go();
    }

    void Go()
    {
        rigi.velocity = (b - a) / travelTime;
        action = "Come";
        Invoke("Rest", travelTime);
    }

    void Come()
    {
        rigi.velocity = (a - b) / travelTime;
        action = "Go";
        Invoke("Rest", travelTime);
    }

    void Rest()
    {
        Invoke(action, restTime);
    }
}
