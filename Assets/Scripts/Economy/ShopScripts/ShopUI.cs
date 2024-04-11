using System.Collections.Generic;
using Player.Inventory;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Economy.ShopScripts
{
    public class ShopUI : MonoBehaviour
    {
        public delegate void FSellSelected(SellListItem[] selectedItems);

        public event FSellSelected OnSellSelectedButtonPressed;


        [SerializeField] private GameObject shopItemPrefab;
        [SerializeField] private GameObject shopObject;
        [SerializeField] private Transform itemHolder;
        [SerializeField] private GameObject sellSheetTextPrefab;
        [SerializeField] private Transform sellSheetParent;
        [SerializeField] private TMP_Text totalSellMoney;
        [SerializeField] private TMP_Text totalSellAmount;

        private ShopItem _selectedItem;
        private Shop _shop;
        private List<ShopItem> _shopItems = new();
        private List<SellListItem> _sellSheet = new();
        private List<TMP_Text> _sellTexts = new();

        private void Start()
        {
            _shop = FindObjectOfType<Shop>();
            _shop.OnShopOpen += OpenShopUI;
            _shop.OnShopClose += CloseShopUI; 
        }
        

        private void OnDestroy()
        {
            _shop.OnShopOpen += OpenShopUI;
            _shop.OnShopClose += CloseShopUI;
        }

        private void OpenShopUI()
        {
            shopObject.SetActive(true);
            foreach (var inventoryItem in Inventory.instance.GetInventory())
            {
                GameObject go = Instantiate(shopItemPrefab, itemHolder);
                ShopItem item = go.GetComponent<ShopItem>();
                item.Initialize(inventoryItem.GetFishData(), inventoryItem.GetFishSize(), inventoryItem.GetStackSize());
                item.OnSelectedAmountChanged += UpdateShoppingList;
                _shopItems.Add(item);
            }
        }

        private void CloseShopUI()
        {
            shopObject.SetActive(false);
            foreach (var item in _shopItems)
            {
                Destroy(item.gameObject);
            }

            _shopItems.Clear();
        }

        public void SelectAll()
        {
            ClearSellSheet();
            foreach (var item in _shopItems)
            {
                item.SetInputField(item.GetStackSize());
                UpdateShoppingList(item, item.GetStackSize());
            }
        }
        private void UpdateShoppingList(ShopItem item, int change)
        {
            FishData data = item.GetFishData();
            for (int i = 0; i < _sellSheet.Count; i++)
            {
                var shoppingItem = _sellSheet[i];

                if (shoppingItem.name != data.fishName)
                    continue;
                if (shoppingItem.size != item.GetFishSize())
                    continue;
                if (shoppingItem.amount + change <= 0)
                {
                    _sellSheet.Remove(shoppingItem);
                    Destroy(_sellTexts[^1].gameObject);
                    _sellTexts.RemoveAt(_sellTexts.Count - 1);
                    UpdateShoppingListUI();
                    return;
                }

                shoppingItem.amount += change;
                _sellSheet[i] = shoppingItem;
                UpdateShoppingListUI();
                return;
            }

            _sellSheet.Add(new SellListItem(item, change));
            UpdateShoppingListUI();
        }

        private void UpdateShoppingListUI()
        {
            for (int i = 0; i < _sellSheet.Count; i++)
            {
                if (i >= _sellTexts.Count)
                {
                    GameObject go = Instantiate(sellSheetTextPrefab, sellSheetParent);
                    TMP_Text tmp = go.GetComponent<TMP_Text>();
                    tmp.text = GetSellListText(_sellSheet[i]);
                    _sellTexts.Add(tmp);
                }
                else
                {
                    _sellTexts[i].text = GetSellListText(_sellSheet[i]);
                }
            }

            totalSellMoney.text = $"${CalculateTotalMoney()}";
            totalSellAmount.text = CalculateTotalAmount().ToString();
        }

        private string GetSellListText(SellListItem itemToSell)
        {
            return $"{itemToSell.amount} x {itemToSell.name}, {itemToSell.size} : ${itemToSell.amount * itemToSell.singleCost} ";
        }

        private int CalculateTotalMoney()
        {
            int total = 0;
            foreach (var sell in _sellSheet)
            {
                total += sell.amount * sell.singleCost;
            }

            return total;
        }

        private int CalculateTotalAmount()
        {
            int total = 0;
            foreach (var sell in _sellSheet)
            {
                total += sell.amount;
            }

            return total;
        }

        public void SellSelectItems()
        {
            OnSellSelectedButtonPressed?.Invoke(_sellSheet.ToArray());
            ClearSellSheet();
            UpdateShoppingListUI();
            CloseShopUI();
            OpenShopUI();
        }
        private void ClearSellSheet()
        {
            foreach (var text in _sellTexts)
            {
                Destroy(text.gameObject);
            }
            _sellSheet.Clear();
            _sellTexts.Clear();
        }
    }
}