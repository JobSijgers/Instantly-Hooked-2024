using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Economy;
using Economy.ShopScripts;
using Events;
using UnityEngine;

namespace Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance { get; private set; }

        [SerializeField] private LineLengthUpgrade[] lineLengthUpgrades;
        [SerializeField] private ReelSpeedUpgrade[] reelSpeedUpgrades;
        [SerializeField] private ShipSpeedUpgrade[] shipSpeedUpgrades;
        [SerializeField] private SonarUpgrade[] sonarUpgrades;

        private int _lineLengthIndex;
        private int _reelSpeedIndex;
        private int _shipSpeedIndex;
        private int _sonarUpgradeIndex;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetUpItems();
            StartCoroutine(LateStart());
        }

        private void SetUpItems()
        {
            UpgradeUI.instance.CreateUpgradeItem(GetNextLineLengthUpgrade());
            UpgradeUI.instance.CreateUpgradeItem(GetNextReelSpeedUpgrade());
            UpgradeUI.instance.CreateUpgradeItem(GetNextShipSpeedUpgrade());
            UpgradeUI.instance.CreateUpgradeItem(GetNextSonarUpgrade());
        }

        public Upgrade GetCurrentUpgrade(Upgrade upgrade)
        {
            return upgrade switch
            {
                LineLengthUpgrade => lineLengthUpgrades[_lineLengthIndex],
                ReelSpeedUpgrade => reelSpeedUpgrades[_reelSpeedIndex],
                ShipSpeedUpgrade => shipSpeedUpgrades[_shipSpeedIndex],
                SonarUpgrade => sonarUpgrades[_sonarUpgradeIndex],
                _ => null
            };
        }

        public void UpgradeBought(Upgrade upgrade)
        {
            if (!EconomyManager.instance.HasEnoughMoney(upgrade.cost))
            {
                EventManager.OnNotEnoughMoney();
                Debug.Log("Not neough money");
                return;
            }

            switch (upgrade)
            {
                case LineLengthUpgrade:
                    UpgradeLineLength();
                    Debug.Log("Line Length Upgraded");
                    break;
                case ReelSpeedUpgrade:
                    UpgradeReelSpeed();
                    Debug.Log("Reel Speed Upgraded");
                    break;
                case ShipSpeedUpgrade:
                    UpgradeShipSpeed();
                    Debug.Log("Ship Speed Upgraded");
                    break;
                case SonarUpgrade:
                    UpgradeSonar();
                    Debug.Log("Sonar Upgraded");
                    break;
            }

            EventManager.OnUpgradeBought(GetCurrentUpgrade(upgrade));
            EconomyManager.instance.RemoveMoney(upgrade.cost);
        }

        public Upgrade GetNextUpgrade(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case LineLengthUpgrade:
                    return GetNextLineLengthUpgrade();
                case ReelSpeedUpgrade:
                    return GetNextReelSpeedUpgrade();
                case ShipSpeedUpgrade:
                    return GetNextShipSpeedUpgrade();
                case SonarUpgrade:
                    return GetNextSonarUpgrade();
                default:
                    return null;
            }
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();
            EventManager.OnUpgradeBought(GetCurrentUpgrade(GetCurrentLineLengthUpgrade()));
            EventManager.OnUpgradeBought(GetCurrentUpgrade(GetCurrentReelSpeedUpgrade()));
            EventManager.OnUpgradeBought(GetCurrentUpgrade(GetCurrentShipSpeedUpgrade()));
            EventManager.OnUpgradeBought(GetCurrentUpgrade(GetCurrentSonarUpgrade()));
        }

        #region Line Length Upgrades

        private void UpgradeLineLength()
        {
            _lineLengthIndex++;
        }

        private LineLengthUpgrade GetCurrentLineLengthUpgrade()
        {
            return lineLengthUpgrades[_lineLengthIndex];
        }

        private LineLengthUpgrade GetNextLineLengthUpgrade()
        {
            if (_lineLengthIndex + 1 < lineLengthUpgrades.Length)
            {
                return lineLengthUpgrades[_lineLengthIndex + 1];
            }

            return null;
        }

        #endregion

        #region Reel Speed Upgrades

        private void UpgradeReelSpeed()
        {
            _reelSpeedIndex++;
        }

        private ReelSpeedUpgrade GetCurrentReelSpeedUpgrade()
        {
            return reelSpeedUpgrades[_reelSpeedIndex];
        }

        private ReelSpeedUpgrade GetNextReelSpeedUpgrade()
        {
            if (_reelSpeedIndex + 1 < reelSpeedUpgrades.Length)
            {
                return reelSpeedUpgrades[_reelSpeedIndex + 1];
            }

            return null;
        }

        #endregion

        #region Ship Speed Upgrades

        private void UpgradeShipSpeed()
        {
            _shipSpeedIndex++;
        }

        private ShipSpeedUpgrade GetCurrentShipSpeedUpgrade()
        {
            return shipSpeedUpgrades[_shipSpeedIndex];
        }

        private ShipSpeedUpgrade GetNextShipSpeedUpgrade()
        {
            if (_shipSpeedIndex + 1 < shipSpeedUpgrades.Length)
            {
                return shipSpeedUpgrades[_shipSpeedIndex + 1];
            }

            return null;
        }

        #endregion

        #region Sonar Upgrades

        private void UpgradeSonar()
        {
            _sonarUpgradeIndex++;
        }

        private SonarUpgrade GetCurrentSonarUpgrade()
        {
            return sonarUpgrades[_sonarUpgradeIndex];
        }

        private SonarUpgrade GetNextSonarUpgrade()
        {
            if (_sonarUpgradeIndex + 1 < sonarUpgrades.Length)
            {
                return sonarUpgrades[_sonarUpgradeIndex + 1];
            }

            return null;
        }

        #endregion
    }
}