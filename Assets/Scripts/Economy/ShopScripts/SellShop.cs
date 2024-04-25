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
            
            StartCoroutine(LateStart());
        }

        private void OnDestroy()
        {
            EventManager.SellSelectedButton -= SellSelected;
        }

        private void OpenShop()
        {
            EventManager.OnSellShopOpen();
        }

        private void CloseShop()
        {
            EventManager.OnSellShopClose();
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