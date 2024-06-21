using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpinComponent : MonoBehaviour
{
    [SerializeField] private float maxSpinSpeed = 1;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        if (rb == null)
            return;

        float spinSpeed = rb.velocity.magnitude * maxSpinSpeed;
        transform.Rotate(Vector3.forward, spinSpeed);
    }
}
