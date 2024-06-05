using UnityEngine;

namespace Upgrades.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "HookUpgrade", menuName = "Upgrades/New Hook effectiveness Upgrade", order = 0)]
    public class HookUpgrade : Upgrade
    {
        public int BiteMultiply;
        public override string[] GetEffectName()
        {
            return new[]
            {
                "Hook Effectiveness: "
            };
        }

        public override string[] GetUpgradeEffect()
        {
            return new[]
            {
                $"{BiteMultiply}"
            };
        }

        public override string[] GetPrefix()
        {
            return new[]
            {
                ""
            };
        }

        public override string[] GetSuffix()
        {
            return new[]
            {
                "/ 10 %"
            };
        }
    }
}
