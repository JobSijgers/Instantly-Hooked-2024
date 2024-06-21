using System;
using Enums;
using Events;
using Fish;
using UnityEngine;

namespace Boat
{
    public class DecorationSpawner : MonoBehaviour
    {
        [Serializable]
        public struct SpecialFish
        {
            public GameObject decoration;
            public FishData data;
        }

        [SerializeField] private SpecialFish[] specialFish;

        private void Start()
        {
            EventManager.FishCaught += SpawnDecoration;
        }

        private void OnDestroy()
        {
            EventManager.FishCaught -= SpawnDecoration;
        }

        private void SpawnDecoration(FishData data, FishSize size)
        {
            foreach (SpecialFish special in specialFish)
            {
                if (special.data != data) continue;
                GameObject go = Instantiate(special.decoration, transform);
                go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }
    }
}