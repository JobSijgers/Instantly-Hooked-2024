using System.Collections.Generic;
using Catalogue;
using Enums;
using Events;
using Fish;
using PauseMenu;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Player.Inventory
{
    // This class manages the player's inventory, including adding and removing fish, and updating the UI.
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        // List of current fish items in the player's inventory.
        public List<InventoryItem> currentFish;

        [SerializeField] private GameObject defaultInventorySlot;
        [SerializeField] private Transform inventoryParent;
        [SerializeField] private Color[] rarityColors;
        [SerializeField] private Transform inventoryUI;
        [SerializeField] private Sprite[] raritySprites;
        [SerializeField] private CatalogueUIItem highlight;


        private void Awake()
        {
            Instance = this;
            EventManager.FishCaught += AddFish;
        }

        private void Start()
        {
            EventManager.PauseStateChange += OnPause;
            EventManager.PlayerDied += ClearInventory;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
            EventManager.PlayerDied -= ClearInventory;
            EventManager.FishCaught -= AddFish;
        }


        /// <summary>
        /// This method adds a fish to the inventory. If the fish is already in the inventory, it increases the stack size.
        /// </summary>
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

        /// <summary>
        /// This method removes a specified amount of a specific type and size of fish from the inventory.
        /// </summary>
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


        /// <summary>
        /// This method deletes a specific inventory item and removes it from the current fish list.
        /// </summary>
        /// <param name="items"></param>
        private void DeleteFishItem(InventoryItem[] items)
        {
            foreach (InventoryItem item in items)
            {
                Destroy(item.gameObject);
                currentFish.Remove(item);
            }
        }

        /// <summary>
        /// This method recalculates the stack sizes of a specific type and size of fish in the inventory.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
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


        private int GetFishCount(FishData data)
        {
            int runningTotal = 0;
            foreach (InventoryItem item in currentFish)
            {
                if (item.GetFishData() != data)
                    continue;

                runningTotal += item.GetStackSize();
            }

            return runningTotal;
        }

        /// <summary>
        /// This method returns an array of all current fish items in the inventory.
        /// </summary>
        /// <returns></returns>
        public InventoryItem[] GetInventory()
        {
            return currentFish.ToArray();
        }

        /// <summary>
        /// This method returns the color associated with a specific fish rarity.
        /// </summary>
        public Color GetRarityColor(FishRarity rarity)
        {
            return rarityColors[(int)rarity];
        }

        /// <summary>
        /// This method handles the inventory UI based on the current game pause state.
        /// </summary>
        /// <param name="newState"></param>
        private void OnPause(PauseState newState)
        {
            switch (newState)
            {
                case PauseState.Playing:
                    CloseInventory(true);
                    break;
                case PauseState.InPauseMenu:
                    CloseInventory(true);
                    break;
                case PauseState.InInventory:
                    OpenInventory(true);
                    break;
                case PauseState.InCatalogue:
                    CloseInventory(true);
                    break;
                case PauseState.InQuests:
                    CloseInventory(true);
                    break;
            }
        }

        public void OpenInventory(bool suppressEvent)
        {
            inventoryUI.gameObject.SetActive(true);
            PauseManager.SetState(PauseState.InInventory, suppressEvent);
            if (currentFish.Count == 0 || currentFish == null)
            {
                ClearHightlight();
            }
            else
            {
                HighlightItem(currentFish[0].GetFishData());
            }
        }
        
        public void CloseInventory(bool suppressEvent = false)
        {
            if (!inventoryUI.gameObject.activeSelf)
                return;
            inventoryUI.gameObject.SetActive(false);
            PauseManager.SetState(PauseState.Playing, suppressEvent);
        }

        public void HighlightItem(FishData item)
        {
            highlight.gameObject.SetActive(true);
            highlight.Initialize(item.name, item.fishDescription, GetFishCount(item), item.habitat,
                raritySprites[(int)item.fishRarity], item.fishVisual);
        }
        
        private void ClearHightlight()
        {
            highlight.gameObject.SetActive(false);
        }
    }
}