using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FishLineState
{
    linein,
    lineout,
    lineInWater
}
public class FisherMan : MonoBehaviour
{
    [SerializeField] private Rigidbody hook;
    [SerializeField] private GameObject OriginPoint;
    [SerializeField] private Dock _Dock;
    public FishLineState LineState;
    [SerializeField] private float MoveSpeed;
    public float DistanceToRot;
    [SerializeField] private float DisToWater;
    private Vector3 TrowTo;
    [SerializeField] private float forcePercentage;
    [SerializeField] private GameObject[] Field;
    private float Depth;
    private bool Trow = false;

    void Start()
    {
        Depth = GetRandomDepth();
    }
    void Update()
    {
        if (LineState == FishLineState.linein)
        {
            hook.useGravity = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && LineState == FishLineState.linein && _Dock.BStatus == Boatstatus.OnSea)
        {
            TrowTo.x = Input.GetAxisRaw("Mouse X");
            TrowTo.y = Input.GetAxisRaw("Mouse Y");
        }
        else if (TrowTo.x != 0 && TrowTo.y != 0)
        {
            if (TrowTo.y < 0) TrowTo.y = -TrowTo.y;
            //if (TrowTo.x < 0) TrowTo.x = -TrowTo.x;
            hook.isKinematic = false;
            hook.useGravity = true;
            Trow = true;
            LineState = FishLineState.lineout;
        }
        if (hook.transform.position.y < Depth && !Input.GetKey(KeyCode.Mouse1))
        {
            LineState = FishLineState.lineInWater;
            hook.isKinematic = true;
        }
        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.W)) && !hook.isKinematic) hook.velocity = RemoveVelocity();
        if (Input.GetKeyUp(KeyCode.Mouse1) && hook.transform.position.y > OriginPoint.transform.position.y - DisToWater)
        {
            hook.useGravity = true;
            hook.isKinematic = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            hook.useGravity = true;
            hook.isKinematic = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            hook.useGravity = false;
            hook.isKinematic = true;
        }
        if (LineState == FishLineState.linein) hook.transform.position = OriginPoint.transform.position;
    }
    private Vector3 RemoveVelocity()
    {
        return new Vector3(0, 0, 0);
    }
    private void ResetTrow()
    {
        Trow = false;
        TrowTo = new Vector3(0, 0, 0);
    }
    private void FixedUpdate()
    {
        if (Trow)
        {
            hook.AddForce(TrowTo.normalized * forcePercentage, ForceMode.Impulse);
            Invoke("ResetTrow", 0.1f);
        }
    }
    private float GetRandomDepth()
    {
        float value = Random.Range(Field[0].transform.position.y, Field[1].transform.position.y);
        return value;
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(OriginPoint.transform.position, Vector3.down * DistanceToRot);
    }
}
