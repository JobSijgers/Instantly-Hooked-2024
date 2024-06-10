using System;
using System.Collections;
using Interfaces;
using PathCreation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fish.Background_Fish
{
    public class BackgroundFishSpawner : MonoBehaviour
    {
        [Serializable]
        private struct BackgroundFish
        {
            public GameObject fish;
            public int fishCount;
        }

        [SerializeField] private PathCreator path;
        [SerializeField] private BackgroundFish[] fishToSpawn;
        [SerializeField] private Vector3 maxOffset;
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;

        private void Start()
        {
            StartCoroutine(SpawnAllFish());
        }

        private IEnumerator SpawnAllFish()
        {
            Vector3 beginPoint = path.path.GetPoint(0);

            foreach (BackgroundFish backgroundFish in fishToSpawn)
            {
                for (int i = 0; i < backgroundFish.fishCount; i++)
                {
                    GameObject fish = SpawnFish(backgroundFish.fish, beginPoint);
                    IBackgroundFish pathFollower = fish.AddComponent<PathFollower>();
                    pathFollower?.Initialize(path, GetRandomOffset(), GetRandomSpeed());
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        private GameObject SpawnFish(GameObject fish, Vector3 beginPoint)
        {
            GameObject go = Instantiate(fish, transform);
            go.transform.position = beginPoint;
            return go;
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