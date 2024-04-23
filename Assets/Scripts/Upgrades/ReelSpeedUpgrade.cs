using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "LineLengthUpgrade", menuName = "Upgrades/New ReelSpeed Upgrade", order = 0)]
    public class ReelSpeedUpgrade : Upgrade
    {
        public float reelSpeed;
        public float dropSpeed;
    }
}