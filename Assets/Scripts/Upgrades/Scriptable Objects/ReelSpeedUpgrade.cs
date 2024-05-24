﻿using UnityEngine;

namespace Upgrades.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "ReelSpeedUpgrade", menuName = "Upgrades/New ReelSpeed Upgrade", order = 0)]
    public class ReelSpeedUpgrade : Upgrade
    {
        public float reelSpeed;
        public float dropSpeed;
        
        public override string[] GetEffectName()
        {
            return new[]
            {
                "Reel Speed: ",
                "Drop Speed: "
            };
        }

        public override string[] GetUpgradeEffect()
        {
            return new[]
            {
                $"{reelSpeed}",
                $"{dropSpeed}"
            };
        }

        public override string[] GetPrefix()
        {
            return new[]
            {
                "",
                ""
            };
        }

        public override string[] GetSuffix()
        {
            return new[]
            {
                "",
                ""
            };
        }
    }
}