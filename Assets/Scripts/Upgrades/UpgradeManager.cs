﻿using System;
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
        private class UpgradeState
        {
            [SerializeField] private Upgrade[] upgrades;
            private int upgradeIndex;

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
                return;
            }

            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            if (upgradeState == null)
                return;

            upgradeState.IncreaseUpgradeIndex();
            EventManager.OnUpgradeBought(upgrade);
        }

        public Upgrade GetNextUpgrade(Upgrade upgrade)
        {
            UpgradeState upgradeState = GetMatchingUpgradeState(upgrade);
            return upgradeState?.GetNextUpgrade();
        }

        /// <summary>
        /// This method returns the UpgradeState that matches the given upgrade.
        /// </summary>
        /// <param name="upgrade"></param>
        /// <returns></returns>
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

        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();
            NotifyUpgrades();
        }
    }
}