using System.Collections.Generic;
using Events;
using UnityEngine;
using Upgrades.Scriptable_Objects;
using Views;

namespace Upgrades
{
    public class UpgradeUI : View
    {
        public static UpgradeUI instance;
        [SerializeField] private GameObject evenUpgradeShopItem;
        [SerializeField] private GameObject oddUpgradeShopItem;
        [SerializeField] private Transform upgradeShopItemParent;
        [SerializeField] private UpgradeShopHighlight upgradeHighlight;
        private readonly List<UpgradeShopItem> upgradeShopItems = new();

        public override void Initialize()
        {
            base.Initialize();
            instance = this;
        }

        private void Start()
        {
            EventManager.UpgradeBought += ChangeItemUpgrade;
        }

        private void OnDestroy()
        {
            EventManager.UpgradeBought -= ChangeItemUpgrade;
        }
        
        
        /// <summary>
        /// his method creates a new upgrade item in the shop for each upgrade.
        /// </summary>
        /// <param name="upgrade">Upgrade to create item for</param>
        /// <param name="id"></param>
        public void CreateUpgradeItem(Upgrade upgrade, int id)
        {
            GameObject upgradeItem = Instantiate(id % 2 == 0 ? evenUpgradeShopItem : oddUpgradeShopItem,
                upgradeShopItemParent);

            UpgradeShopItem shopItem = upgradeItem.GetComponent<UpgradeShopItem>();
            shopItem.SetUpgrade(upgrade);
            upgradeShopItems.Add(shopItem);
        }

        /// <summary>
        /// This method updates the displayed upgrade items in the shop when an upgrade is bought.
        /// </summary>
        /// <param name="upgrade">New  Upgrade</param>
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
            upgradeHighlight.ClearHighlight();
        }

        public void SelectUpgrade(Upgrade upgrade)
        {
            int level = UpgradeManager.Instance.GetUpgradeLevel(upgrade);
            upgradeHighlight.HighlightUpgrade(upgrade, level);
        }
    }
}