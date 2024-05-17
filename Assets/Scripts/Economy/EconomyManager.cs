using System;
using UnityEngine;
using Economy.ShopScripts;
using Events;
using Unity.VisualScripting;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager instance;
        private int _currentMoney = 0;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            EventManager.ShopSell += AddMoney;
            AddMoney(1000000);
        }

        private void OnDestroy()
        {
            EventManager.ShopSell -= AddMoney;
        }

        public bool HasEnoughMoney(int purchaseAmount)
        {
            return _currentMoney - purchaseAmount >= 0;
        }

        public void AddMoney(int addAmount)
        {
            _currentMoney += addAmount;
            EventManager.OnMoneyUpdate(_currentMoney);
        }
        
        public void RemoveMoney(int removeMoney)
        {
            _currentMoney -= removeMoney;
            EventManager.OnMoneyUpdate(_currentMoney);
        }
    }
}