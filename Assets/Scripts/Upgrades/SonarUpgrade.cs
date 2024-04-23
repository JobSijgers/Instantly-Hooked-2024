using UnityEngine;

namespace Upgrades
{

    [CreateAssetMenu(fileName = "LineLengthUpgrade", menuName = "Upgrades/New Sonar Upgrade", order = 0)]
    public class SonarUpgrade : Upgrade
    {
        public float sonarDistance;
    }
}