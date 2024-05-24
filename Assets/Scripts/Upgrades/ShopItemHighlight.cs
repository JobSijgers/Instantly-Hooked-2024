using TMPro;
using UnityEngine;
using Upgrades.Scriptable_Objects;

namespace Upgrades
{
    public class UpgradeShopHighlight : MonoBehaviour
    {
        [SerializeField] private TMP_Text upgradeNameText;
        [SerializeField] private TMP_Text upgradeDescriptionText;
        [SerializeField] private TMP_Text upgradeCostText;
        [SerializeField] private TMP_Text upgradeEffectText;
        [SerializeField] private TMP_Text upgradeLevelText;

        public void HighlightUpgrade(Upgrade upgrade, int level)
        {
            if (upgrade == null)
            {
                upgradeNameText.text = "MAX";
                upgradeDescriptionText.text = "You have maxed out this upgrade";
                upgradeCostText.text = "";
                upgradeEffectText.text = "";
                return;
            }
            string upgradeEffectString = "";
            string[] upgradeEffectNames = upgrade.GetEffectName();
            string[] currentUpgradeEffect = UpgradeManager.Instance.GetCurrentUpgrade(upgrade).GetUpgradeEffect();
            string[] upgradeEffect = upgrade.GetUpgradeEffect();

            upgradeNameText.text = upgrade.upgradeName;
            upgradeDescriptionText.text = upgrade.description;
            upgradeCostText.text = upgrade.cost.ToString();


            for (int i = 0; i < upgrade.GetEffectName().Length; i++)
            {
                upgradeEffectString += $"{upgradeEffectNames[i]} {currentUpgradeEffect[i]} -> {upgradeEffect[i]} \n";
            }

            upgradeEffectText.text = upgradeEffectString;
        }

        public void ClearHighlight()
        {
            upgradeNameText.text = "";
            upgradeDescriptionText.text = "";
            upgradeCostText.text = "";
            upgradeEffectText.text = "";
        }
    }
}