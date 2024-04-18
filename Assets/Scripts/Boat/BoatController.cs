using UnityEngine;

namespace Boat
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MonoBehaviour
    {
        [SerializeField] private float maxVelocity;
        [SerializeField] private float speedMultiplier;
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

            //check if velocity succeeds max speed and counteracts it.
            if (_rigidbody.velocity.magnitude < maxVelocity) return;

            var velocity = _rigidbody.velocity;
            var counteractForce = velocity.normalized * (maxVelocity - velocity.magnitude);
            _rigidbody.AddForce(counteractForce);
        }
    }
}
