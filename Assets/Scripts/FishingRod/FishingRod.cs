using System;
using Enums;
using Events;
using PauseMenu;
using UnityEngine;
using Upgrades;
using Upgrades.Scriptable_Objects;

namespace FishingRod
{
    [RequireComponent(typeof(SpringJoint))]
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private Transform hook;
        private SpringJoint springJoint;
        private float reelingSpeed = 5;
        private float dropSpeed = 3;
        private float maxLineLength = 5;
        private float currentLineLength = 0;
        private bool rodEnabled = true;
        public float GetLineLength() => maxLineLength;

        private void Start()
        {
            EventManager.Dock += OnDock;
            EventManager.UpgradeBought += UpgradeBought;
            EventManager.LeftShore += OnUndock;
            EventManager.PauseStateChange += OnPause;

            springJoint = GetComponent<SpringJoint>();

            springJoint.connectedBody = hook.GetComponent<Rigidbody>();
            if (springJoint.connectedBody == null)
            {
                Debug.LogError("No rigidbody attached to hook");
            }

            springJoint.maxDistance = 0;
        }

        private void OnDestroy()
        {
            EventManager.Dock -= OnDock;
            EventManager.LeftShore -= OnUndock;
            EventManager.UpgradeBought -= UpgradeBought;
            EventManager.PauseStateChange -= OnPause;
        }

        private void Update()
        {
            if (!rodEnabled)
                return;
            if (Input.GetMouseButton(0) && !Hook.instance.touchingGround)
            {
                CastHook();
            }

            if (Input.GetMouseButton(1))
            { 
                ReelHook();
            }
        }

        private void CastHook()
        {
            float newLineLength = currentLineLength + dropSpeed * Time.deltaTime;
            float newClampedLineLength = Mathf.Clamp(newLineLength, 0, maxLineLength);

            springJoint.maxDistance = newClampedLineLength;
            springJoint.connectedBody.WakeUp();
            currentLineLength = newClampedLineLength;
        }

        private void ReelHook()
        {
            float newLineLength = currentLineLength - reelingSpeed * Time.deltaTime;
            float newClampedLineLength = Mathf.Clamp(newLineLength, 0, maxLineLength);
            if (newClampedLineLength <= 0 && Hook.instance.FishOnHook != null && Hook.instance.FishOnHook.states.Biting.CurrentState == FishBitingState.onhook)
            {
                FishBrain fish = Hook.instance.FishOnHook;
                EventManager.OnFishCaught(fish.fishData, fish.fishSize);
            }

            springJoint.maxDistance = newClampedLineLength;
            springJoint.connectedBody.WakeUp();
            currentLineLength = newClampedLineLength;
        }

        public void SetLineLength(Vector2 fishpos)
        {
            float distance = Vector2.Distance(origin.transform.position, fishpos);
            distance += 0.1f;
            float newLineLength = distance;
            float newClampedLineLength = Mathf.Clamp(newLineLength, 0, maxLineLength);

            springJoint.maxDistance = newClampedLineLength;
            springJoint.connectedBody.WakeUp();
            currentLineLength = newClampedLineLength;
        }
        
        private void OnDock()
        {
            rodEnabled = false;
            currentLineLength = 0;
            springJoint.maxDistance = 0;
            springJoint.connectedBody.velocity = Vector3.zero;
        }

        private void OnUndock()
        {
            rodEnabled = true;
            currentLineLength = 0;
            springJoint.maxDistance = 0;
            springJoint.connectedBody.velocity = Vector3.zero;
            springJoint.connectedBody.WakeUp();
        }

        private void UpgradeBought(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case LineLengthUpgrade lineLengthUpgrade:
                    maxLineLength = lineLengthUpgrade.lineLength;
                    Hook.instance.Offset = lineLengthUpgrade.offset;
                    break;
                case ReelSpeedUpgrade reelSpeedUpgrade:
                    reelingSpeed = reelSpeedUpgrade.reelSpeed;
                    dropSpeed = reelSpeedUpgrade.dropSpeed;
                    break;
            }
        }

        private void OnPause(PauseState newState)
        {
            switch (newState)
            {
                case PauseState.Playing:
                    SetPause(false);
                    break;
                case PauseState.InPauseMenu:
                    SetPause(true);
                    break;  
                case PauseState.InInventory:
                    SetPause(true);
                    break;
                case PauseState.InCatalogue:
                    SetPause(true);
                    break;
                case PauseState.InQuests:
                    SetPause(true);
                    break;
            }
        }

        private void SetPause(bool isPaused)
        {
            rodEnabled = !isPaused;
            springJoint.connectedBody.isKinematic = isPaused;
        }
    }
}