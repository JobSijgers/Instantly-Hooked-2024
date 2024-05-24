using Enums;
using Fish;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        protected FishData fishData;
        protected int stackSize = 1;
        private FishSize fishSize;

        [SerializeField] protected TMP_Text stackSizeText;
        [SerializeField] private TMP_Text fishSizeText;
        [SerializeField] protected Image fishImage;
        [SerializeField] private Image background;

        public void Initialize(FishData newFishData, FishSize newFishSize, Color backgroundColor)
        {
            fishData = newFishData;
            fishImage.sprite = newFishData.fishVisual;
            background.color = backgroundColor;
            fishSize = newFishSize;
            fishSizeText.text = newFishSize.ToString();
            UpdateStackUI();
        }

        public void UpdateStackSize(int change)
        {
            stackSize += change;
            UpdateStackUI();
        }

        public void SetStackSize(int newSize)
        {
            stackSize = newSize;
            UpdateStackUI();
        }

        public int GetRemainingStackSize()
        {
            return fishData.maxStackAmount - stackSize;
        }

        public FishData GetFishData()
        {
            return fishData;
        }

        public FishSize GetFishSize()
        {
            return fishSize;
        }

        public int GetStackSize()
        {
            return stackSize;
        }
        public Color GetColor()
        {
            return background.color;
        }

        protected void UpdateStackUI()
        {
            stackSizeText.text = stackSize.ToString();
        }
    }
}