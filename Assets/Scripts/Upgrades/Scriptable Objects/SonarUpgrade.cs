using UnityEngine;

namespace Upgrades.Scriptable_Objects
{

    [CreateAssetMenu(fileName = "SonarUpgrade", menuName = "Upgrades/New Sonar Upgrade", order = 0)]
    public class SonarUpgrade : Upgrade
    {
        public float sonarDistance;
        public override string[] GetEffectName()
        {
            return new[]
            {
                "Sonar Distance: "
            };
        }

        public override string[] GetUpgradeEffect()
        {
            return new[]
            {
                $"{sonarDistance}"
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
                "m"
            };
        }
    }
}