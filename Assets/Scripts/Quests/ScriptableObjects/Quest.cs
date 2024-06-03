using Enums;
using Fish;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    public abstract class Quest : ScriptableObject
    {
        [Header("Quest Info")]
        public int minAmount;
        public int maxAmount;
        
        [Header("Completion Info")]        
        public int minCompletionMoney;
        public int maxCompletionMoney;
        
        public abstract string QuestType { get; }
        public abstract bool IsQuestConditionMet(FishData fishData, FishSize fishSize);
        
        public int GetRandomAmount()
        {
            return Random.Range(minAmount, maxAmount + 1);
        }
        
        public int GetRandomCompletionMoney()
        {
            return Random.Range(minCompletionMoney, maxCompletionMoney + 1);
        }
    }
};

