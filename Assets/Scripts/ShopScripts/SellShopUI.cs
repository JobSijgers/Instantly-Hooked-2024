using System.Collections.Generic;
using System.Text;
using Enums;
using Events;
using Fish;
using Inventory;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using Views;

namespace ShopScripts
{
    public class SellShopUI : View
    {
        private class ReceiptItem
        {
            public TMP_Text Text;
            public int Amount;
        }


        [Header("Shop Items")]
        [SerializeField] private SellShopItem shopItemPrefab;
        [SerializeField] private Transform itemHolder;
        [SerializeField] private FishRarityColor fishRarityColor;
        [Header("Sell Receipt")]
        [SerializeField] private TMP_Text receiptTextPrefab;
        [SerializeField] private Transform receiptParent;
        [SerializeField] private TMP_Text totalSellMoneyText;
        [SerializeField] private TMP_Text totalSellAmountText;
        
        private readonly List<SellShopItem> activeSellShopItems = new();
        private readonly Dictionary<(FishData, FishSize), ReceiptItem> receipt = new();

        private int totalSellMoney;
        private int totalSellAmount;

        public override void Show()
        {
            base.Show();
            LoadSellShop();
        }

        public override void Hide()
        {
            base.Hide();
            foreach (SellShopItem shopItem in activeSellShopItems)
            {
                Destroy(shopItem.gameObject);
            }

            activeSellShopItems.Clear();
            ResetReceiptTotal();
        }

        public void SelectAll()
        {
            ResetReceiptTotal();
            ClearReceipt();
            foreach (SellShopItem item in activeSellShopItems)
            {
                item.SetInputField(item.GetStackSize());
                UpdateReceipt(item, item.GetStackSize());
            }
            UpdateReceiptTotalUI();
        }

        private void LoadSellShop()
        {
            ClearReceipt();
            Dictionary<(FishData, FishSize), int> inventory = InventoryManager.instance.GetInventory();
            foreach (((FishData data, FishSize size), int amount) in inventory)
            {
                CreateAndInitializeSellShopItems(data, size, amount);
            }
        }

        private void CreateAndInitializeSellShopItems(FishData data, FishSize size, int amount)
        {
            int remainingAmount = amount % data.maxStackAmount;
            
            SellShopItem remaining = Instantiate(shopItemPrefab, itemHolder);
            activeSellShopItems.Add(remaining);
            remaining.Initialize(data, size, fishRarityColor.GetRarityColor(data.fishRarity), remainingAmount,
                UpdateReceipt);

            if (amount <= data.maxStackAmount) return;
            
            int stacks = amount / data.maxStackAmount;

            for (int i = 0; i < stacks; i++)
            {
                SellShopItem fullStacks = Instantiate(shopItemPrefab, itemHolder);
                activeSellShopItems.Add(fullStacks);
                fullStacks.Initialize(data, size, fishRarityColor.GetRarityColor(data.fishRarity), data.maxStackAmount,
                    UpdateReceipt);
            }
        }
        
        /// <summary>
        /// Updates the shopping list when the selected amount of an item changes.
        /// </summary>
        private void UpdateReceipt(SellShopItem item, int change)
        {
            FishData data = item.FishData;
            FishSize size = item.Size;
            (FishData FishData, FishSize Size) key = (data, size);
            if (receipt.TryGetValue(key, out ReceiptItem receiptItem))
            {
                if (receiptItem.Amount + change <= 0)
                {
                    RemoveFromReceipt(item, receiptItem);
                    return;
                }

                UpdateReceiptItem(data, item.Size, receiptItem, change);
                return;
            }
            AddNewItemToReceipt(data, item.Size, change);
        }

        private void RemoveFromReceipt(SellShopItem item, ReceiptItem receiptItem)
        {
            Destroy(receiptItem.Text.gameObject);
            activeSellShopItems.Remove(item);
            ChangeReceiptTotal(-receiptItem.Amount, item.FishData, item.Size);
            UpdateShoppingListUI();
        }

        private void UpdateReceiptItem(FishData data, FishSize size,ReceiptItem receiptItem, int change)
        {
            receiptItem.Amount += change;
            receiptItem.Text.text = GetSellListText(data, size, receiptItem.Amount);
            receipt[(data, size)] = receiptItem;
            ChangeReceiptTotal(change, data, size);
            UpdateShoppingListUI();
        }

        private void AddNewItemToReceipt(FishData data, FishSize size, int amount)
        {
            TMP_Text text = Instantiate(receiptTextPrefab, receiptParent);
            receipt.Add((data, size), new ReceiptItem { Text = text, Amount = amount });
            ChangeReceiptTotal(amount, data, size);
            UpdateShoppingListUI();
        }
        /// <summary>
        /// Updates the shopping list UI to reflect the current state of the shopping list.
        /// </summary>
        private void UpdateShoppingListUI()
        {
            foreach (((FishData fishData, FishSize fishSize), ReceiptItem value) in receipt)
            {
                value.Text.text = GetSellListText(fishData, fishSize, value.Amount);
            }
        }
        
        private void ClearReceipt()
        {
            ResetReceiptTotal();
            foreach (ReceiptItem item in receipt.Values)
            {
                Destroy(item.Text.gameObject);
            }

            receipt.Clear();
        }

        /// <summary>
        /// Returns a string representing a sell list item.
        /// </summary>
        private string GetSellListText(FishData data, FishSize size, int amount)
        {
            StringBuilder sb = new();
            sb.Append(amount);
            sb.Append(" x ");
            sb.Append(data.fishName);
            sb.Append(", ");
            sb.Append(size);
            sb.Append(" : $");
            sb.Append(amount * data.fishSellAmount[(int)size]);
            return sb.ToString();
        }

        private void ChangeReceiptTotal(int change, FishData data, FishSize size)
        {
            totalSellMoney += change * data.fishSellAmount[(int)size];
            totalSellAmount += change;
            UpdateReceiptTotalUI();
        }

        private void ResetReceiptTotal()
        {
            totalSellAmount = 0;
            totalSellMoney = 0;
            UpdateReceiptTotalUI();
        }

        private void UpdateReceiptTotalUI()
        {
            totalSellMoneyText.text = $"${totalSellMoney}";
            totalSellAmountText.text = totalSellAmount.ToString();
        }

        public void SellSelectedItems()
        {
            EventManager.OnShopSell(totalSellAmount);
            foreach (((FishData data, FishSize size), ReceiptItem item) in receipt)
            {
                InventoryManager.instance.RemoveFish(data, size, item.Amount);
            }

            Hide();
            Show();
        }
    }
}