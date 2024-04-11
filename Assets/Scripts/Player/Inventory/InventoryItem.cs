using Enums;
using Fish;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        protected FishData FishData;
        protected int StackSize = 1;
        private FishSize _fishSize;

        [SerializeField] protected TMP_Text stackSizeText;
        [SerializeField] private TMP_Text fishSizeText;
        [SerializeField] protected Image fishImage;
        [SerializeField] private Image background;

        public void Initialize(FishData newFishData, FishSize newFishSize, Color backgroundColor)
        {
            FishData = newFishData;
            fishImage.sprite = newFishData.fishVisual;
            background.color = backgroundColor;
            _fishSize = newFishSize;
            fishSizeText.text = newFishSize.ToString();
            UpdateStackUI();
        }

        public void UpdateStackSize(int change)
        {
            StackSize += change;
            UpdateStackUI();
        }

        public void SetStackSize(int newSize)
        {
            StackSize = newSize;
            UpdateStackUI();
        }

        public int GetRemainingStackSize()
        {
            return FishData.maxStackAmount - StackSize;
        }

        public FishData GetFishData()
        {
            return FishData;
        }

        public FishSize GetFishSize()
        {
            return _fishSize;
        }

        public int GetStackSize()
        {
            return StackSize;
        }

        protected void UpdateStackUI()
        {
            stackSizeText.text = StackSize.ToString();
        }
    }
}