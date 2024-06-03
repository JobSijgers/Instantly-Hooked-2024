using System.Collections.Generic;
using Enums;
using Events;
using Fish;
using Quests.ScriptableObjects;
using UnityEngine;


namespace Quests
{
    public partial class QuestTracker : MonoBehaviour
    {
        [SerializeField] private QuestState[] questStates;
        private readonly Dictionary<QuestProgress, QuestDifficulty> activeQuests = new();
        
        private void OnEnable()
        {
            EventManager.FishCaught += IsQuestConditionMet;
        }

        private void OnDisable()
        {
            EventManager.FishCaught -= IsQuestConditionMet;
        }

        private void Start()
        {
            foreach (QuestState questState in questStates)
            {
                for (int i = 0; i < questState.maxQuestsActive; i++)
                {
                    GenerateNewQuest(questState.difficulty);
                }
            }
        }

        private void GenerateNewQuest(QuestDifficulty difficulty)
        {
            foreach (QuestState questState in questStates)
            {
                // Skip if the quest state is not the same as the difficulty
                if (questState.difficulty != difficulty) continue;
                
                // Generate a random quest from the available quests in difficulty
                Quest newQuest = questState.GetRandomQuest();
                
                // Generate a random amount and money for the quest
                int randomAmount = newQuest.GetRandomAmount();
                int randomMoney = newQuest.GetRandomCompletionMoney();
                
                // Create a new quest progress and add it to the active quests
                QuestProgress progress = new(newQuest, randomAmount, randomMoney);
                activeQuests.Add(progress, difficulty);
            }
        }

        private void IsQuestConditionMet(FishData fishData, FishSize fishSize)
        {
            foreach (QuestProgress activeQuest in activeQuests.Keys)
            {
                if (!activeQuest.Quest.IsQuestConditionMet(fishData, fishSize)) continue;
                activeQuest.Progress++;

                if (activeQuest.Progress >= activeQuest.CompletionAmount)
                {
                    QuestCompleted(activeQuest);
                }
            }
        }
        
        private void QuestCompleted(QuestProgress questProgress)
        {
            EventManager.OnQuestCompleted(questProgress);
            
            // Remove the quest from the active quests
            activeQuests.Remove(questProgress);
            
            // Generate a new quest of the same difficulty
            GenerateNewQuest(activeQuests[questProgress]);
        }
    }
}