using System;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Economy.ShopScripts
{
    public class ShopUI : MonoBehaviour
    {
        public delegate void FSellSelected(FishData data);

        public delegate void FSellAll();

        public event FSellSelected OnSellSelectedButtonPressed;
        public event FSellAll OnSellAllButtonPressed;
        
        
        [SerializeField] private GameObject shopItemPrefab;
        [SerializeField] private GameObject shopObject;
        [SerializeField] private Transform itemHolder;
        [SerializeField] private Transform highlightHolder;
        [SerializeField] private GameObject highlightItem;
        private ShopItem _selectedItem;
        private Shop _shop;
        private List<ShopItem> _shopItems = new(); 
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
            foreach (var fishData in Inventory.instance.GetFishInInventory())
            {
                GameObject go = Instantiate(shopItemPrefab, itemHolder);
                ShopItem item =go.GetComponent<ShopItem>();
                item.Initialize(fishData);
                item.OnFishButtonPressed += SelectItem;
                _shopItems.Add(item);
            }
        }
        
        private void CloseShopUI()
        {
            shopObject.SetActive(false);
            foreach (var item in _shopItems)
            {
                item.OnFishButtonPressed -= SelectItem;
                Destroy(item.gameObject);
            }

            _shopItems.Clear();
        }
        
        private void SelectItem(ShopItem item)
        {
            ClearHighlight();
            GameObject go = Instantiate(highlightItem, highlightHolder);
            ShopItem highlight =go.GetComponent<ShopItem>();
            _selectedItem = item;
            highlight.Initialize(item.fishData);
            highlight.enabled = false;
        }

        private void ClearHighlight()
        {
            ShopItem itemToDestroy = highlightHolder.GetComponentInChildren<ShopItem>();
            if (itemToDestroy != null)
            {
                Destroy(itemToDestroy.gameObject);
                _selectedItem = null;
            }
        }
        public void SellSelectedItem()
        {
            
            OnSellSelectedButtonPressed?.Invoke(_selectedItem.fishData);
            _shopItems.Remove(_selectedItem);
            Destroy(_selectedItem.gameObject);
            ClearHighlight();
        }

        public void SellAllItems()
        {
            OnSellAllButtonPressed?.Invoke();
            foreach (var item in _shopItems)
            {
                Destroy(item.gameObject);
            }
            _shopItems.Clear();
            ClearHighlight();

        }
    }
}