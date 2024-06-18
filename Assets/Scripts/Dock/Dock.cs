using System;
using Boat;
using Events;
using Interfaces;
using PauseMenu;
using UnityEngine;

namespace Dock
{
    public class Dock : MonoBehaviour
    {
        [SerializeField] private float dockingRange;
        [SerializeField] private Transform boat;
        [SerializeField] private Transform dockPoint;
        private bool boatDocked;

        private void Start()
        {
            EventManager.DockSuccess += DockSuccess;
            EventManager.LeftShoreSuccess += UnDockBoat;
        }

        private void OnDestroy()
        {
            EventManager.DockSuccess -= DockSuccess;
            EventManager.LeftShoreSuccess -= UnDockBoat;
        }

        private void Update()
        {
            if (IsBoatInRange() && !boatDocked && Hook.instance.FishOnHook == null)
            {
                GetDockInput();
            }
        }

        private bool IsBoatInRange()
        {
            float distance = Vector2.Distance(boat.position, dockPoint.position);
            return distance < dockingRange;
        }

        private void GetDockInput()
        {
            if (!Input.GetKeyDown(KeyCode.E))
                return;
            DockBoat();
        }

        private void DockBoat()
        {
            boat.GetComponent<IBoat>()?.DockBoat();
        }

        private void DockSuccess()
        {
            boatDocked = true;
            EventManager.OnDock();
        }

        private void UnDockBoat()
        {
            boatDocked = false;
        }
    }
}