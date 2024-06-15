using System;
using Events;
using UnityEngine;

namespace Generic
{
    public class SpawnAtTime : MonoBehaviour
    {
        [SerializeField] private GameObject objectToSpawn;
        [SerializeField] private int spawnMinutes;
        [SerializeField] private float despawnAfterSeconds;
        [SerializeField] private bool destroyOnSpawn;
        private bool hasSpawned;

        private void OnEnable()
        {
            EventManager.TimeUpdate += CheckTime;
            EventManager.NewDay += ResetSpawn;
        }

        private void ResetSpawn(int arg0)
        {
            hasSpawned = false;
        }

        private void OnDisable()
        {
            EventManager.TimeUpdate -= CheckTime;
            EventManager.NewDay -= ResetSpawn;
        }

        private void CheckTime(TimeSpan time)
        {
            if (hasSpawned) return;
            if (!(time.TotalMinutes >= spawnMinutes)) return;
            
            GameObject go = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            Destroy(go, despawnAfterSeconds);
            hasSpawned = true;
            if (!destroyOnSpawn) return;
            Destroy(gameObject);
        }
    }
}