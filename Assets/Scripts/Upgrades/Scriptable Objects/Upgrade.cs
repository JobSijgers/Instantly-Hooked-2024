using System;
using UnityEngine;

namespace Upgrades.Scriptable_Objects
{
    public abstract class Upgrade : ScriptableObject
    {
        [Serializable]
        public class UpgradeVisual
        {
            public Mesh mesh;
            public Material material;
        }
        public string upgradeName;
        public string description;
        public int cost;
        public UpgradeVisual upgradeVisual;

        public abstract string[] GetEffectName();
        public abstract string[] GetUpgradeEffect();
    }
}