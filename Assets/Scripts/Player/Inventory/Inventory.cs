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
            foreach (var inventoryItem in GetInventory())
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
            List<InventoryItem> itemsToDelete = new();
            if (fishToRemove == null)
                return;
            foreach (var item in GetInventory())
            {
                if (item == null) continue;

                if (item.GetFishData() != fishToRemove) continue;

                if (item.GetFishSize() != size) continue;

                if (remainingAmount < item.GetStackSize())
                {
                    item.UpdateStackSize(-remainingAmount);
                    DeleteFishItem(itemsToDelete.ToArray());
                    RecalculateItemStacks(item.GetFishData(), item.GetFishSize());
                    return;
                }

                remainingAmount -= item.GetStackSize();
                itemsToDelete.Add(item);

                if (remainingAmount > 0) continue;
                
                DeleteFishItem(itemsToDelete.ToArray());
                RecalculateItemStacks(fishToRemove, size);
                return;
            }
        }

        private void DeleteFishItem(InventoryItem[] items)
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
                currentFish.Remove(item);
            }
        }

        private void RecalculateItemStacks(FishData data, FishSize size)
        {
            int totalAmountOfFish = 0;
            List<InventoryItem> itemsContainingFish = new();
            foreach (var item in GetInventory())
            {
                if (item == null) continue;

                if (item.GetFishData() != data) continue;

                if (item.GetFishSize() != size) continue;
                
                totalAmountOfFish += item.GetStackSize();
                itemsContainingFish.Add(item);
            }

            if (totalAmountOfFish == 0)
            {
                return;
            }

            float amountOfSlots = (float)totalAmountOfFish / (float)data.maxStackAmount;
            int amountOfSlotsRounded = Mathf.CeilToInt(amountOfSlots);
            for (int i = 0; i < amountOfSlotsRounded; i++)
            {
                if (totalAmountOfFish - data.maxStackAmount >= 0)
                {
                    itemsContainingFish[i].SetStackSize(data.maxStackAmount);
                    totalAmountOfFish -= data.maxStackAmount;
                    if (totalAmountOfFish == 0)
                    {
                        break;
                    }
                }
                else
                {
                    itemsContainingFish[i].SetStackSize(totalAmountOfFish);
                    break;
                }
            }

            List<InventoryItem> itemsToDelete = new();
            for (int i =  amountOfSlotsRounded; i < itemsContainingFish.Count; i++)
            {
                itemsToDelete.Add(itemsContainingFish[i]);
            }

            DeleteFishItem(itemsToDelete.ToArray());
        }
        
        public InventoryItem[] GetInventory()
        {
            return currentFish.ToArray();
        }
    }
}
