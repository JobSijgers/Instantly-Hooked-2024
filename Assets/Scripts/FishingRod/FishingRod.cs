using System;
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
        private float _reelingSpeed = 20;
        private float _dropSpeed = 20;
        private float _maxLineLength = 1000;
        private float _currentLineLength = 0;
        private bool _rodEnabled = true;
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
                ReelHook();
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

            _springJoint.maxDistance = newClampedLineLength;
            _springJoint.connectedBody.WakeUp();
            _currentLineLength = newClampedLineLength;
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