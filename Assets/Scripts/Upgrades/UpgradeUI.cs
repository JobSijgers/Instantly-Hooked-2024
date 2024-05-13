using System;
using System.Collections.Generic;
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
        private List<UpgradeShopItem> _upgradeShopItems = new List<UpgradeShopItem>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            EventManager.UpgradeBought += ChangeItemUpgrade;
            EventManager.UpgradeShopOpen += OpenUpgradeShopUI;
            EventManager.UpgradeShopClose += CloseUpgradeShopUI;
        }

        private void OnDestroy()
        {
            EventManager.UpgradeBought -= ChangeItemUpgrade;
            EventManager.UpgradeShopOpen -= OpenUpgradeShopUI;
            EventManager.UpgradeShopClose -= CloseUpgradeShopUI;
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
            var upgradeItem = Instantiate(upgradeShopItem, upgradeShopItemParent);
            var shopItem = upgradeItem.GetComponent<UpgradeShopItem>();
            shopItem.SetUpgrade(upgrade);
            _upgradeShopItems.Add(shopItem);
        }

        private void ChangeItemUpgrade(Upgrade upgrade)
        {
            foreach (var shopItem in _upgradeShopItems)
            {
                if (shopItem.GetUpgrade() == null) continue;
                if (shopItem.GetUpgrade().GetType() != upgrade.GetType()) continue;
                Upgrade nextUpgrade = UpgradeManager.Instance.GetNextUpgrade(upgrade);
                
                if (nextUpgrade == null)
                {
                    shopItem.SetMaxed();
                    shopItem.UpgradeButtonPressed();
                    return;
                }
                shopItem.SetUpgrade(nextUpgrade);
                shopItem.UpgradeButtonPressed();
            }
        }

        public void SelectUpgrade(Upgrade upgrade)
        {
            upgradeShopHighlight.HighlightUpgrade(upgrade);
        }
    }
}