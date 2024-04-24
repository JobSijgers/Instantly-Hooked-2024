using System;
using Events;
using UnityEngine;
using UnityEngine.Experimental.Audio;
using Upgrades;

namespace FishingRod
{
    [RequireComponent(typeof(SpringJoint))]
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] private Transform hook;
        private SpringJoint _springJoint;
        private float _reelingSpeed = 5;
        private float _dropSpeed = 3;
        private float _maxLineLength = 5;
        private float _currentLineLength = 0;
        private bool _rodEnabled = true;
        private void Start()
        {
            EventManager.UpgradeBought += UpgradeBought;
            EventManager.Dock += OnDock;
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
            EventManager.UpgradeBought -= UpgradeBought;
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

        private void UpgradeBought(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case LineLengthUpgrade lineLengthUpgrade:
                    _maxLineLength = lineLengthUpgrade.lineLength;
                    break;
                case ReelSpeedUpgrade reelSpeedUpgrade:
                    _reelingSpeed = reelSpeedUpgrade.reelSpeed;
                    _dropSpeed = reelSpeedUpgrade.dropSpeed;
                    break;
            }
        }
    }
}