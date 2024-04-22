using System;
using System.Collections.Generic;
using Enums;
using Events;
using Fish;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        public List<InventoryItem> currentFish;

        [SerializeField] private GameObject defaultInventorySlot;
        [SerializeField] private Transform inventoryParent;
        [SerializeField] private Color[] rarityColors;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            EventManager.FishCaught += AddFish;
        }

        private void AddFish(FishData fishToAdd, FishSize size)
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
            item.Initialize(fishToAdd, size, GetRarityColor(fishToAdd.fishRarity));
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
                return;

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
            for (int i = amountOfSlotsRounded; i < itemsContainingFish.Count; i++)
            {
                itemsToDelete.Add(itemsContainingFish[i]);
            }

            DeleteFishItem(itemsToDelete.ToArray());
        }

        public InventoryItem[] GetInventory()
        {
            return currentFish.ToArray();
        }

        public Color GetRarityColor(FishRarity rarity)
        {
            return rarityColors[(int)rarity];
        }
    }
}