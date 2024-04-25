using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeShopItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text upgradeNameText;
        [SerializeField] private TMP_Text upgradeCostText;
        [SerializeField] private Button upgradeButton;
        private Upgrade _upgrade;
        
        private void Start()
        {
            upgradeButton.onClick.AddListener(UpgradeButtonPressed);
        }

        private void OnDestroy()
        {
            upgradeButton.onClick.RemoveListener(UpgradeButtonPressed);
        }

        public void UpgradeButtonPressed()
        {
            UpgradeUI.instance.SelectUpgrade(_upgrade);
        }
        
        public void SetUpgrade(Upgrade upgrades)
        {
            _upgrade = upgrades;
            upgradeNameText.text = _upgrade.upgradeName;
            upgradeCostText.text = _upgrade.cost.ToString();
        }
        public void SetMaxed()
        {
            _upgrade = null;
            upgradeNameText.text = "Maxed";
            upgradeCostText.text = "";
        }

        public Upgrade GetUpgrade()
        {
            return _upgrade;
        }
    }
}