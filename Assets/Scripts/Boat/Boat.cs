using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    private Dock _Dock;
    private Rigidbody rb;
    [Tooltip("Boat Speed controlled by player")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float MaxSpeed;
    [Tooltip("Docking Speed")]
    [SerializeField] private float ForceSpeed;

    [Header("not using in inspector")]
    public bool MoveOverride;
    public Vector3 DockingSpace;
    public Vector3 SeaSpace;
    private float MoveDir;
    [SerializeField] private float SpeedRemove;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _Dock = FindObjectOfType<Dock>();
    }

    void Update()
    {
        if (_Dock.BStatus == Boatstatus.OnSea)BoatMovement();
    }
    public void BoatMovement()
    {
        if (!MoveOverride)
        {
            MoveDir = Input.GetAxisRaw("Horizontal");
        }
    }
    public void MoveToSea()
    {
        MoveOverride = true;
        transform.position = Vector3.MoveTowards(transform.position, SeaSpace, MoveSpeed * Time.deltaTime);
        if (transform.position != SeaSpace) Invoke("MoveToSea", 0f);
        else MoveOverride = false;
    }
    public void MoveBackToDock()
    {
        MoveOverride = true;
        transform.position = Vector3.MoveTowards(transform.position, DockingSpace, MoveSpeed * Time.deltaTime);
        if (transform.position != DockingSpace) Invoke("MoveBackToDock", 0f);
        else MoveOverride = false;
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.right * ForceSpeed * MoveDir, ForceMode.Force);
        FixedSpeed();
        //if (Input.GetKey(KeyCode.Space)) rb.velocity = new Vector3(0, 0, 0);
    }

    private void FixedSpeed()
    {
        if (rb.velocity.magnitude > MaxSpeed)
        {
            Vector3 FixedSpeed = rb.velocity.normalized * MaxSpeed;
            rb.velocity = FixedSpeed; 
        }
    }
}
