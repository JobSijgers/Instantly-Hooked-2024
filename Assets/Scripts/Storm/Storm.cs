using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Interfaces;
using UnityEngine;

namespace Storm
{
    public class Storm : MonoBehaviour, IStorm
    {
        [SerializeField] private float stormDamageTime;
        private Vector3 stormStartLocation;
        private Vector3 stormEndLocation;
        private float stormDuration;
        private float playerInStormTime;
        private bool playerInStorm;

        private void Start()
        {
            EventManager.NewDay += DestroyStorm;
        }

        private void OnDestroy()
        {
            EventManager.NewDay -= DestroyStorm;
        }


        private void Update()
        {
            if (!playerInStorm)
                return;
            playerInStormTime += Time.deltaTime;
            if (!(playerInStormTime >= stormDamageTime)) return;
            
            EventManager.OnPlayerDied();
            playerInStormTime = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInStorm = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInStorm = false;
            }
        }

        public void InitStorm(float duration, Vector3 endLocation)
        {
            stormDuration = duration;
            stormEndLocation = endLocation;
            stormStartLocation = transform.position;

            StartCoroutine(MoveStorm());
        }

        private IEnumerator MoveStorm()
        {
            float t = 0;
            while (Vector3.Distance(transform.position, stormEndLocation) > 0.1f)
            {
                t += Time.deltaTime / stormDuration;
                Vector3 newLocation = Vector3.Lerp(stormStartLocation, stormEndLocation, t);
                transform.position = newLocation;
                yield return null;
            }
        }
        
        private void DestroyStorm(int newDay)
        {
            Destroy(gameObject);
        }
    }
}