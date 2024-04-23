using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "LineLengthUpgrade", menuName = "Upgrades/New Ship Speed Upgrade", order = 0)]
    public class ShipSpeedUpgrade : Upgrade
    {
        public float maxSpeed;
        public float acceleration;
    }
}