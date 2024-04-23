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

        private int currentLineLengthUpgrade;
        private int currentReelSpeedUpgrade;
        private int currentShipSpeedUpgrade;
        private int currentSonarUpgrade;

        private void Awake()
        {
            Instance = this;
        }

        #region Line Length Upgrades

        public void UpgradeLineLength()
        {
            currentLineLengthUpgrade++;
        }

        public LineLengthUpgrade GetCurrentLineLengthUpgrade()
        {
            return lineLengthUpgrades[currentLineLengthUpgrade];
        }

        public LineLengthUpgrade GetNextLineLengthUpgrade()
        {
            if (lineLengthUpgrades.Length < currentLineLengthUpgrade)
            {
                return lineLengthUpgrades[currentLineLengthUpgrade + 1];
            }

            return null;
        }

        #endregion

        #region Reel Speed Upgrades

        public void UpgradeReelSpeed()
        {
            currentReelSpeedUpgrade++;
        }

        public ReelSpeedUpgrade GetCurrentReelSpeedUpgrade()
        {
            return reelSpeedUpgrades[currentReelSpeedUpgrade];
        }

        public ReelSpeedUpgrade GetNextReelSpeedUpgrade()
        {
            if (reelSpeedUpgrades.Length < currentReelSpeedUpgrade)
            {
                return reelSpeedUpgrades[currentReelSpeedUpgrade + 1];
            }

            return null;
        }

        #endregion

        #region Ship Speed Upgrades

        public void UpgradeShipSpeed()
        {
            currentShipSpeedUpgrade++;
        }

        public ShipSpeedUpgrade GetCurrentShipSpeedUpgrade()
        {
            return shipSpeedUpgrades[currentShipSpeedUpgrade];
        }

        public ShipSpeedUpgrade GetNextShipSpeedUpgrade()
        {
            if (shipSpeedUpgrades.Length < currentShipSpeedUpgrade)
            {
                return shipSpeedUpgrades[currentShipSpeedUpgrade + 1];
            }

            return null;
        }

        #endregion

        #region Sonar Upgrades

        public void UpgradeSonar()
        {
            currentSonarUpgrade++;
        }

        public SonarUpgrade GetCurrentSonarUpgrade()
        {
            return sonarUpgrades[currentSonarUpgrade];
        }

        public SonarUpgrade GetNextSonarUpgrade()
        {
            if (sonarUpgrades.Length < currentSonarUpgrade)
            {
                return sonarUpgrades[currentSonarUpgrade + 1];
            }

            return null;
        }

        #endregion
    }
}