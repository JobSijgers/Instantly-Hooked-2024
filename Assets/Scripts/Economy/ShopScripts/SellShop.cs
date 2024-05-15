using Events;
using Player.Inventory;
using Enums;
using UnityEngine;

namespace Economy.ShopScripts
{
    public class SellShop : MonoBehaviour
    {
        private ShopState shopState = ShopState.Closed;

        private SellShopUI sellShopUI;
        
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
            if (Input.GetKeyDown(KeyCode.Escape) && shopState == ShopState.Open)
            {
                CloseShop();
            }
        }

        private void OpenShop()
        {
            shopState = ShopState.Open;
        }
        
        private void CloseShop()
        {
            EventManager.OnSellShopClose();
            shopState = ShopState.Closed;
        }
        
        private void SellSelected(SellListItem[] fishToSell)
        {
            foreach (SellListItem fish in fishToSell)
            {
                Inventory.Instance.RemoveFish(fish.data, fish.size, fish.amount);
                EventManager.OnShopSell(fish.amount * fish.data.fishSellAmount[(int)fish.size]);
            }
        }
    }
}