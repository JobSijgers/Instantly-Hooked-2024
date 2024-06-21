using Enums;
using Fish;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Create CatchSizeQuest", fileName = "CatchSizeQuest", order = 0)]
    public class CatchSizeQuest : Quest
    {
        public FishSize size;
        
        public override bool IsQuestConditionMet(FishData fishData, FishSize fishSize)
        {
            return size == fishSize;
        }
    }
}