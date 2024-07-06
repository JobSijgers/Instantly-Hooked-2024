using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RaritySprites", menuName = "Fish/Rarity Sprites", order = 0)]
    public class FishRaritySprites : ScriptableObject
    {
        public Sprite[] raritySprites;
        
        public Sprite GetRaritySprite(FishRarity rarity)
        {
            return raritySprites[(int) rarity];
        }
    }
}