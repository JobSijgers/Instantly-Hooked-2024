using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpinComponent : MonoBehaviour
{
    [SerializeField] private float maxSpinSpeed = 3f;
    [SerializeField] private Vector3 spinAxis = Vector3.forward;
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
        bool movingLeft = rb.velocity.x < 0;
        if (movingLeft)
        {
            spinSpeed *= -1;
        }
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime);
    }
}
