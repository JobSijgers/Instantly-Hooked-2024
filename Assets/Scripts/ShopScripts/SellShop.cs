using Audio;
using Events;
using Player.Inventory;
using UnityEngine;

namespace ShopScripts
{
    public class SellShop : MonoBehaviour
    {
        private SellShopUI sellShopUI;
        
        private void Start()
        {
            EventManager.SellSelectedButton += SellSelected;
        }

        private void OnDestroy()
        {
            EventManager.SellSelectedButton -= SellSelected;
        }

        private void SellSelected(SellListItem[] fishToSell)
        {
            if (fishToSell.Length == 0)
                return;
            foreach (SellListItem fish in fishToSell)
            {
                Inventory.instance.RemoveFish(fish.data, fish.size, fish.amount);
                EventManager.OnShopSell(fish.amount * fish.data.fishSellAmount[(int)fish.size]);
            }
            
            AudioManager.instance.PlaySound("SellFish");
        }
    }
}