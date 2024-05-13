using System.Collections.Generic;
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
        private List<SellShopItem> shopItems = new();
        private List<SellListItem> sellSheet = new();
        private List<TMP_Text> sellTexts = new();

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

        public void CloseSellShopUI()
        {
            shopObject.SetActive(false);
            foreach (SellShopItem item in shopItems)
            {
                Destroy(item.gameObject);
            }

            shopItems.Clear();
        }

        public void SelectAll()
        {
            ClearSellSheet();
            foreach (SellShopItem item in shopItems)
            {
                item.SetInputField(item.GetStackSize());
                UpdateShoppingList(item, item.GetStackSize());
            }
        }

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
                UpdateShoppingListUI();
                return;
            }

            sellSheet.Add(new SellListItem(item, change));
            UpdateShoppingListUI();
        }

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

            totalSellMoney.text = $"${CalculateTotalMoney()}";
            totalSellAmount.text = CalculateTotalAmount().ToString();
        }

        private string GetSellListText(SellListItem itemToSell)
        {
            return
                $"{itemToSell.amount} x {itemToSell.name}, {itemToSell.size} : ${itemToSell.amount * itemToSell.singleCost} ";
        }

        private int CalculateTotalMoney()
        {
            int total = 0;
            foreach (SellListItem sell in sellSheet)
            {
                total += sell.amount * sell.singleCost;
            }

            return total;
        }

        private int CalculateTotalAmount()
        {
            int total = 0;
            foreach (SellListItem sell in sellSheet)
            {
                total += sell.amount;
            }

            return total;
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
            foreach (var text in sellTexts)
            {
                Destroy(text.gameObject);
            }

            sellSheet.Clear();
            sellTexts.Clear();
        }
    }
}