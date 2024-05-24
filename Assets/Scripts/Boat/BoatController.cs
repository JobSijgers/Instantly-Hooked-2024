using System.Collections;
using Events;
using Interfaces;
using PauseMenu;
using UnityEngine;
using Upgrades;
using Upgrades.Scriptable_Objects;

namespace Boat
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MonoBehaviour, IBoat
    {
        [SerializeField] private float maxVelocity;
        [SerializeField] private float speedMultiplier;
        [SerializeField] private float dockStoppingDistance;
        [SerializeField] private Transform dock;

        private Rigidbody _rigidbody;
        private float _input;
        private bool _docked;
        private Vector3 _velocityAtPause;

        private void Start()
        {
            EventManager.UpgradeBought += OnUpgrade;
            EventManager.PauseStateChange += OnPause;
            EventManager.LeftShore += UndockBoat;
            EventManager.BoatControlsChange += DisableControls;
            EventManager.BoatAutoDock += DockBoat;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
            EventManager.UpgradeBought -= OnUpgrade;
            EventManager.LeftShore -= UndockBoat;
            EventManager.BoatControlsChange -= DisableControls;
        }

        private void Update()
        {
            if (!_docked)
            {
                _input = GetBoatInput();
            }
        }

        private void FixedUpdate()
        {
            MoveBoat(_input);
        }

        private static float GetBoatInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        private void MoveBoat(float input)
        {
            if (input == 0) return;
            _rigidbody.AddForce(Vector3.right * (input * speedMultiplier));
            LimitVelocity();
        }

        private void LimitVelocity()
        {
            //check if velocity succeeds max speed and counteracts it.
            if (_rigidbody.velocity.magnitude < maxVelocity) return;
            Vector3 velocity = _rigidbody.velocity;
            Vector3 counteractForce = velocity.normalized * (maxVelocity - velocity.magnitude);
            _rigidbody.AddForce(counteractForce);
        }

        public void DockBoat()
        {
            StartCoroutine(MoveBoatToDock(dock.position));
            _docked = true;
        }

        private void UndockBoat()
        {
            _docked = false;
        }

        private IEnumerator MoveBoatToDock(Vector3 dockLocation)
        {
            while (Vector3.Distance(transform.position, dockLocation) > 0.3f)
            {
                // Get direction to target
                Vector3 boatPosition = transform.position;
                Vector3 direction = (dockLocation - boatPosition).normalized;
                float distance = Vector3.Distance(boatPosition, dockLocation);
                float forceMagnitude = Mathf.Clamp(distance * speedMultiplier, 0f, Mathf.Infinity);

                float decelerationLerp = Mathf.Clamp01((distance - dockStoppingDistance) / dockStoppingDistance);
                forceMagnitude *= decelerationLerp;

                _rigidbody.AddForce(direction * forceMagnitude);
                LimitVelocity();

                yield return null;
            }

            // Stop movement at target
            _rigidbody.velocity = Vector3.zero;

            EventManager.OnDockSuccess();
        }

        private void OnUpgrade(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case ShipSpeedUpgrade shipSpeedUpgrade:
                    maxVelocity = shipSpeedUpgrade.maxSpeed;
                    speedMultiplier = shipSpeedUpgrade.acceleration;
                    break;
            }
        }

        private void OnPause(PauseState newState)
        {
            switch (newState)
            {
                case PauseState.Playing:
                    SetPaused(false);
                    break;
                case PauseState.InPauseMenu:
                    SetPaused(true);
                    break;
                case PauseState.InInventory:
                    SetPaused(true);
                    break;
                case PauseState.InCatalogue:
                    SetPaused(true);
                    break;
                case PauseState.InQuests:
                    SetPaused(true);
                    break;
            }
        }

        private void SetPaused(bool isPaused)
        {
            _rigidbody.isKinematic = isPaused;
            enabled = !isPaused;
            if (isPaused)
            {
                _velocityAtPause = _rigidbody.velocity;
            }
            else
            {
                _rigidbody.velocity = _velocityAtPause;
            }
        }

        private void DisableControls(bool state)
        {
            _rigidbody.isKinematic = state;
            //enabled = !state;
            if (state)
            {
                _velocityAtPause = _rigidbody.velocity;
            }
            else
            {
                _rigidbody.velocity = _velocityAtPause;
            }
        }
    }
}