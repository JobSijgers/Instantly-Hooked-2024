using TMPro;
using UnityEngine;

namespace Quests
{
    public class ExpandedQuestDetailUI : QuestDetailUI
    {
        [SerializeField] private TMP_Text questName;
        [SerializeField] private TMP_Text completionAmount;
        
        public override void SetQuest(QuestProgress questProgress)
        {
            base.SetQuest(questProgress);
            questName.text = questProgress.quest.questName;
            completionAmount.text = questProgress.completionAmount.ToString();
        }

        public override void ClearDetail()
        {
            base.ClearDetail();
            questName.text = "";
            completionAmount.text = "";
        }
    }
}