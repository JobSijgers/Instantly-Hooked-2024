using System;
using Enums;
using Events;
using PauseMenu;
using UnityEngine;
using UnityEngine.Experimental.Audio;
using Upgrades;

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
        public float GetLineLenght() => maxLineLength;
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
            if (Input.GetMouseButton(0))
            {
                CastHook();
            }

            if (Input.GetMouseButton(1))
            {
                if (Hook.instance.FishOnHook == null || !Hook.instance.FishOnHook.IsStruggeling())
                {
                    ReelHook();
                }
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
            if (newClampedLineLength <= 0 && Hook.instance.FishOnHook != null)
            {
                EventManager.OnFishCaught(Hook.instance.FishOnHook.fishData, GetRandomFishSize());
                FishPooler.instance.ReturnFish(Hook.instance.FishOnHook);
                Hook.instance.FishOnHook = null;
            }
            springJoint.maxDistance = newClampedLineLength;
            springJoint.connectedBody.WakeUp();
            currentLineLength = newClampedLineLength;
        }
        public void SetLineLength(Vector2 fishpos)
        {
            float distance = Vector2.Distance(origin.transform.position, fishpos);
            float newLineLength = distance;
            float newClampedLineLength = Mathf.Clamp(newLineLength, 0, maxLineLength);

            springJoint.maxDistance = newClampedLineLength;
            springJoint.connectedBody.WakeUp();
            currentLineLength = newClampedLineLength;
        }
        private FishSize GetRandomFishSize()
        {
            var fish = Enum.GetValues(typeof(FishSize));
            return (FishSize)fish.GetValue(UnityEngine.Random.Range(0, fish.Length));
        }
        
        private void OnDock()
        {
            rodEnabled = false;
            springJoint.maxDistance = 0;
        }

        private void OnUndock()
        {
            rodEnabled = true;
        }

        private void UpgradeBought(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case LineLengthUpgrade lineLengthUpgrade:
                    maxLineLength = lineLengthUpgrade.lineLength;
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
                    rodEnabled = true;
                    springJoint.connectedBody.isKinematic = false;
                    break;
                case PauseState.InPauseMenu:
                    rodEnabled = false;
                    springJoint.connectedBody.isKinematic = true;
                    break;
                case PauseState.InInventory:
                    springJoint.connectedBody.isKinematic = true;
                    rodEnabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }
}