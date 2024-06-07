using System;
using Events;
using UnityEngine;
using Upgrades;
using Upgrades.Scriptable_Objects;

namespace Boat
{
    public class BoatUpgradeVisual : MonoBehaviour
    {
        [SerializeField] private Transform upgradeVisualParent;
        private void Start()
        {
            EventManager.UpgradeBought += UpdateVisuals;
        }

        private void OnDestroy()
        {
            EventManager.UpgradeBought -= UpdateVisuals;
        }
        
        private void UpdateVisuals(Upgrade upgrade)
        {
            if (upgrade.upgradeVisual == null)
                return;
            GameObject go = Instantiate(upgrade.upgradeVisual, upgradeVisualParent);
            go.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}