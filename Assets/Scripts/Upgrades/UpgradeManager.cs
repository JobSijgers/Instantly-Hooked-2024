using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Economy;
using Economy.ShopScripts;
using Enums;
using Events;
using UnityEngine;

namespace Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        [Serializable]
        private class UpgradeState
        {
            [SerializeField] private Upgrade[] upgrades;
            private int _upgradeIndex;

            public void IncreaseUpgradeIndex()
            {
                _upgradeIndex++;
            }

            public Upgrade GetCurrentUpgrade()
            {
                return upgrades[_upgradeIndex];
            }

            public Upgrade GetNextUpgrade()
            {
                if (_upgradeIndex + 1 < upgrades.Length)
                {
                    return upgrades[_upgradeIndex + 1];
                }

                return null;
            }

            public bool IsSameType(Upgrade upgrade)
            {
                if (upgrades.Length <= 0 || upgrade == null)
                    return false;

                return upgrade.GetType() == upgrades[0].GetType();
            }
        }

        public static UpgradeManager Instance { get; private set; }

        [SerializeField] private UpgradeState[] upgradeStates;
        private ShopState _shopState = ShopState.Closed;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            EventManager.LeftShore += NotifyUpgrades;
            EventManager.UpgradeShopOpen += OpenShop;
            SetUpItems();
            StartCoroutine(LateStart());
        }
        private void OnDestroy()
        {
            EventManager.LeftShore -= NotifyUpgrades;
            EventManager.UpgradeShopOpen -= OpenShop;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _shopState == ShopState.Open)
            {
                CloseShop();
            }
        }

        private void SetUpItems()
        {
            foreach (UpgradeState upgradeState in upgradeStates)
            {
                UpgradeUI.instance.CreateUpgradeItem(upgradeState.GetNextUpgrade());
            }
        }

        public Upgrade GetCurrentUpgrade(Upgrade upgrade)
        {
            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            return upgradeState?.GetCurrentUpgrade();
        }

        public void UpgradeBought(Upgrade upgrade)
        {
            if (!EconomyManager.instance.HasEnoughMoney(upgrade.cost))
            {
                EventManager.OnNotEnoughMoney();
                Debug.Log("Not neough money");
                return;
            }

            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            if (upgradeState == null)
                return;

            upgradeState.IncreaseUpgradeIndex();
            EconomyManager.instance.RemoveMoney(upgrade.cost);
            EventManager.OnUpgradeBought(upgrade);
        }

        public Upgrade GetNextUpgrade(Upgrade upgrade)
        {
            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            return upgradeState?.GetNextUpgrade();
        }
        private UpgradeState GetMatchingUpgradeState(Upgrade upgrade)
        {
            foreach (UpgradeState upgradeState in upgradeStates)
            {
                if (upgradeState.IsSameType(upgrade))
                {
                    return upgradeState;
                }
            }
            return null;
        }

        private void NotifyUpgrades()
        {
            foreach (UpgradeState upgradeState in upgradeStates)
            {
                EventManager.OnUpgradeBought(upgradeState.GetCurrentUpgrade());
            }
        }
        
        private void OpenShop()
        {
            _shopState = ShopState.Open;
        }
        
        private void CloseShop()
        {
            EventManager.OnUpgradeShopClose();
            _shopState = ShopState.Closed;
            
        }
        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();
            NotifyUpgrades();
        }
    }
}