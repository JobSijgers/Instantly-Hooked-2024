using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RarityColors", menuName = "Fish/Rarity Colors", order = 0)]
    public class FishRarityColor : ScriptableObject
    {
        public Color[] rarityColors;
        
        public Color GetRarityColor(FishRarity rarity)
        {
            return rarityColors[(int) rarity];
        }
    }
}