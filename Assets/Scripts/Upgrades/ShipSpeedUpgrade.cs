﻿using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "ShipSpeedUpgrade", menuName = "Upgrades/New Ship Speed Upgrade", order = 0)]
    public class ShipSpeedUpgrade : Upgrade
    {
        public float maxSpeed;
        public float acceleration;
        public override string[] GetEffectName()
        {
            return new[]
            {
                "Max Speed: ",
                "Acceleration: "
            };
        }

        public override string[] GetUpgradeEffect()
        {
            return new[]
            {
                $"{maxSpeed}",
                $"{acceleration}"
            };
        }
    }
}