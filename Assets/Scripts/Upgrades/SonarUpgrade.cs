using UnityEngine;

namespace Upgrades
{
    public enum SonarLevel
    {
        
    }
    [CreateAssetMenu(fileName = "LineLengthUpgrade", menuName = "Upgrades/New ReelSpeed Upgrade", order = 0)]
    public class SonarUpgrade : Upgrade
    {
        public float sonarDistance;
        
    }
}