using System;
using UnityEngine;
using Economy.ShopScripts;
using Events;
using Unity.VisualScripting;
using Upgrades.Scriptable_Objects;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager instance;

        private int currentMoney = 0;
        public int GetCurrentMoneyAmount() => currentMoney;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            EventManager.ShopSell += AddMoney;
        }

        private void OnDestroy()
        {
            EventManager.ShopSell -= AddMoney;
        }

        public bool HasEnoughMoney(int purchaseAmount)
        {
            return currentMoney - purchaseAmount >= 0;
        }

        private void AddMoney(int addAmount)
        {
            currentMoney += addAmount;
            EventManager.OnMoneyUpdate(currentMoney);
        }

        public void RemoveMoney(int removeMoney)
        {
            currentMoney -= removeMoney;
            EventManager.OnMoneyUpdate(currentMoney);
        }
    }
}