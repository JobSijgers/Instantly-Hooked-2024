using UnityEngine;

namespace Upgrades
{
    public abstract class Upgrade : ScriptableObject
    {
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