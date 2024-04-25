using Events;
using UnityEngine;

namespace Shore
{
    public class ShoreSelectionMenu : MonoBehaviour
    {
        [SerializeField] private GameObject shoreSelectionMenu;
        private void Start()
        {
            EventManager.ArrivedAtShore += ShowShoreUI;
            EventManager.LeftShore += HideShoreUI;
            EventManager.SellShopClose += ShowShoreUI;
            EventManager.UpgradeShopClose += ShowShoreUI;
            HideShoreUI();
        }

        private void OnDestroy()
        {
            EventManager.ArrivedAtShore -= ShowShoreUI;
            EventManager.LeftShore -= HideShoreUI;
            EventManager.SellShopClose -= ShowShoreUI;
            EventManager.UpgradeShopClose -= ShowShoreUI;
        }
        
        private void ShowShoreUI()
        {
            shoreSelectionMenu.SetActive(true);
        }
        
        private void HideShoreUI()
        {
            shoreSelectionMenu.SetActive(false);
        }

        public void OpenSellShop()
        {
            EventManager.OnSellShopOpen();
        }
        
        public void OpenUpgradeShop()
        {
            EventManager.OnUpgradeShopOpen();
        }
    }
}