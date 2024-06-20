using System;
using Cinemachine;
using Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySoundByDistance : MonoBehaviour
    {
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume;
        private Transform mainCamera;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            mainCamera = MainCamera.instance.mainCamera;
        }

        private void Update()
        {
            audioSource.volume = CalculateVolume();
        }

        private float CalculateVolume()
        {
            float distance = Vector3.Distance(transform.position, mainCamera.position);
            float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);
            float t = 1 - Mathf.Clamp01(normalizedDistance);
            return Mathf.Lerp(minVolume, maxVolume, t);
        }

        private void OnValidate()
        {
            if (minDistance > maxDistance)
            {
                minDistance = maxDistance;
            }

            if (minVolume > maxVolume)
            {
                minVolume = maxVolume;
            }
            
            audioSource = GetComponent<AudioSource>();
            audioSource.maxDistance = maxDistance;
        }
    }
}