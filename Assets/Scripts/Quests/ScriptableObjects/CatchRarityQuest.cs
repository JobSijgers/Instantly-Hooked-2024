using Enums;
using Fish;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Create CatchRarityQuest", fileName = "CatchRarityQuest", order = 0)]
    public class CatchRarityQuest : Quest
    {
        public FishRarity rarity;
        
        public override string QuestType => "CatchSize";
        public override bool IsQuestConditionMet(FishData fishData, FishSize fishSize)
        {
            return rarity == fishData.fishRarity;
        }
    }
}