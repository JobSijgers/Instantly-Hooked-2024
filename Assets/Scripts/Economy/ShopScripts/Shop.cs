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
    public class Shop : MonoBehaviour
    {
        private ShopState _shopState = ShopState.Closed;

        private ShopUI _shopUI;
        
        private void Start()
        {
            EventManager.Dock += OpenShop;
            EventManager.UnDock += CloseShop;
            EventManager.SellSelectedButton += SellSelected;
            
            StartCoroutine(LateStart());
        }

        private void OnDestroy()
        {
            EventManager.Dock -= OpenShop;
            EventManager.UnDock -= CloseShop;
            EventManager.SellSelectedButton -= SellSelected;
        }

        private void OpenShop()
        {
            EventManager.OnShopOpen();
        }

        private void CloseShop()
        {
            EventManager.OnShopClose();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L) && _shopState != ShopState.Open)
            {
                OpenShop();
                _shopState = ShopState.Open;
            }

            if (Input.GetKeyDown(KeyCode.K) && _shopState != ShopState.Closed)
            {
                CloseShop();
                _shopState = ShopState.Closed;
            }
        }

        private void SellSelected(SellListItem[] fishToSell)
        {
            foreach (var fish in fishToSell)
            {
                Inventory.Instance.RemoveFish(fish.data, fish.size, fish.amount);
                EventManager.OnShopSell(fish.amount * fish.data.fishSellAmount[(int)fish.size]);
            }
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();
            OpenShop();
            CloseShop();
        }
    }
}