using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeShopHighlight : MonoBehaviour
    {
        [SerializeField] private TMP_Text upgradeNameText;
        [SerializeField] private TMP_Text upgradeDescriptionText;
        [SerializeField] private TMP_Text upgradeCostText;
        [SerializeField] private TMP_Text upgradeEffectText;
        [SerializeField] private Button upgradeButton;
        private Upgrade _currentHighLight;

        private void Start()
        {
            upgradeButton.onClick.AddListener(BuyButtonPressed);
        }

        public void HighlightUpgrade(Upgrade upgrade)
        {
            if (upgrade == null)
            {
                upgradeNameText.text = "MAX";
                upgradeDescriptionText.text = "You have maxed out this upgrade";
                upgradeCostText.text = "";
                upgradeEffectText.text = "";
                upgradeButton.interactable = false;
                return;
            }
            string upgradeEffectString = "";
            string[] upgradeEffectNames = upgrade.GetEffectName();
            string[] currentUpgradeEffect = UpgradeManager.Instance.GetCurrentUpgrade(upgrade).GetUpgradeEffect();
            string[] upgradeEffect = upgrade.GetUpgradeEffect();

            upgradeNameText.text = upgrade.upgradeName;
            upgradeDescriptionText.text = upgrade.description;
            upgradeCostText.text = upgrade.cost.ToString();
            upgradeButton.interactable = true;


            for (int i = 0; i < upgrade.GetEffectName().Length; i++)
            {
                upgradeEffectString += $"{upgradeEffectNames[i]} {currentUpgradeEffect[i]} -> {upgradeEffect[i]} \n";
            }

            upgradeEffectText.text = upgradeEffectString;

            _currentHighLight = upgrade;
        }

        private void BuyButtonPressed()
        {
            if (_currentHighLight == null)
                return;
            UpgradeManager.Instance.UpgradeBought(_currentHighLight);
        }
    }
}