using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Quests
{
    public class HighlightQuestUI : QuestDetailUI
    {
        [SerializeField] private TMP_Text questName;
        [SerializeField] private TMP_Text completionMoney;
        [SerializeField] private TMP_Text questDescription;
        [SerializeField] private Image progressImage;
        
        public override void SetQuest(QuestProgress questProgress)
        {
            base.SetQuest(questProgress);
            questName.text = questProgress.quest.questName;
            completionMoney.text = questProgress.completionMoney.ToString();
            questDescription.text = questProgress.quest.questDescription;
            progressImage.fillAmount = (float) questProgress.progress / questProgress.completionAmount;
        }

        public override void ClearDetail()
        {
            base.ClearDetail();
            questName.text = "";
            completionMoney.text = "";
            questDescription.text = "";
            progressImage.fillAmount = 0;
        }
    }
}