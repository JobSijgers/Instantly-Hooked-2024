using Events;
using UnityEngine;
using Views;

namespace Quests
{
    public class QuestBookUI : View
    {
        public static QuestBookUI instance;
        [SerializeField] private GameObject questDisplayPrefab;
        [SerializeField] private Transform questDisplayParent;
        [SerializeField] private HighlightQuestUI questUIHighlight;

        private ExpandedQuestDetailUI[] questDetails;

        private void Awake() => instance = this;
        
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

        public override void Show()
        {
            base.Show();
            LoadQuests();
        }
        
        public void ChangeBookPage(View newView)
        {
            ViewManager.ShowView(newView, false);
        }
    }
}