using UnityEngine;
using Economy.ShopScripts;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance;

        public delegate void FMoneyUpdate(int newMoneyAmount);

        public event FMoneyUpdate OnMoneyUpdate;
        private int _currentMoney;

        private void Awake()
        {
            Instance = this;
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