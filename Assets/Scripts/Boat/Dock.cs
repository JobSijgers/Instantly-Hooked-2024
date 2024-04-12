using Economy.ShopScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum Boatstatus
{
    Docked,
    OnSea,
    Moving
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
    public Boatstatus BStatus;
    [SerializeField] private float DistacnceToDock;
    [SerializeField] private BoxCollider Trigger;
    [SerializeField] private TMP_Text Text;
    private string DockString = "Press E To Dock";
    private string ReleaseString = "Press R To Release";

    void Start()
    {
        _Boat = FindObjectOfType<Boat>();
        _Boat.DockingSpace = _Boat.transform.position;
        _Boat.SeaSpace = SeaPoint.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && BStatus == Boatstatus.Docked && !_Boat.MoveOverride) ReleaseBoat();
        if (Input.GetKeyDown(KeyCode.E) && BStatus == Boatstatus.OnSea && !_Boat.MoveOverride) DockBoat();
        Debug.DrawRay(transform.position, Vector3.right * DistacnceToDock);
    }

    private void ReleaseBoat()
    {
        FindObjectOfType<Shop>().CloseShop();
        BStatus = Boatstatus.OnSea;
        _Boat.MoveToSea();
        OnBoatLeaveDock?.Invoke();
    }

    private void DockBoat()
    {
        if (Vector3.Distance(transform.position, _Boat.transform.position) < DistacnceToDock)
        {
            FindObjectOfType<Shop>().OpenShop();
            BStatus = Boatstatus.Docked;
            _Boat.MoveBackToDock();
            OnBoatEnterDock?.Invoke();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        switch (BStatus)
        {
            case Boatstatus.OnSea:
                Text.text = DockString;
                break;
            case Boatstatus.Docked:
                Text.text = ReleaseString;
                break;
        }
        if (BStatus == Boatstatus.OnSea && _Boat.MoveOverride) Text.text = "";
    }
    private void OnTriggerExit(Collider other)
    {
        Text.text = "";
    }
}