using System;
using System.Collections;
using Economy;
using Enums;
using Events;
using UnityEngine;
using Upgrades.Scriptable_Objects;

namespace Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        // This class represents the state of an upgrade, including its current level and the array of possible upgrades.
        [Serializable]
        public class UpgradeState
        {
            [SerializeField] private Upgrade[] upgrades;
            private int upgradeIndex;
            public int GetUpgradeIndex() => upgradeIndex;
            public void Setindex(int index) { upgradeIndex = index; }

            public void IncreaseUpgradeIndex()
            {
                upgradeIndex++;
            }

            public Upgrade GetCurrentUpgrade()
            {
                return upgrades[upgradeIndex];
            }

            public Upgrade GetNextUpgrade()
            {
                if (upgradeIndex + 1 < upgrades.Length)
                {
                    return upgrades[upgradeIndex + 1];
                }

                return null;
            }

            /// <summary>
            /// This method checks if the given upgrade is of the same type as the upgrades in this state.
            /// </summary>
            /// <param name="upgrade"></param>
            /// <returns></returns>
            public bool IsSameType(Upgrade upgrade)
            {
                if (upgrades.Length <= 0 || upgrade == null)
                    return false;

                return upgrade.GetType() == upgrades[0].GetType();
            }
            
            public int GetUpgradeLevel()
            {
                return upgradeIndex;
            }
            
            public int GetMaxLevel()
            {
                return upgrades.Length;
            }
            public void SetUpgradeLevel(int index)
            {
                upgradeIndex = index;
                Instance.NotifyUpgrades();
            }
        }

        public static UpgradeManager Instance { get; private set; }

        [SerializeField] private UpgradeState[] upgradeStates;
        private ShopState shopState = ShopState.Closed;

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
            if (Input.GetKeyDown(KeyCode.Escape) && shopState == ShopState.Open)
            {
                CloseShop();
            }
        }

        /// <summary>
        /// This method sets up the upgrade items in the shop UI.
        /// </summary>
        private void SetUpItems()
        {
            for (int i = 0; i < upgradeStates.Length; i++)
            {
                UpgradeUI.instance.CreateUpgradeItem(upgradeStates[i].GetNextUpgrade(), i + 1);
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
                return;
            }
            
            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            if (upgradeState == null)
                return;
            EconomyManager.instance.RemoveMoney(upgrade.cost);
            upgradeState.IncreaseUpgradeIndex();
            EventManager.OnUpgradeBought(upgrade);
        }

        public Upgrade GetNextUpgrade(Upgrade upgrade)
        {
            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            return upgradeState?.GetNextUpgrade();
        }
        
        public int GetUpgradeLevel(Upgrade upgrade)
        {
            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            return upgradeState?.GetUpgradeLevel() ?? 0;
        }
        
        public int GetMaxLevel(Upgrade upgrade)
        {
            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            return upgradeState?.GetMaxLevel() ?? 0;
        }

        /// <summary>
        /// This method returns the UpgradeState that matches the given upgrade.
        /// </summary>
        /// <param name="upgrade"></param>
        /// <returns></returns>
        public UpgradeState GetMatchingUpgradeState(Upgrade upgrade)
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
        /// <summary>
        /// This method notifies all upgrades that the player has left the shore.
        /// </summary>
        private void NotifyUpgrades()
        {
            foreach (UpgradeState upgradeState in upgradeStates)
            {
                EventManager.OnUpgradeBought(upgradeState.GetCurrentUpgrade());
            }
        }

        private void OpenShop()
        {
            shopState = ShopState.Open;
        }

        private void CloseShop()
        {
            EventManager.OnUpgradeShopClose();
            shopState = ShopState.Closed;
        }

        public int[] GetUpgrades()
        {
            int[] updateindexes = new int[upgradeStates.Length];
            for (int i = 0; i < upgradeStates.Length -1; i++)
            {
                updateindexes[i] = upgradeStates[i].GetUpgradeIndex();
            }
            return updateindexes;
        }
        public void SetUpgrades(int[] upgradeindex)
        {
            for (int i = 0;i < upgradeStates.Length; i++)
            {
                upgradeStates[i].Setindex(upgradeindex[i]);
            }
        }
        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();
            NotifyUpgrades();
        }
    }
}