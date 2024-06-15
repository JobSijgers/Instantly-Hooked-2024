using System;
using Events;
using PauseMenu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] private int maxQuestsDisplayed = 3;
        [SerializeField] private GameObject questDetailUIPrefab;
        [SerializeField] private Transform questDetailUIParent;

        private HUDQuest[] hudQuests;

        private void Awake()
        {
            hudQuests = new HUDQuest[maxQuestsDisplayed];
            for (int i = 0; i < maxQuestsDisplayed; i++)
            {
                GameObject questDetailUI = Instantiate(questDetailUIPrefab, questDetailUIParent);
                hudQuests[i] = questDetailUI.GetComponent<HUDQuest>();
            }
        }
        
        private void OnEnable()
        {
            EventManager.QuestHighlighted += HighlightQuest;
            EventManager.QuestUnHighlighted += UnhighlightQuest;
            EventManager.QuestUpdated += UpdateQuestProgress;
            EventManager.QuestCompleted += UnhighlightQuest;
            EventManager.PauseStateChange += ChangeActiveState;
        }

        private void ChangeActiveState(PauseState state)
        {
            questDetailUIParent.gameObject.SetActive(state == PauseState.Playing);
        }

        private void OnDisable()
        {
            EventManager.QuestHighlighted -= HighlightQuest;
            EventManager.QuestUnHighlighted -= UnhighlightQuest;
            EventManager.QuestUpdated -= UpdateQuestProgress;
            EventManager.QuestCompleted -= UnhighlightQuest;
            EventManager.PauseStateChange -= ChangeActiveState;
        }

        private void HighlightQuest(QuestProgress highlightedQuest)
        {
            int freeQuestSlot = GetFreeQuestSlot();
            if (freeQuestSlot == -1) return;
            {
                highlightedQuest.highlighted = true;
                hudQuests[freeQuestSlot].SetQuest(highlightedQuest);
            }
        }
        
        private void UnhighlightQuest(QuestProgress unHighlightedQuest)
        {
            int questSlot = GetQuestSlot(unHighlightedQuest);
            if (questSlot == -1) return;

            unHighlightedQuest.highlighted = false;
            hudQuests[questSlot].ClearDetail();
        }
        
        private void UpdateQuestProgress(QuestProgress questProgress)
        {
            foreach (HUDQuest hudQuest in hudQuests)
            {
                if (!hudQuest.IsInUse()) continue;

                if (hudQuest.GetQuest() != questProgress.quest) continue;
                
                hudQuest.SetQuest(questProgress);
                return;
            }
        }

        private int GetQuestSlot(QuestProgress questProgress)
        {
            for (int i = 0; i < hudQuests.Length; i++)
            {
                if (hudQuests[i].GetQuest() == questProgress.quest)
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetFreeQuestSlot()
        {
            for (int i = 0; i < hudQuests.Length; i++)
            {

                if (!hudQuests[i].IsInUse())
                {
                    return i;
                }
            }

            return -1;
        }
    }
}