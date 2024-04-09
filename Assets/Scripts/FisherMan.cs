using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FishLineState
{
    linein,
    lineout,
}
public class FisherMan : MonoBehaviour
{
    [SerializeField] private Rigidbody hook;
    [SerializeField] private GameObject OriginPoint;
    [SerializeField] private FishLineState LineState;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float distanceToRot; 
    private Vector3 TrowTo;
    private bool Trow = false;
    [SerializeField] private float forcePercentage;

    [SerializeField] private GameObject[] Field;
    private float Depth;
    private bool move;

    void Start()
    {
            Depth = GetRandomDepth();   
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && LineState == FishLineState.linein)
        {
            TrowTo.x = Input.GetAxisRaw("Mouse X");
            TrowTo.y = Input.GetAxisRaw("Mouse Y");
        }
        else if (TrowTo.x != 0 && TrowTo.y != 0)
        {
            Trow = true;
            LineState = FishLineState.lineout;   
        }
        if (hook.transform.position.y < Depth && !Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("kurwa");
            hook.isKinematic = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            hook.transform.position = OriginPoint.transform.position;
            hook.isKinematic = false;
            LineState = FishLineState.linein;
            //if (Vector3.Distance(OriginPoint.transform.position, transform.position) > distanceToRot)
            //{
            //    hook.transform.position = OriginPoint.transform.position;
            //    hook.isKinematic = false;
            //}
            //else
            //{
            //    LineState = FishLineState.linein;
            //    hook.isKinematic = true;
            //}
        }
        else move = false;
    }
    private void ResetKurwa()
    {
        Trow = false;
        TrowTo = new Vector3(0, 0, 0);
    }
    private void FixedUpdate()
    {
        if (Trow)
        {
            hook.AddForce(TrowTo.normalized * forcePercentage, ForceMode.Impulse);
            Invoke("ResetKurwa", 0.1f);
        }
        if (move)
        {
            Vector3 Dir = OriginPoint.transform.position - transform.position;
            //Vector3 Dir = transform.position - OriginPoint.transform.position;
            hook.AddForce(-Dir * MoveSpeed, ForceMode.Force);
        }
    }
    private float GetRandomDepth()
    {
        float value = Random.Range(Field[0].transform.position.y, Field[1].transform.position.y);
        return value;
    }
}
