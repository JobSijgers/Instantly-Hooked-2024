using UnityEngine;

namespace Upgrades
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
    }
}