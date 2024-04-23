using System;
using System.Runtime.InteropServices.WindowsRuntime;
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
            var controller = boat.GetComponent<BoatController>();
            if (controller == null)
                return;
            controller.OnDockSuccess += DockSuccess;

            EventManager.PauseStateChange += OnPause;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
        }

        private void Update()
        {
            if (IsBoatInRange() && !_boatDocked)
            {
                TryBoatDock();
            }

            if (_boatDocked)
            {
                TryUndock();
            }
        }

        private bool IsBoatInRange()
        {
            var distance = Vector2.Distance(boat.position, dockPoint.position);
            return distance < dockingRange;
        }

        private void TryBoatDock()
        {
            if (!Input.GetKeyDown(KeyCode.E))
                return;
            
            var boatInterface = boat.GetComponent<IBoat>();
            if (boatInterface == null)
            {
                Debug.LogWarning("Boat interface not valid");
                return;
            }

            boatInterface.DockBoat(dockPoint.position);
        }

        private void TryUndock()
        {
            if (!Input.GetKeyDown(KeyCode.E))
                return; 
            EventManager.OnUndock();
            _boatDocked = false;
        }

        private void DockSuccess()
        {
            _boatDocked = true;
            EventManager.OnDock();
        }

        private void OnPause(PauseState newState)
        {
            enabled = newState switch
            {
                PauseState.Playing => true,
                PauseState.InPauseMenu => false,
                PauseState.InInventory => false,
                _ => throw new ArgumentOutOfRangeException(nameof(newState), newState, null)
            };
        }
    }
}