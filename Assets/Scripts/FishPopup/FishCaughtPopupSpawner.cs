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
        [SerializeField] private FishCaughtPopup fishCaughtPopupPrefab;
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
            FishCaughtPopup popup = Instantiate(fishCaughtPopupPrefab, spawnLocation);
            Sprite sprite = fishCaughtSprites[(int)data.fishRarity];
            Sprite sizeSprite = fishSizeSprites[(int)size];
            Color color = fishRarityColors[(int)data.fishRarity];
            popup.InitPopup(data, sprite, sizeSprite, color);
        }
    }
}