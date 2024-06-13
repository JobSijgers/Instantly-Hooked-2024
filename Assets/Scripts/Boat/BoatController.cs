using System.Collections;
using Events;
using Interfaces;
using PauseMenu;
using Unity.VisualScripting;
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

        private Rigidbody rb;
        private float input;
        private bool docked;
        private Vector3 velocityAtPause;

        private void Start()
        {
            EventManager.UpgradeBought += OnUpgrade;
            EventManager.PauseStateChange += OnPause;
            EventManager.LeftShore += UndockBoat;
            EventManager.BoatControlsChange += DisableControls;
            EventManager.BoatAutoDock += DockBoat;
            EventManager.PlayerDied += ResetBoatPosition;
            rb = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
            EventManager.UpgradeBought -= OnUpgrade;
            EventManager.LeftShore -= UndockBoat;
            EventManager.BoatControlsChange -= DisableControls;
            EventManager.BoatAutoDock -= DockBoat;
            EventManager.PlayerDied -= ResetBoatPosition;
        }

        private void Update()
        {
            if (!docked)
            {
                input = GetBoatInput();
            }
        }

        private void FixedUpdate()
        {
            MoveBoat(input);
        }

        private static float GetBoatInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        private void MoveBoat(float input)
        {
            if (input == 0) return;
            rb.AddForce(Vector3.right * (input * speedMultiplier));
            LimitVelocity();
        }

        private void LimitVelocity()
        {
            //check if velocity succeeds max speed and counteracts it.
            if (rb.velocity.magnitude < maxVelocity) return;
            Vector3 velocity = rb.velocity;
            Vector3 counteractForce = velocity.normalized * (maxVelocity - velocity.magnitude);
            rb.AddForce(counteractForce);
        }

        public void DockBoat()
        {
            StartCoroutine(MoveBoatToDock(dock.position));
            docked = true;
        }

        private void UndockBoat()
        {
            docked = false;
        }

        private IEnumerator MoveBoatToDock(Vector3 dockLocation)
        {
            float totalDockingTime = 0f;
            while (Vector3.Distance(transform.position, dockLocation) > 0.3f)
            {
                totalDockingTime += Time.deltaTime;
                if (totalDockingTime >= 2f)
                {
                    break;
                }
                // Get direction to target
                Vector3 boatPosition = transform.position;
                Vector3 direction = (dockLocation - boatPosition).normalized;
                float distance = Vector3.Distance(boatPosition, dockLocation);
                float forceMagnitude = Mathf.Clamp(distance * speedMultiplier, 0f, Mathf.Infinity);

                float decelerationLerp = Mathf.Clamp01((distance - dockStoppingDistance) / dockStoppingDistance);
                forceMagnitude *= decelerationLerp;

                rb.AddForce(direction * forceMagnitude);
                LimitVelocity();

                yield return null;
            }

            // Stop movement at target
            rb.velocity = Vector3.zero;

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
            rb.isKinematic = isPaused;
            enabled = !isPaused;
            if (isPaused)
            {
                velocityAtPause = rb.velocity;
            }
            else
            {
                rb.velocity = velocityAtPause;
            }
        }

        private void DisableControls(bool state)
        {
            rb.isKinematic = state;
            //enabled = !state;
            if (state)
            {
                velocityAtPause = rb.velocity;
            }
            else
            {
                rb.velocity = velocityAtPause;
            }
        }
        
        private void ResetBoatPosition()
        {
            transform.position = dock.position;
            rb.velocity = Vector3.zero;
            DockBoat();
        }
    }
}