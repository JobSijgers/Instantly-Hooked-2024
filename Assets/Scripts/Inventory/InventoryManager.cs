using System;
using System.Collections.Generic;
using Enums;
using Events;
using Fish;
using UnityEditor.UI;
using UnityEngine;

namespace Inventory
{
    // This class manages the player's inventory, including adding and removing fish, and updating the UI.
    public class InventoryManager : MonoBehaviour
    {
        private const int InitialFishCount = 1;
        public static InventoryManager instance;

        private readonly Dictionary<(FishData, FishSize), int> fishInventory = new();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            EventManager.FishCaught += AddOrIncrementFish;
            EventManager.PlayerDied += ClearInventory;
        }

        private void OnDestroy()
        {
            EventManager.PlayerDied -= ClearInventory;
            EventManager.FishCaught -= AddOrIncrementFish;
        }


        /// <summary>
        /// This method adds a fish to the inventory. If the fish is already in the inventory, it increases the stack size.
        /// </summary>
        private void AddOrIncrementFish(FishData fishToAdd, FishSize size)
        {
            (FishData fishToAdd, FishSize size) key = (fishToAdd, size);
            if (!fishInventory.TryAdd(key, InitialFishCount))
            {
                fishInventory[key]++;
            }
        }

        /// <summary>
        /// This method removes a specified amount of a specific type and size of fish from the inventory.
        /// </summary>
        public void RemoveFish(FishData fishToRemove, FishSize size, int amount)
        {
            (FishData fishToAdd, FishSize size) key = (fishToRemove, size);
            if (!fishInventory.ContainsKey(key))
                return;

            fishInventory[key] -= amount;
            if (fishInventory[key] <= 0)
            {
                fishInventory.Remove(key);
            }
        }

        private void ClearInventory()
        {
            fishInventory.Clear();
        }


        public Dictionary<(FishData, FishSize), int> GetInventory()
        {
            return fishInventory;
        }

        /// <summary>
        /// Returns the total count of a specific type of fish in the inventory.
        /// </summary>
        public int GetFishCount(FishData data)
        {
            int count = 0;

            // Loop through all possible fish sizes and add the count of each size to the total count
            FishSize[] sizes = (FishSize[])Enum.GetValues(typeof(FishSize));
            foreach (FishSize size in sizes)
            {
                (FishData data, FishSize size) key = (data, size);
                if (fishInventory.TryGetValue(key, out int value))
                {
                    count += value;
                }
            }

            return count;
        }
    }
}