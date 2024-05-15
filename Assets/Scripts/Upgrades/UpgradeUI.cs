using System;
using System.Collections.Generic;
using Economy;
using Events;
using UnityEngine;

namespace Upgrades
{
    public class UpgradeUI : MonoBehaviour
    {
        public static UpgradeUI instance;
        [SerializeField] private GameObject upgradeShopItem;
        [SerializeField] private Transform upgradeShopItemParent;
        [SerializeField] private UpgradeShopHighlight upgradeShopHighlight;
        [SerializeField] private GameObject upgradeShopUI;
        private List<UpgradeShopItem> upgradeShopItems = new List<UpgradeShopItem>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            EventManager.UpgradeBought += ChangeItemUpgrade;
            EventManager.UpgradeShopOpen += OpenUpgradeShopUI;
            EventManager.UpgradeShopClose += CloseUpgradeShopUI;
            EventManager.LeftShore += CloseUpgradeShopUI;
        }

        private void OnDestroy()
        {
            EventManager.UpgradeBought -= ChangeItemUpgrade;
            EventManager.UpgradeShopOpen -= OpenUpgradeShopUI;
            EventManager.UpgradeShopClose -= CloseUpgradeShopUI;
            EventManager.LeftShore -= CloseUpgradeShopUI;
        }
        
        private void OpenUpgradeShopUI() 
        {
            upgradeShopUI.SetActive(true);
        }
        
        public void CloseUpgradeShopUI()
        {
            upgradeShopUI.SetActive(false);
        }
        
        public void CreateUpgradeItem(Upgrade upgrade)
        {
            GameObject upgradeItem = Instantiate(upgradeShopItem, upgradeShopItemParent);
            UpgradeShopItem shopItem = upgradeItem.GetComponent<UpgradeShopItem>();
            shopItem.SetUpgrade(upgrade);
            upgradeShopItems.Add(shopItem);
        }

        private void ChangeItemUpgrade(Upgrade upgrade)
        {
            foreach (UpgradeShopItem shopItem in upgradeShopItems)
            {
                if (shopItem.GetUpgrade() == null) continue;
                if (shopItem.GetUpgrade().GetType() != upgrade.GetType()) continue;
                Upgrade nextUpgrade = UpgradeManager.Instance.GetNextUpgrade(upgrade);
                
                if (nextUpgrade == null)
                {
                    shopItem.SetMaxed();
                    SelectUpgrade(null);
                    return;
                }
                shopItem.SetUpgrade(nextUpgrade);
                SelectUpgrade(nextUpgrade);
            }
        }

        public void ClearHighlight()
        {
            upgradeShopHighlight.ClearHighlight();
        }
        public void SelectUpgrade(Upgrade upgrade)
        {
            upgradeShopHighlight.HighlightUpgrade(upgrade);
        }
    }
}