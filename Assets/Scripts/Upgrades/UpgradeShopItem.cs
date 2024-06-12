using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Upgrades.Scriptable_Objects;

namespace Upgrades
{
    public class UpgradeShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text upgradeCostText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_InputField inputField;
        private Upgrade upgrade;
        
        private void Start()
        {
            upgradeButton.onClick.AddListener(UpgradeButtonPressed);
        }

        private void OnDestroy()
        {
            upgradeButton.onClick.RemoveListener(UpgradeButtonPressed);
        }

        private void UpgradeButtonPressed()
        {
            if (upgrade == null)
                return;
            
            UpgradeManager.Instance.UpgradeBought(upgrade);
        }
        
        public void SetUpgrade(Upgrade upgrades)
        {
            upgrade = upgrades;
            upgradeCostText.text = $"{upgrade.cost.ToString()}" ;
        }
        public void SetMaxed()
        {
            upgrade = null;
            upgradeCostText.text = "MAX";
        }

        public Upgrade GetUpgrade()
        {
            return upgrade;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UpgradeUI.instance.SelectUpgrade(upgrade);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UpgradeUI.instance.ClearHighlight();
        }

        public void idk()
        {
            Debug.LogWarning("dont forget to remove this");
            int newInt = int.Parse(inputField.text);
            UpgradeManager.Instance.GetMatchingUpgradeState(upgrade).SetUpgradeLevel(newInt);
        }
    }
}