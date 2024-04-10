using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;

        public List<InventoryItem> currentFish;

        [SerializeField] private GameObject defaultInventorySlot;
        [SerializeField] private Transform inventoryParent;

        private void Awake()
        {
            instance = this;
        }

        public void AddFish(FishData fishToAdd, FishSize size)
        {
            if (fishToAdd.maxStackAmount <= 1) return;
            foreach (var inventoryItem in currentFish)
            {
                if (inventoryItem == null || inventoryItem.GetFishData() != fishToAdd ||
                    inventoryItem.GetFishSize() != size) continue;

                if (inventoryItem.GetRemainingStackSize() <= 0) continue;
                inventoryItem.UpdateStackSize(1);

                return;
            }

            GameObject go = Instantiate(defaultInventorySlot, inventoryParent);
            InventoryItem item = go.GetComponent<InventoryItem>();
            item.Initialize(fishToAdd, size);
            currentFish.Add(item);
        }

        public void RemoveFish(FishData fishToRemove, FishSize size, int amount)
        {
            var remainingAmount = amount;
            if (fishToRemove == null)
                return;
            foreach (var fish in currentFish)
            {
                if (fish != null)
                    return;

                if (fish.GetFishData() != fishToRemove || fish.GetFishSize() != size) continue;
                
                if (remainingAmount <= fish.GetStackSize())
                {
                    fish.UpdateStackSize(remainingAmount);
                }
                else
                {
                    remainingAmount -= fish.GetStackSize();
                    DeleteFishItem(fish);
                }
            }
        }

        private void DeleteFishItem(InventoryItem item)
        {
            Destroy(item.gameObject);
            currentFish.Remove(item);
        }
        public void ClearFish()
        {
            foreach (var fish in currentFish)
            {
                Destroy(fish.gameObject);
            }

            currentFish.Clear();
        }

        public InventoryItem[] GetInventory()
        {
            return currentFish.ToArray();
        }
    }
}