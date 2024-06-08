using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private Image progressBar;

        public void HighlightUpgrade(Upgrade upgrade, int level)
        {
            if (upgrade == null)
            {
                upgradeNameText.text = "MAX";
                upgradeDescriptionText.text = "You have maxed out this upgrade";
                upgradeCostText.text = "";
                upgradeEffectText.text = "";
                upgradeLevelText.text = "";
                progressBar.fillAmount = 1;
                return;
            }

            string upgradeEffectString = "";
            string[] upgradeEffectNames = upgrade.GetEffectName();
            string[] currentUpgradeEffect = UpgradeManager.Instance.GetCurrentUpgrade(upgrade).GetUpgradeEffect();
            string[] upgradeEffect = upgrade.GetUpgradeEffect();
            string[] suffix = upgrade.GetSuffix();
            string[] prefix = upgrade.GetPrefix();

            upgradeNameText.text = upgrade.upgradeName;
            upgradeDescriptionText.text = upgrade.description;
            upgradeCostText.text = $"Cost: ${upgrade.cost.ToString()}";
            int levelIndex = level + 1;
            upgradeLevelText.text = levelIndex.ToString();


            for (int i = 0; i < upgrade.GetEffectName().Length; i++)
            {
                upgradeEffectString += $"{upgradeEffectNames[i]} {prefix[i]}{currentUpgradeEffect[i]}{suffix[i]} -> {prefix[i]}{upgradeEffect[i]}{suffix[i]} \n";
            }

            upgradeEffectText.text = upgradeEffectString;
            progressBar.fillAmount = (float) (levelIndex - 1) / (UpgradeManager.Instance.GetMaxLevel(upgrade) - 1);
        }

        public void ClearHighlight()
        {
            upgradeNameText.text = "";
            upgradeDescriptionText.text = "";
            upgradeCostText.text = "";
            upgradeEffectText.text = "";
            upgradeLevelText.text = "";
            progressBar.fillAmount = 0;
        }
    }
}