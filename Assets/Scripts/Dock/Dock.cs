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
        private bool _boatDocked;

        private void Start()
        {
            BoatController controller = boat.GetComponent<BoatController>();
            if (controller == null)
                return;
            controller.OnDockSuccess += DockSuccess;

            EventManager.PauseStateChange += OnPause;
            EventManager.PlayerDied += DockBoat;
            EventManager.LeftShore += UnDockBoat;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
            EventManager.PlayerDied -= DockBoat;
            EventManager.LeftShore -= UnDockBoat;
        }

        private void Update()
        {
            if (IsBoatInRange() && !_boatDocked)
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
            boat.GetComponent<IBoat>()?.DockBoat(dockPoint.position);
        }

        private void DockSuccess()
        {
            _boatDocked = true;
            EventManager.OnDock();
        }

        private void UnDockBoat()
        {
            _boatDocked = false;
        }

        private void OnPause(PauseState newState)
        {
            enabled = newState switch
            {
                PauseState.Playing => true,
                PauseState.InPauseMenu => false,
                PauseState.InInventory => false,
                PauseState.InCatalogue => false,
                PauseState.InQuests => false
            };
        }
    }
}