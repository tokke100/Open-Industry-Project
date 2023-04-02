using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerturn : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.angularVelocity = transform.TransformDirection(Vector3.up) * 1;
    }
}
