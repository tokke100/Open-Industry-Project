using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerturn : MonoBehaviour
{
    Rigidbody rb;
    Vector3 startAngle = new();
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startAngle = transform.eulerAngles;
    }

    void FixedUpdate()
    {
        rb.angularVelocity = transform.TransformDirection(Vector3.up) * 1;
        transform.eulerAngles= startAngle;
    }
}
