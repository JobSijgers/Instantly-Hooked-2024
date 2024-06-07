using System;
using Enums;
using Events;
using Fish;
using UnityEngine;

namespace FishPopup
{
    public class FishCaughtPopupSpawner : MonoBehaviour
    {
        [SerializeField] private Sprite[] fishCaughtSprites;
        [SerializeField] private Sprite[] fishSizeSprites;
        [SerializeField] private Color[] fishRarityColors;
        [SerializeField] private GameObject fishCaughtPopupPrefab;
        [SerializeField] private Transform spawnLocation;
        
        private void OnEnable()
        {
            EventManager.FishCaught += SpawnPopup;
        }

        private void OnDisable()
        {
            EventManager.FishCaught -= SpawnPopup;
        }

        private void SpawnPopup(FishData data, FishSize size)
            {
                GameObject go = Instantiate(fishCaughtPopupPrefab, spawnLocation.position, Quaternion.identity);
                go.transform.SetParent(transform);
                Sprite sprite = fishCaughtSprites[(int)data.fishRarity];
                Sprite sizeSprite = fishSizeSprites[(int)size];
                Color color = fishRarityColors[(int)data.fishRarity];
                go.GetComponent<FishCaughtPopup>()?.InitPopup(data, sprite, sizeSprite, color);
                Debug.Log("test");
            }
        }
    }