using Events;
using PauseMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using Upgrades.Scriptable_Objects;

namespace FishingRod
{
    [RequireComponent(typeof(SpringJoint))]
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private Transform hook;
        [SerializeField] private int minLineLength;
        [SerializeField] private float seaLevel;
        private SpringJoint springJoint;
        private float reelingSpeed = 5;
        private float dropSpeed = 3;
        private float maxLineLength = 5;
        private float currentLineLength = 0;
        private bool rodEnabled = true;
        private float defaultOffset;
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
            
            currentLineLength = minLineLength;
            springJoint.maxDistance = minLineLength;
            defaultOffset = Vector2.Distance(origin.position, new Vector2(origin.position.x, seaLevel));
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
            if (!rodEnabled || EventSystem.current.IsPointerOverGameObject())
                return;
            if (Input.GetMouseButton(0) && !Hook.instance.touchingGround)
            {
                CastHook();
            }
            else if (Input.GetMouseButton(1))
            { 
                ReelHook();
            }
            else if (Hook.instance.touchingGround)
            {
                float newLineLength = Vector3.Distance(hook.position, origin.position) - 0.5f;
                float newClampedLineLength = Mathf.Clamp(newLineLength, minLineLength, maxLineLength);

                springJoint.maxDistance = newClampedLineLength;
                springJoint.connectedBody.WakeUp();
                currentLineLength = newClampedLineLength;
            }
        }

        private void CastHook()
        {
            float newLineLength = currentLineLength + dropSpeed * Time.deltaTime;
            float newClampedLineLength = Mathf.Clamp(newLineLength, minLineLength, maxLineLength);

            springJoint.maxDistance = newClampedLineLength;
            springJoint.connectedBody.WakeUp();
            currentLineLength = newClampedLineLength;
        }

        private void ReelHook()
        {
            float newLineLength = currentLineLength - reelingSpeed * Time.deltaTime;
            float newClampedLineLength = Mathf.Clamp(newLineLength, minLineLength , maxLineLength);
            if (newClampedLineLength <= minLineLength && Hook.instance.FishOnHook != null && Hook.instance.FishOnHook.states.Biting.CurrentState == FishBitingState.OnHook)
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
            float newClampedLineLength = Mathf.Clamp(newLineLength, minLineLength, maxLineLength);

            springJoint.maxDistance = newClampedLineLength;
            springJoint.connectedBody.WakeUp();
            currentLineLength = newClampedLineLength;
        }
        
        private void OnDock()
        {
            rodEnabled = false;
            currentLineLength = minLineLength;
            springJoint.maxDistance = minLineLength;
            springJoint.connectedBody.velocity = Vector3.zero;
        }

        private void OnUndock()
        {
            rodEnabled = true;
            springJoint.connectedBody.gameObject.transform.position = transform.position;
            currentLineLength = minLineLength;
            springJoint.maxDistance = minLineLength;
            springJoint.connectedBody.velocity = Vector3.zero;
            springJoint.connectedBody.WakeUp();
        }

        private void UpgradeBought(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case LineLengthUpgrade lineLengthUpgrade:
                    maxLineLength = lineLengthUpgrade.lineLength + defaultOffset;
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