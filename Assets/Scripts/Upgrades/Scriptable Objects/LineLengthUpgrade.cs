using UnityEngine;

namespace Upgrades.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "LineLengthUpgrade", menuName = "Upgrades/New Line Length Upgrade", order = 0)]
    public class LineLengthUpgrade : Upgrade
    {
        public float lineLength;
        public override string[] GetEffectName()
        {
            return new[]
            {
                "Line Length: "
            };
        }

        public override string[] GetUpgradeEffect()
        {
            return new[]
            {
                $"{lineLength}"
            };
        }
        
    }
}