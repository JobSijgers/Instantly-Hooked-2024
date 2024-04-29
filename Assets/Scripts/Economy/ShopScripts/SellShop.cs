using System;
using System.Collections;
using Events;
using Player.Inventory;
using Timer;
using UnityEngine;

public enum ShopState
{
    Open,
    Closed
}

namespace Economy.ShopScripts
{
    public class SellShop : MonoBehaviour
    {
        private ShopState _shopState = ShopState.Closed;

        private SellShopUI _sellShopUI;
        
        private void Start()
        {
            EventManager.SellSelectedButton += SellSelected;
            EventManager.SellShopOpen += OpenShop;
        }

        private void OnDestroy()
        {
            EventManager.SellShopOpen -= OpenShop;
            EventManager.SellSelectedButton -= SellSelected;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _shopState == ShopState.Open)
            {
                CloseShop();
            }
        }

        private void OpenShop()
        {
            _shopState = ShopState.Open;
        }
        
        private void CloseShop()
        {
            EventManager.OnSellShopClose();
            _shopState = ShopState.Closed;
        }
        
        private void SellSelected(SellListItem[] fishToSell)
        {
            foreach (var fish in fishToSell)
            {
                Inventory.Instance.RemoveFish(fish.data, fish.size, fish.amount);
                EventManager.OnShopSell(fish.amount * fish.data.fishSellAmount[(int)fish.size]);
            }
        }
    }
}