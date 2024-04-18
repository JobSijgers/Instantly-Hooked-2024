using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace Boat
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MonoBehaviour, IBoat
    {
        public delegate void FDockSuccess();

        public event FDockSuccess OnDockSuccess;

        [SerializeField] private float maxVelocity;
        [SerializeField] private float speedMultiplier;
        [SerializeField] private float dockStoppingDistance;
        
        private Rigidbody _rigidbody;
        private float _input;
        private bool _inputEnabled = true;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_inputEnabled)
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
            var velocity = _rigidbody.velocity;
            var counteractForce = velocity.normalized * (maxVelocity - velocity.magnitude);
            _rigidbody.AddForce(counteractForce);
        }

        public void DockBoat(Vector3 dockLocation, Dock.Dock dock)
        {
            StartCoroutine(MoveBoatToDock(dockLocation));
            dock.OnUndockSuccess += UndockBoat;
            _inputEnabled = false;
        }

        public void UndockBoat(Dock.Dock dock)
        {
            dock.OnUndockSuccess -= UndockBoat;
            _inputEnabled = true;
        }

        private IEnumerator MoveBoatToDock(Vector3 dockLocation)
        {
            while (Vector3.Distance(transform.position, dockLocation) > 0.3f)
            {
                // Get direction to target
                var boatPosition = transform.position;
                var direction = (dockLocation - boatPosition).normalized;
                
                var distance = Vector3.Distance(boatPosition, dockLocation);
                var forceMagnitude = Mathf.Clamp(distance * speedMultiplier, 0f, Mathf.Infinity);

                var decelerationLerp = Mathf.Clamp01((distance - dockStoppingDistance) / dockStoppingDistance);
                forceMagnitude *= decelerationLerp;

                _rigidbody.AddForce(direction * forceMagnitude);
                LimitVelocity();

                yield return null;
            }

            // Stop movement at target
            _rigidbody.velocity = Vector3.zero;
            
            OnDockSuccess?.Invoke();
        }
    }
}