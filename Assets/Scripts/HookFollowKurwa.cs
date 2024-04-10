using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookFollowKurwa : MonoBehaviour
{
    [SerializeField] private Rigidbody Hook;
    [SerializeField] private Vector3 Originpoint;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float distanceToRot;
    private void Start()
    {

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.position = Vector3.MoveTowards(transform.position, Originpoint, MoveSpeed * Time.deltaTime);
            Hook.transform.position = transform.position;
        }
    }
}
