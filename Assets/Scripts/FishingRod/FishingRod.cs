using System;
using Enums;
using Events;
using PauseMenu;
using UnityEngine;

namespace FishingRod
{
    [RequireComponent(typeof(SpringJoint))]
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private Transform hook;
        private SpringJoint _springJoint;
        private float _reelingSpeed = 5;
        private float _dropSpeed = 5;
        private float _maxLineLength = 1000;
        private float _currentLineLength = 0;
        private bool _rodEnabled = true;
        public float GetLineLenght() => _maxLineLength;
        private void Start()
        {
            EventManager.Dock += OnDock;
            EventManager.PauseStateChange += OnPause;
            _springJoint = GetComponent<SpringJoint>();

            _springJoint.connectedBody = hook.GetComponent<Rigidbody>();
            if (_springJoint.connectedBody == null)
            {
                Debug.LogError("No rigidbody attached to hook");
            }

            _springJoint.maxDistance = 0;
        }

        private void OnDestroy()
        {
            EventManager.Dock -= OnDock;
            EventManager.UnDock -= OnUndock;
            EventManager.PauseStateChange -= OnPause;
        }

        private void Update()
        {
            if (!_rodEnabled)
                return;
            if (Input.GetMouseButton(0))
            {
                CastHook();
            }

            if (Input.GetMouseButton(1))
            {
                if (Hook.FishOnHook == null || !Hook.FishOnHook.IsStruggeling())
                {
                    ReelHook();
                }
            }
        }

        private void CastHook()
        {
            var newLineLength = _currentLineLength + _dropSpeed * Time.deltaTime;
            var newClampedLineLength = Mathf.Clamp(newLineLength, 0, _maxLineLength);

            _springJoint.maxDistance = newClampedLineLength;
            _springJoint.connectedBody.WakeUp();
            _currentLineLength = newClampedLineLength;
        }

        private void ReelHook()
        {
            var newLineLength = _currentLineLength - _reelingSpeed * Time.deltaTime;
            var newClampedLineLength = Mathf.Clamp(newLineLength, 0, _maxLineLength);
            if (newClampedLineLength <= 0 && Hook.FishOnHook != null)
            {
                EventManager.OnFishCaught(Hook.FishOnHook.fishData, GetRandomFishSize());
                FishPooler.instance.ReturnFish(Hook.FishOnHook);
                Hook.FishOnHook = null;
            }
            _springJoint.maxDistance = newClampedLineLength;
            _springJoint.connectedBody.WakeUp();
            _currentLineLength = newClampedLineLength;
        }
        public void SetLineLength(Vector2 fishpos)
        {
            float Distace = Vector2.Distance(origin.transform.position, fishpos);
            var newLineLength = Distace;
            var newClampedLineLength = Mathf.Clamp(newLineLength, 0, _maxLineLength);

            _springJoint.maxDistance = newClampedLineLength;
            _springJoint.connectedBody.WakeUp();
            _currentLineLength = newClampedLineLength;
        }
        private FishSize GetRandomFishSize()
        {
            var fish = Enum.GetValues(typeof(FishSize));
            return (FishSize)fish.GetValue(UnityEngine.Random.Range(0, fish.Length));
        }
        private void OnDock()
        {
            EventManager.UnDock += OnUndock;
            _rodEnabled = false;
        }

        private void OnUndock()
        {
            EventManager.UnDock -= OnUndock;
            _rodEnabled = true;
        }

        private void OnPause(PauseState newState)
        {
            _springJoint.connectedBody.isKinematic = newState switch
            {
                PauseState.Playing => false,
                PauseState.InPauseMenu => true,
                PauseState.InInventory => true,
                _ => throw new ArgumentOutOfRangeException(nameof(newState), newState, null)
            };
        }
    }
}