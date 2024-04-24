using UnityEngine;

namespace Upgrades
{
    public abstract class Upgrade : ScriptableObject
    {
        public string upgradeName;
        public string description;
        public int cost;

        public abstract string[] GetEffectName();
        public abstract string[] GetUpgradeEffect();
    }
}