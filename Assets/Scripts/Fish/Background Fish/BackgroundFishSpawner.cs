using System.Collections;
using System.Collections.Generic;
using Interfaces;
using PathCreation;
using UnityEngine;

namespace Fish.Background_Fish
{
    public class BackgroundFishSpawner : MonoBehaviour
    {
        [SerializeField] private PathCreator[] paths;
        [SerializeField] private GameObject[] fishToSpawn;
        [SerializeField] private int fishCount;
        [SerializeField] private Vector3 maxOffset;
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;

        private void Start()
        {
            StartCoroutine(SpawnAllFish());
        }
        private IEnumerator SpawnAllFish()
        {
            for (int i = 0; i < fishCount; i++)
            {
                GameObject fish = SpawnFish();
                IBackgroundFish pathFollower = fish.AddComponent<PathFollower>();
                pathFollower?.Initialize(GetRandomPath(), GetRandomOffset(), GetRandomSpeed());
                yield return new WaitForSeconds(0.1f);
            }
        }

        private GameObject SpawnFish()
        {
            return Instantiate(fishToSpawn[Random.Range(0, fishToSpawn.Length)], transform);
        }

        private PathCreator GetRandomPath()
        {
            int randomIndex = Random.Range(0, paths.Length);
            return paths[randomIndex];
        }

        private Vector3 GetRandomOffset()
        {
            float randomX = Random.Range(-maxOffset.x, maxOffset.x);
            float randomY = Random.Range(-maxOffset.y, maxOffset.y);
            float randomZ = Random.Range(-maxOffset.z, maxOffset.z);

            return new Vector3(randomX, randomY, randomZ);
        }

        private float GetRandomSpeed()
        {
            return Random.Range(minSpeed, maxSpeed);
        }
    }
}