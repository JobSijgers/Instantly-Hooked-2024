using System.Collections.Generic;
using System.Linq;
using Book;
using Catalogue;
using Enums;
using Fish;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace Inventory
{
    public class InventoryBookUI : BookPage
    {
        [SerializeField] private InventoryItem defaultInventorySlot;
        [SerializeField] private Transform itemHolder;
        [SerializeField] private FishUIHighlight highlight;
        [SerializeField] private FishRarityColor rarityColors;
        [SerializeField] private FishRaritySprites raritySprites;

        private const int DefaultCapacity = 30;
        private const int MaxCapacity = 1000;

        private IObjectPool<InventoryItem> inventoryItemPool;
        private readonly List<InventoryItem> activeItems = new();

        public override void Initialize()
        {
            base.Initialize();
            inventoryItemPool = new ObjectPool<InventoryItem>(CreateInventoryItem, InitializeItem, OnReleaseItem,
                OnDestroyItem, true, DefaultCapacity, MaxCapacity);
            highlight.DisableHolder();
        }

        public override void Show()
        {
            base.Show();
            GenerateInventoryUI();
        }

        public override void Hide()
        {
            base.Hide();
            foreach (InventoryItem inventoryItem in activeItems)
            {
                inventoryItemPool.Release(inventoryItem);
            }

            activeItems.Clear();
        }

        private Color GetRarityColor(FishRarity rarity)
        {
            return rarityColors.GetRarityColor(rarity);
        }

        private void HighlightItem(FishData fishData)
        {
            highlight.EnableHolder();
            highlight.Initialize(fishData, InventoryManager.instance.GetFishCount(fishData),
                raritySprites.GetRaritySprite(fishData.fishRarity));
        }

        private void GenerateInventoryUI()
        {
            Dictionary<(FishData, FishSize), int> inventory = InventoryManager.instance.GetInventory();
            FishData lastFishData = null;
            foreach (((FishData data, FishSize size), int amount) in inventory)
            {
                int remainingAmount = amount % data.maxStackAmount;

                if (amount > data.maxStackAmount)
                {
                    int stacks = amount / data.maxStackAmount;

                    for (int i = 0; i < stacks; i++)
                    {
                        InventoryItem fullStacks = inventoryItemPool.Get();
                        activeItems.Add(fullStacks);
                        fullStacks.Initialize(data, size, GetRarityColor(data.fishRarity), data.maxStackAmount,
                            HighlightItem);
                    }
                }

                InventoryItem remaining = inventoryItemPool.Get();
                activeItems.Add(remaining);
                remaining.Initialize(data, size, GetRarityColor(data.fishRarity), remainingAmount, HighlightItem);
                lastFishData = data;
            }

            if (lastFishData == null)
            {
                highlight.DisableHolder();
            }
            else
            {
                highlight.EnableHolder();
                highlight.Initialize(lastFishData, InventoryManager.instance.GetFishCount(lastFishData),
                    raritySprites.GetRaritySprite(lastFishData.fishRarity));
            }
        }

        private InventoryItem CreateInventoryItem() => Instantiate(defaultInventorySlot, itemHolder);
        private void InitializeItem(InventoryItem item) => item.gameObject.SetActive(true);
        private void OnReleaseItem(InventoryItem item) => item.gameObject.SetActive(false);
        private void OnDestroyItem(InventoryItem item) => Destroy(item.gameObject);
    }
}