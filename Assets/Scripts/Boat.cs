using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    private float Percentage;
    private Dock _Dock;
    [SerializeField] private float MoveSpeed;

    [Header("not using in inspector")]
    public bool MoveOverride;
    public Vector3 DockingSpace;
    public Vector3 SeaSpace;
    private float MoveDir;
    void Start()
    {
        _Dock = FindObjectOfType<Dock>();
    }

    void Update()
    {
        if (_Dock.BoatStatus == BoatBoatStatus.OnSea)BoatMovement();
    }
    public void BoatMovement()
    {
        if (!MoveOverride)
        {
            MoveDir = Input.GetAxisRaw("Horizontal");
            transform.position = new Vector3(transform.position.x + MoveSpeed * Time.deltaTime * MoveDir, transform.position.y, transform.position.z);
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
}
