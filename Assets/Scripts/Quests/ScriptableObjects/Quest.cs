using System;
using Enums;
using Fish;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Quests.ScriptableObjects
{
    public abstract class Quest : ScriptableObject
    {
        [Header("Quest Info")]
        public int minAmount;
        public int maxAmount;
        public string questDescription;
        public Sprite questIcon;
        
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

