using System;
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
        [SerializeField] private Camera mainCamera;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            audioSource.volume = CalculateVolume();
        }

        private float CalculateVolume()
        {
            float distance = Vector3.Distance(transform.position, mainCamera.transform.position);
            float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);
            float t = 1 - Mathf.Clamp01(normalizedDistance);
            return Mathf.Lerp(minVolume, maxVolume, t);
        }
    }
}