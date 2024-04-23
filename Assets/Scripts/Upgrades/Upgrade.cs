using UnityEngine;

namespace Upgrades
{
    public abstract class Upgrade : ScriptableObject
    {
        public string upgradeName;
        public string description;
        public int cost;
    }
}