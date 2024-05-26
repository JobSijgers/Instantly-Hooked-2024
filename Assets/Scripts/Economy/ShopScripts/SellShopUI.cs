using System.Collections.Generic;
using System.Text;
using Events;
using Fish;
using Player.Inventory;
using TMPro;
using UnityEngine;

namespace Economy.ShopScripts
{
    public class SellShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject shopItemPrefab;
        [SerializeField] private GameObject shopObject;
        [SerializeField] private Transform itemHolder;
        [SerializeField] private GameObject sellSheetTextPrefab;
        [SerializeField] private Transform sellSheetParent;
        [SerializeField] private TMP_Text totalSellMoney;
        [SerializeField] private TMP_Text totalSellAmount;


        private SellShopItem selectedItem;
        private readonly List<SellShopItem> shopItems = new();
        private readonly List<SellListItem> sellSheet = new();
        private readonly List<TMP_Text> sellTexts = new();
        private int currentTotalSellMoneyAmount;
        private int currentTotalSellAmount;

        private void Start()
        {
            EventManager.SellShopOpen += OpenSellShopUI;
            EventManager.SellShopClose += CloseSellShopUI;
            EventManager.LeftShore += CloseSellShopUI;
        }


        private void OnDestroy()
        {
            EventManager.SellShopOpen += OpenSellShopUI;
            EventManager.SellShopClose += CloseSellShopUI;
            EventManager.LeftShore += CloseSellShopUI;
        }

        private void OpenSellShopUI()
        {
            shopObject.SetActive(true);
            foreach (InventoryItem inventoryItem in Inventory.Instance.GetInventory())
            {
                GameObject go = Instantiate(shopItemPrefab, itemHolder);
                SellShopItem item = go.GetComponent<SellShopItem>();
                item.Initialize(inventoryItem.GetFishData(), inventoryItem.GetFishSize(), inventoryItem.GetStackSize(),
                    Inventory.Instance.GetRarityColor(inventoryItem.GetFishData().fishRarity));
                item.OnSelectedAmountChanged += UpdateShoppingList;
                shopItems.Add(item);
            }
        }

        private void CloseSellShopUI()
        {
            shopObject.SetActive(false);
            foreach (SellShopItem item in shopItems)
            {
                Destroy(item.gameObject);
            }

            shopItems.Clear();
        }

        public void CloseUI()
        {
            EventManager.OnSellShopClose();
        }

        public void SelectAll()
        {
            ClearSellSheet();
            currentTotalSellMoneyAmount = 0;
            currentTotalSellAmount = 0;
            foreach (SellShopItem item in shopItems)
            {
                item.SetInputField(item.GetStackSize());
                UpdateShoppingList(item, item.GetStackSize());
            }
        }

        /// <summary>
        /// Updates the shopping list when the selected amount of an item changes.
        /// </summary>
        private void UpdateShoppingList(SellShopItem item, int change)
        {
            FishData data = item.GetFishData();
            for (int i = 0; i < sellSheet.Count; i++)
            {
                SellListItem shoppingItem = sellSheet[i];

                if (shoppingItem.name != data.fishName)
                    continue;
                if (shoppingItem.size != item.GetFishSize())
                    continue;
                if (shoppingItem.amount + change <= 0)
                {
                    sellSheet.Remove(shoppingItem);
                    Destroy(sellTexts[^1].gameObject);
                    sellTexts.RemoveAt(sellTexts.Count - 1);
                    UpdateShoppingListUI();
                    return;
                }

                shoppingItem.amount += change;
                sellSheet[i] = shoppingItem;

                currentTotalSellMoneyAmount += change * item.GetFishData().fishSellAmount[(int)item.GetFishSize()];
                currentTotalSellAmount += change;
                UpdateShoppingListUI();
                return;
            }

            sellSheet.Add(new SellListItem(item, change));
            UpdateShoppingListUI();
        }

        /// <summary>
        /// Updates the shopping list UI to reflect the current state of the shopping list.
        /// </summary>
        private void UpdateShoppingListUI()
        {
            for (int i = 0; i < sellSheet.Count; i++)
            {
                if (i >= sellTexts.Count)
                {
                    GameObject go = Instantiate(sellSheetTextPrefab, sellSheetParent);
                    TMP_Text tmp = go.GetComponent<TMP_Text>();
                    tmp.text = GetSellListText(sellSheet[i]);
                    sellTexts.Add(tmp);
                }
                else
                {
                    sellTexts[i].text = GetSellListText(sellSheet[i]);
                }
            }

            totalSellMoney.text = $"${currentTotalSellMoneyAmount}";
            totalSellAmount.text = currentTotalSellMoneyAmount.ToString();
        }

        /// <summary>
        /// Returns a string representing a sell list item.
        /// </summary>
        private static string GetSellListText(SellListItem itemToSell)
        {
            StringBuilder sb = new();
            sb.Append(itemToSell.amount);
            sb.Append(" x ");
            sb.Append(itemToSell.name);
            sb.Append(", ");
            sb.Append(itemToSell.size);
            sb.Append(" : $");
            sb.Append(itemToSell.amount * itemToSell.singleCost);
            return sb.ToString();
        }

        public void SellSelectItems()
        {
            EventManager.OnSellSelectedButton(sellSheet.ToArray());
            ClearSellSheet();
            UpdateShoppingListUI();
            CloseSellShopUI();
            OpenSellShopUI();
        }

        private void ClearSellSheet()
        {
            foreach (TMP_Text text in sellTexts)
            {
                Destroy(text.gameObject);
            }

            sellSheet.Clear();
            sellTexts.Clear();
        }
    }
}