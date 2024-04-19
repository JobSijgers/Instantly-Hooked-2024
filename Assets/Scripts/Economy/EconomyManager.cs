using UnityEngine;
using Economy.ShopScripts;
using Events;
using Unity.VisualScripting;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        private int _currentMoney;
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
            return _currentMoney - purchaseAmount >= 0;
        }

        private void AddMoney(int addAmount)
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