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

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _input = GetBoatInput();
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
        }

        public void UndockBoat(Dock.Dock dockToUndock)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator MoveBoatToDock(Vector3 dockLocation)
        {
            while (Vector3.Distance(transform.position, dockLocation) > 0.3f)
            {
                // Get direction to target
                var boatPosition = transform.position;
                Vector3 direction = (dockLocation - boatPosition).normalized;

                // Calculate force based on distance and desired speed
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
            
            OnDockSuccess?.Invoke();
        }
    }
}