using System;
using Events;
using PauseMenu;
using Unity.VisualScripting;
using UnityEngine;

namespace Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] private int maxQuestsDisplayed = 3;
        [SerializeField] private GameObject questDetailUIPrefab;
        [SerializeField] private Transform questDetailUIParent;

        private QuestDetailUI[] questDetailUIs;

        private void Awake()
        {
            questDetailUIs = new QuestDetailUI[maxQuestsDisplayed];
            for (int i = 0; i < maxQuestsDisplayed; i++)
            {
                GameObject questDetailUI = Instantiate(questDetailUIPrefab, questDetailUIParent);
                questDetailUIs[i] = questDetailUI.GetComponent<QuestDetailUI>();
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
                questDetailUIs[freeQuestSlot].SetQuest(highlightedQuest);
            }
        }

        private void UnhighlightQuest(QuestProgress unHighlightedQuest)
        {
            int questSlot = GetQuestSlot(unHighlightedQuest);
            if (questSlot == -1) return;

            unHighlightedQuest.highlighted = false;
            questDetailUIs[questSlot].ClearDetail();
        }

        private void UpdateQuestProgress(QuestProgress questProgress)
        {
            foreach (QuestDetailUI questDetailUI in questDetailUIs)
            {
                if (!questDetailUI.IsInUse()) continue;

                if (questDetailUI.GetQuest() != questProgress.quest) continue;
                
                questDetailUI.SetQuest(questProgress);
                return;
            }
        }

        private int GetQuestSlot(QuestProgress questProgress)
        {
            for (int i = 0; i < questDetailUIs.Length; i++)
            {
                if (questDetailUIs[i].GetQuest() == questProgress.quest)
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetFreeQuestSlot()
        {
            for (int i = 0; i < questDetailUIs.Length; i++)
            {

                if (!questDetailUIs[i].IsInUse())
                {
                    return i;
                }
            }

            return -1;
        }
    }
}