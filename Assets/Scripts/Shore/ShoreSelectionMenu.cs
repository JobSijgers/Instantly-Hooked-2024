using System;
using Events;
using UnityEditor;
using UnityEngine;
using PauseState = PauseMenu.PauseState;

namespace Shore
{
    public class ShoreSelectionMenu : MonoBehaviour
    {
        [SerializeField] private GameObject shoreSelectionMenu;
        private bool inShore = false;

        private void Start()
        {
            EventManager.ArrivedAtShore += ArrivedAtShore;
            EventManager.LeftShore += LeftShore;
            EventManager.SellShopClose += ShowShoreUI;
            EventManager.UpgradeShopClose += ShowShoreUI;
            EventManager.PauseStateChange += CheckUI;
            HideShoreUI();
        }

        private void CheckUI(PauseState newState)
        {
            if (!inShore)
                return;
            switch (newState)
            {
                case PauseState.Playing:
                    ShowShoreUI();
                    break;
                case PauseState.InPauseMenu:
                    break;
                case PauseState.InInventory:
                    HideShoreUI();
                    break;
                case PauseState.InCatalogue:
                    HideShoreUI();
                    break;
                case PauseState.InQuests:
                    HideShoreUI();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void OnDestroy()
        {
            EventManager.ArrivedAtShore -= ArrivedAtShore;
            EventManager.LeftShore -= LeftShore;
            EventManager.SellShopClose -= ShowShoreUI;
            EventManager.UpgradeShopClose -= ShowShoreUI;
            EventManager.PauseStateChange -= CheckUI;
        }

        private void ArrivedAtShore()
        {
            inShore = true;
            ShowShoreUI();
        }

        private void LeftShore()
        {
            inShore = false;
            HideShoreUI();
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
            HideShoreUI();
        }

        public void OpenUpgradeShop()
        {
            EventManager.OnUpgradeShopOpen();
            HideShoreUI();
        }
    }
}