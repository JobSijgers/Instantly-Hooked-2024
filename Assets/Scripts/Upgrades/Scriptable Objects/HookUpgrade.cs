using UnityEngine;

namespace Upgrades.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "HookUpgrade", menuName = "Upgrades/New Hook effectiveness Upgrade", order = 0)]
    public class HookUpgrade : Upgrade
    {
        public float BiteMultiply;
        public float StaminaDrain;
        public override string[] GetEffectName()
        {
            return new[]
            {
                "Bait Effectiveness",
                "Fish Stamina draining: "
            };
        }

        public override string[] GetUpgradeEffect()
        {
            return new[]
            {
                $"{BiteMultiply}",
                $"{StaminaDrain}"
            };
        }

        public override string[] GetPrefix()
        {
            return new[]
            {
                "+",
                "+"
            };
        }

        public override string[] GetSuffix()
        {
            return new[]
            {
                "%",
                "%"
            };
        }
    }
}
