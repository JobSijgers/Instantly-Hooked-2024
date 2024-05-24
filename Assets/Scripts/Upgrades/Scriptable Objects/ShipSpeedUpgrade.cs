using UnityEngine;

namespace Upgrades.Scriptable_Objects
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

        public override string[] GetPrefix()
        {
            return new[]{
                "",
                ""
            };
        }

        public override string[] GetSuffix()
        {
            return new[]{
                "m/s",
                "m/s^2"
            };
        }
    }
}