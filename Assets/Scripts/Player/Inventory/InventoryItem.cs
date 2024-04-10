using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        protected FishData fishData;    
        protected FishSize size;
        protected int stackSize = 1;
        [SerializeField] protected TMP_Text stackSizeText;
        [SerializeField] protected Image fishImage;

        public void Initialize(FishData newFishData, FishSize fishSize)
        {
            fishData = newFishData;
            fishImage.sprite = newFishData.fishVisual;
            UpdateStackUI();
            size = fishSize;
        }

        public void UpdateStackSize(int amount)
        {
            stackSize += amount;
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
            return size;
        }

        public int GetStackSize()
        {
            return stackSize;
        }

        protected void UpdateStackUI()
        {
            stackSizeText.text = stackSize.ToString();
        }
    }
}