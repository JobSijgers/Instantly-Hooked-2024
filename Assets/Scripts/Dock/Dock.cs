using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Boat;
using Interfaces;
using UnityEngine;

namespace Dock
{
    public class Dock : MonoBehaviour
    {
        public static Dock instance;
        public delegate void FUndockSuccess(Dock dock);

        public event FUndockSuccess OnUndockSuccess;
        
        public delegate void FDockSuccess();
        
        public event FDockSuccess OnDockSuccess;
        
        [SerializeField] private float dockingRange;
        [SerializeField] private Transform boat;
        [SerializeField] private Transform dockPoint;
        private bool _boatDocked;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            var controller = boat.GetComponent<BoatController>();
            if (controller == null)
                return;
            controller.OnDockSuccess += DockSuccess;
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

            boatInterface.DockBoat(dockPoint.position, this);
        }

        private void TryUndock()
        {
            if (!Input.GetKeyDown(KeyCode.E))
                return; 
            OnUndockSuccess?.Invoke(this);
            _boatDocked = false;
        }

        private void DockSuccess()
        {
            _boatDocked = true;
            OnDockSuccess?.Invoke();
        }
    }
}