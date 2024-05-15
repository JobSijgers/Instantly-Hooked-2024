using System;
using System.Collections.Generic;
using Enums;
using Events;
using Fish;
using PauseMenu;
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
        [SerializeField] private Transform inventoryUI;

        private void Awake()
        {
            Instance = this;
            EventManager.FishCaught += AddFish;
        }

        private void Start()
        {
            EventManager.PauseStateChange += OnPause;
            EventManager.PlayerDied += ClearInventory;

            inventoryUI.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
            EventManager.PlayerDied -= ClearInventory;
            EventManager.FishCaught -= AddFish;
        }

        private void AddFish(FishData fishToAdd, FishSize size)
        {
            if (fishToAdd.maxStackAmount <= 1) return;
            foreach (InventoryItem inventoryItem in GetInventory())
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
            int remainingAmount = amount;
            List<InventoryItem> itemsToDelete = new();
            if (fishToRemove == null)
                return;
            foreach (InventoryItem item in GetInventory())
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
            foreach (InventoryItem item in items)
            {
                Destroy(item.gameObject);
                currentFish.Remove(item);
            }
        }

        private void RecalculateItemStacks(FishData data, FishSize size)
        {
            int totalAmountOfFish = 0;
            List<InventoryItem> itemsContainingFish = new();
            foreach (InventoryItem item in GetInventory())
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
        
        private void ClearInventory()
        {
            foreach (InventoryItem item in GetInventory())
            {
                Destroy(item.gameObject);
            }
            currentFish.Clear();
        }

        public InventoryItem[] GetInventory()
        {
            return currentFish.ToArray();
        }

        public Color GetRarityColor(FishRarity rarity)
        {
            return rarityColors[(int)rarity];
        }
        
        private void OnPause(PauseState newState)
        {
            switch (newState)
            {
                case PauseState.Playing:
                    inventoryUI.gameObject.SetActive(false);
                    break;
                case PauseState.InPauseMenu:
                    break;
                case PauseState.InInventory:
                    inventoryUI.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }
}