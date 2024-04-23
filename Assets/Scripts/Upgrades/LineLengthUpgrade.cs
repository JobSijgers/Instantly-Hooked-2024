using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "LineLengthUpgrade", menuName = "Upgrades/New Line Length Upgrade", order = 0)]
    public class LineLengthUpgrade : Upgrade
    {
        public float lineLengthMultiplier;
    }
}