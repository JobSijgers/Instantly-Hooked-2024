using Enums;
using Fish;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Quests/Create CatchFishAmount", fileName = "CatchFishAmount", order = 0)]
    public class CatchFishAmount : Quest
    {
        public override bool IsQuestConditionMet(FishData fishData, FishSize fishSize)
        {
            return true;
        }
    }
}