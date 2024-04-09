using System;
using Economy;
using Unity.VisualScripting;
using UnityEngine;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager instance;

        public delegate void FMoneyUpdate(int newMoneyAmount);

        public event FMoneyUpdate OnMoneyUpdate;
        private int _currentMoney;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Shop.instance.OnSuccessfulSell += AddMoney;
        }

        private void OnDestroy()
        {
            Shop.instance.OnSuccessfulSell -= AddMoney;
        }

        public bool HasEnoughMoney(int purchaseAmount)
        {
            return _currentMoney - purchaseAmount >= 0;
        }

        private void AddMoney(int addAmount)
        {
            _currentMoney += addAmount;
            OnMoneyUpdate?.Invoke(_currentMoney);
        }
        
        public void RemoveMoney(int removeMoney)
        {
            _currentMoney -= removeMoney;
            OnMoneyUpdate?.Invoke(_currentMoney);
        }
    }
}