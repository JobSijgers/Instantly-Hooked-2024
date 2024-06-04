using Enums;
using Fish;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Create CatchFishQuest", fileName = "CatchFishQuest", order = 0)]
    public class CatchFishQuest : Quest
    {
        public FishData fish;
        public override string QuestType => "CatchSize";
        public override bool IsQuestConditionMet(FishData fishData, FishSize fishSize)
        {
            return fish == fishData;
        }
    }
}