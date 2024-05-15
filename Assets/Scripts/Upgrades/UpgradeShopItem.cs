﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text upgradeNameText;
        [SerializeField] private TMP_Text upgradeCostText;
        [SerializeField] private Button upgradeButton;
        private Upgrade upgrade;
        
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
            UpgradeManager.Instance.UpgradeBought(upgrade);
        }
        
        public void SetUpgrade(Upgrade upgrades)
        {
            upgrade = upgrades;
            upgradeNameText.text = upgrade.upgradeName;
            upgradeCostText.text = upgrade.cost.ToString();
        }
        public void SetMaxed()
        {
            upgrade = null;
            upgradeNameText.text = "Maxed";
            upgradeCostText.text = "";
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
    }
}