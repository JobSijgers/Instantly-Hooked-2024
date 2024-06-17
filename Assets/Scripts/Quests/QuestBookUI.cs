using System;
using Events;
using PauseMenu;
using UnityEngine;

namespace Quests
{
    public class QuestBookUI : MonoBehaviour
    {
        public static QuestBookUI instance;
        [SerializeField] private GameObject questBookUIParent;
        [SerializeField] private GameObject questDisplayPrefab;
        [SerializeField] private Transform questDisplayParent;
        [SerializeField] private HighlightQuestUI questUIHighlight;

        private ExpandedQuestDetailUI[] questDetails;

        private void Awake() => instance = this;

        private void Start()
        {
            EventManager.PauseStateChange += OnPauseStateChange;
            EventManager.HUDQuestSelected += OnHUDQuestClicked;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPauseStateChange;
            EventManager.HUDQuestSelected -= OnHUDQuestClicked;
        }

        public void OpenQuests(bool suppressEvent)
        {
            questBookUIParent.SetActive(true);
            LoadQuests();
            PauseManager.SetState(PauseState.InQuests, suppressEvent);
        }

        private void LoadQuests()
        {
            ClearQuests();
            QuestProgress[] quests = QuestTracker.instance.GetQuests();
            questDetails = new ExpandedQuestDetailUI[quests.Length];

            for (int i = 0; i < quests.Length; i++)
            {
                if (questDetails[i] == null)
                {
                    ExpandedQuestDetailUI questDetail = Instantiate(questDisplayPrefab, questDisplayParent)
                        .GetComponent<ExpandedQuestDetailUI>();

                    questDetail.OnQuestSelected += HighlightQuest;
                    questDetails[i] = questDetail;
                }

                questDetails[i].SetQuest(quests[i]);
            }
        }

        private void ClearQuests()
        {
            if (questDetails == null || questDetails.Length == 0)
                return;


            foreach (ExpandedQuestDetailUI questDetail in questDetails)
            {
                if (questDetail == null) continue;

                questDetail.OnQuestSelected -= HighlightQuest;
                Destroy(questDetail.gameObject);
            }

            questUIHighlight.ClearDetail();
        }

        private void HighlightQuest(QuestProgress quest)
        {
            questUIHighlight.SetQuest(quest);
        }

        public void CloseQuests(bool suppressEvent)
        {
            if (!questBookUIParent.activeSelf)
                return;
            questBookUIParent.SetActive(false);
            PauseManager.SetState(PauseState.Playing, suppressEvent);
        }


        private void OnPauseStateChange(PauseState newState)
        {
            if (newState == PauseState.InQuests)
            {
                OpenQuests(true);
            }
            else
            {
                CloseQuests(true);
            }
        }

        private void OnHUDQuestClicked(QuestProgress questProgress)
        {
            OpenQuests(false);
            HighlightQuest(questProgress);
        }
    }
}