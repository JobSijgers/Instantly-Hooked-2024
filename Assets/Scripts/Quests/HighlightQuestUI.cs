using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quests
{
    public class HighlightQuestUI : ExpandedQuestDetailUI
    {
        [SerializeField] private TMP_Text questDescription;
        [SerializeField] private Image progressImage;
        
        public override void SetQuest(QuestProgress questProgress)
        {
            base.SetQuest(questProgress);
            questDescription.text = questProgress.quest.questDescription;
            progressImage.fillAmount = (float) questProgress.progress / questProgress.completionAmount;
        }

        public override void ClearDetail()
        {
            base.ClearDetail();

        }
    }
}