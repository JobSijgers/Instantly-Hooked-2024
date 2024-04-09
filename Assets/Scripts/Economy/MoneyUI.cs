using System;
using TMPro;
using UnityEngine;

namespace Economy
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        private void Start()
        {
            EconomyManager.instance.OnMoneyUpdate += UpdateMoneyUI;
        }

        private void OnDestroy()
        {
            EconomyManager.instance.OnMoneyUpdate -= UpdateMoneyUI;
        }

        private void UpdateMoneyUI(int newMoney)
        {
            moneyText.text = $"Money: {newMoney}";
        }
    }
}