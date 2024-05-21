using System;
using Events;
using UnityEngine;
using Upgrades;

namespace Boat
{
    public class BoatUpgradeVisual : MonoBehaviour
    {
        [Serializable]
        private class UpgradeVisual
        {
            public Upgrade upgrade;
            public MeshFilter meshFilter;
            public MeshRenderer meshRenderer;
        }
        
        [SerializeField] private UpgradeVisual[] upgradeVisuals;

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
            foreach (UpgradeVisual upgradeVisual in upgradeVisuals)
            {
                if (upgradeVisual.upgrade != upgrade) continue;
                
                upgradeVisual.meshFilter.mesh = upgrade.upgradeVisual.mesh;
                upgradeVisual.meshRenderer.material = upgrade.upgradeVisual.material;
                return;
            }
        }
    }
}