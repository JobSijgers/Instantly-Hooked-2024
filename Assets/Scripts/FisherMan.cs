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
    public FishLineState LineState;
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
            Debug.Log($"trow force {TrowTo}");
        }
        else if (TrowTo.x != 0 && TrowTo.y != 0)
        {
            hook.isKinematic = false;
            Trow = true;
            LineState = FishLineState.lineout;   
        }
        if (hook.transform.position.y < Depth && !Input.GetKey(KeyCode.Mouse1))
        {
            hook.isKinematic = true;
        }

        //if (Input.GetKey(KeyCode.Mouse1))
        //{
        //    hook.useGravity = false;
        //    hook.transform.position = Vector3.MoveTowards(transform.position, OriginPoint.transform.position, MoveSpeed);
        //}
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (hook.transform.position.y < Field[0].transform.position.y)
            {
                hook.useGravity = true;
                hook.isKinematic = false;
            }
        }
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
    }
    private float GetRandomDepth()
    {
        float value = Random.Range(Field[0].transform.position.y, Field[1].transform.position.y);
        return value;
    }
}
