using Catalogue;
using Events;
using PauseMenu;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class QuestBookUI : MonoBehaviour
    {
        [SerializeField] private GameObject questBookUIParent;
        [SerializeField] private GameObject questDisplayPrefab;
        [SerializeField] private Transform questDisplayParent;
        [SerializeField] private HighlightQuestUI questUIHighlight;

        private ExpandedQuestDetailUI[] questDetails;

        private int currentPage;

        private void Start()
        {
            EventManager.PauseStateChange += OnPauseStateChange;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPauseStateChange;
        }

        public void OpenQuests(bool suppressEvent)
        {
            questBookUIParent.SetActive(true);
            LoadQuests();
            PauseManager.SetState(PauseState.InCatalogue, suppressEvent);
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
    }
}