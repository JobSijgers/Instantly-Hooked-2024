using System;
using Events;
using TMPro;
using UnityEngine;

namespace Economy
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;

        private void Start()
        {
            EventManager.MoneyUpdate += UpdateMoneyUI;
        }

        private void OnDestroy()
        {
            EventManager.MoneyUpdate -= UpdateMoneyUI;
        }

        private void UpdateMoneyUI(int newMoney)
        {
            moneyText.text = $"Money: {newMoney}";
        }
    }
}