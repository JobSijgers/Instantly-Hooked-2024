using System.Collections;
using Player.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ShopState
{
    Open,
    Closed
}

namespace Economy.ShopScripts
{
    public class Shop : MonoBehaviour
    {
        public static Shop instance;

        public delegate void FSuccessfulSell(int sellAmount);

        public delegate void FOnShopOpen();

        public delegate void FOnShopClose();


        public event FSuccessfulSell OnSuccessfulSell;

        public event FOnShopOpen OnShopOpen;

        public event FOnShopClose OnShopClose;

        private ShopState _shopState = ShopState.Closed;

        private ShopUI _shopUI;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _shopUI = FindObjectOfType<ShopUI>();
            _shopUI.OnSellSelectedButtonPressed += SellSelected;
            StartCoroutine(LateStart());
        }

        public void OpenShop()
        {
            OnShopOpen?.Invoke();
        }

        public void CloseShop()
        {
            OnShopClose?.Invoke();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L) && _shopState != ShopState.Open)
            {
                OpenShop();
                _shopState = ShopState.Open;
            }

            if (Input.GetKeyDown(KeyCode.K) && _shopState != ShopState.Closed)
            {
                CloseShop();
                _shopState = ShopState.Closed;
            }
        }

        private void SellSelected(SellListItem[] fishToSell)
        {
            foreach (var fish in fishToSell)
            {
                Inventory.instance.RemoveFish(fish.data, fish.size, fish.amount);
                OnSuccessfulSell?.Invoke(fish.amount * fish.data.fishSellAmount[(int)fish.size]);
            }
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();
            OpenShop();
            CloseShop();
        }
    }
}