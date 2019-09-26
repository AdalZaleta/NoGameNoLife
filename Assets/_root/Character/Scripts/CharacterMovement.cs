using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5;
    public float angularSpeed = 5;

    Rigidbody rigi;

    public float vel;
    public float axis;

    private void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    public void Move(float _x, float _y)
    {

    }

    private void Rotate(float _x, float _y)
    {

    }
}
