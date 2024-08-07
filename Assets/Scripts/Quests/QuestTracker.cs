﻿using System;
using System.Collections.Generic;
using Audio;
using Enums;
using Events;
using Fish;
using Quests.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Quests
{
    public class QuestTracker : MonoBehaviour
    {
        [Serializable]
        public class QuestState
        {
            public QuestDifficulty difficulty;
            public Quest[] quests;
            public Quest[] startQuests;
            public int maxQuestsActive;

            public Quest GetRandomQuest()
            {
                return quests[Random.Range(0, quests.Length)];
            }
        }

        public static QuestTracker instance;
        [SerializeField] private QuestState[] questStates;
        private readonly List<QuestProgress> activeQuests = new();

        private void OnEnable()
        {
            EventManager.FishCaught += IsQuestConditionMet;
        }

        private void OnDisable()
        {
            EventManager.FishCaught -= IsQuestConditionMet;
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            foreach (QuestState questState in questStates)
            {
                if (questState.maxQuestsActive > questState.quests.Length)
                {
                    Debug.LogWarning(
                        "Max quests active is greater than the amount of quests available causing not all quest to be unique");
                }

                for (int i = 0; i < questState.maxQuestsActive; i++)
                {
                    //first check if there are any start quests
                    if (questState.startQuests.Length > 0 && i < questState.startQuests.Length)
                    {
                        //Generate start quest
                        Quest startQuest = questState.startQuests[i];
                        QuestProgress progress = new(startQuest, startQuest.GetRandomAmount(),
                            startQuest.GetRandomCompletionMoney(), questState.difficulty);
                        
                        activeQuests.Add(progress);
                        EventManager.OnQuestHighlight(progress);
                    }
                    else
                    {
                        //iterate through the max amount of quests active and generate a new quest
                        GenerateNewQuest(questState.difficulty);
                        EventManager.OnQuestHighlight(activeQuests[^1]);
                    }
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
                Quest newQuest = null;
                const int maxIterations = 30;

                for (int i = 0; i < maxIterations; i++)
                {
                    Quest quest = questState.GetRandomQuest();
                    if (i == maxIterations - 1)
                    {
                        newQuest = quest;
                        break;
                    }

                    if (DoesQuestExist(quest)) continue;
                    newQuest = quest;
                }
                
                // Generate a random amount and money for the quest
                if (newQuest == null) return;

                int randomAmount = newQuest.GetRandomAmount();
                int randomMoney = newQuest.GetRandomCompletionMoney();

                // Create a new quest progress and add it to the active quests
                QuestProgress progress = new(newQuest, randomAmount, randomMoney, difficulty);
                activeQuests.Add(progress);
            }
        }

        private bool DoesQuestExist(Quest newQuest)
        {
            foreach (QuestProgress activeQuest in activeQuests)
            {
                if (activeQuest.quest == newQuest)
                {
                    return true;
                }
            }

            return false;
        }

        private void IsQuestConditionMet(FishData fishData, FishSize fishSize)
        {
            List<QuestProgress> questCompleted = new();
            foreach (QuestProgress activeQuest in activeQuests)
            {
                if (!activeQuest.quest.IsQuestConditionMet(fishData, fishSize)) continue;
                activeQuest.progress++;
                EventManager.OnQuestUpdated(activeQuest);
                if (activeQuest.progress >= activeQuest.completionAmount)
                {
                    questCompleted.Add(activeQuest);
                }
            }

            foreach (QuestProgress quest in questCompleted)
            {
                QuestCompleted(quest);
            }
        }

        private void QuestCompleted(QuestProgress questProgress)
        {
            EventManager.OnQuestCompleted(questProgress);
            
            AudioManager.instance.PlaySound("QuestComplete");
            
            // Remove the quest from the active quests
            activeQuests.Remove(questProgress);

            // Generate a new quest of the same difficulty
            GenerateNewQuest(questProgress.difficulty);
            EventManager.OnQuestHighlight(activeQuests[activeQuests.Count - 1]);
        }
        
        public QuestProgress[] GetQuests()
        {
            return activeQuests.ToArray();
        }

        public void LoadQuests(QuestProgress[] quests)
        {
            if (quests == null) 
                return;
            
            if (activeQuests != null)
            {
                foreach (QuestProgress t in activeQuests)
                {
                    EventManager.OnQuestUnHighlight(t);
                }

                activeQuests.Clear();
            }
            foreach (QuestProgress t in quests)
            {
                activeQuests.Add(t);
                EventManager.OnQuestHighlight(activeQuests[activeQuests.Count - 1]);
            }
        }
    }
}