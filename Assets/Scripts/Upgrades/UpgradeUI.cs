using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Serialization;
using Upgrades.Scriptable_Objects;

namespace Upgrades
{
    public class UpgradeUI : MonoBehaviour
    {
        public static UpgradeUI instance;
        [SerializeField] private GameObject evenUpgradeShopItem;
        [SerializeField] private GameObject oddUpgradeShopItem;
        [SerializeField] private Transform upgradeShopItemParent;
        [SerializeField] private UpgradeShopHighlight fishHighlight;
        [SerializeField] private GameObject upgradeShopUI;
        private readonly List<UpgradeShopItem> upgradeShopItems = new();

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
            fishHighlight.ClearHighlight();
        }

        public void SelectUpgrade(Upgrade upgrade)
        {
            int level = UpgradeManager.Instance.GetUpgradeLevel(upgrade);
            fishHighlight.HighlightUpgrade(upgrade, level);
        }
    }
}