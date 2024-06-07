using System;
using UnityEngine;

namespace Upgrades.Scriptable_Objects
{
    public abstract class Upgrade : ScriptableObject
    {
        public string upgradeName;
        public string description;
        public int cost;
        public GameObject upgradeVisual;

        public abstract string[] GetEffectName();
        public abstract string[] GetUpgradeEffect();
        public abstract string[] GetPrefix();
        public abstract string[] GetSuffix();
    }
}