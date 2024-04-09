using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoatBoatStatus
{
    Docked,
    OnSea,
}

public class Dock : MonoBehaviour
{
    public static Dock instance;

    public delegate void FBoatLeaveDock();

    public delegate void FBoatEnterDock();

    public event FBoatLeaveDock OnBoatLeaveDock;
    public event FBoatEnterDock OnBoatEnterDock;
    private Boat _Boat;
    [SerializeField] private GameObject SeaPoint;
    public BoatBoatStatus BoatStatus;
    [SerializeField] private float DistacnceToDock;

    void Start()
    {
        _Boat = FindObjectOfType<Boat>();
        _Boat.DockingSpace = _Boat.transform.position;
        _Boat.SeaSpace = SeaPoint.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ReleaseBoat();
        if (Input.GetKeyDown(KeyCode.E) && BoatStatus == BoatBoatStatus.OnSea) DockBoat();
        Debug.DrawRay(transform.position, Vector3.right * DistacnceToDock);
    }

    private void ReleaseBoat()
    {
        BoatStatus = BoatBoatStatus.OnSea;
        _Boat.MoveToSea();
        OnBoatLeaveDock?.Invoke();
    }

    private void DockBoat()
    {
        if (Vector3.Distance(transform.position, _Boat.transform.position) < DistacnceToDock)
        {
            BoatStatus = BoatBoatStatus.Docked;
            _Boat.MoveBackToDock();
            OnBoatEnterDock?.Invoke();
        }
    }
}