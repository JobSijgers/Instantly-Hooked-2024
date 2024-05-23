using System;
using Events;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Storm
{
    public class StormSpawner : MonoBehaviour
    {
        [SerializeField] private float stormDuration;
        [SerializeField] private int stormSpawnTime;
        [SerializeField] private GameObject stormPrefab;
        [SerializeField] private Transform stormSpawnLocation;
        [SerializeField] private Transform stormEndLocation;
        private bool isStormActive;
        
        private void OnEnable()
        {
            EventManager.TimeUpdate += CheckStormTime;
            EventManager.NewDay += ResetStorm;
        }
        
        private void OnDisable()
        {
            EventManager.TimeUpdate -= CheckStormTime;
            EventManager.NewDay -= ResetStorm;
        }

        private void CheckStormTime(float time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            if (timeSpan.Hours * 60 + timeSpan.Minutes >= stormSpawnTime && !isStormActive)
            {
                SpawnStorm();
            }
        }

        private void SpawnStorm()
        {
            GameObject go = Instantiate(stormPrefab, stormSpawnLocation.position, Quaternion.identity);
            go.GetComponent<IStorm>()?.InitStorm(stormDuration, stormEndLocation.position);
            isStormActive = true;
        }
        
        private void ResetStorm(int day)
        {
            isStormActive = false;
        }
    }
}