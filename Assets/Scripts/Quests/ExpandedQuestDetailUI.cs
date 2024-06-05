using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Quests
{
    public class ExpandedQuestDetailUI : QuestDetailUI
    {
        [SerializeField] private TMP_Text questName;
        [SerializeField] private TMP_Text completionMoney;
        [SerializeField] private Button selectionButton;
        private QuestProgress progress;
        
        public event UnityAction<QuestProgress> OnQuestSelected;
        private void SelectQuest() => OnQuestSelected?.Invoke(progress);

        private void OnEnable()
        {
            selectionButton.onClick.AddListener(SelectQuest);
        }

        private void OnDisable()
        {
            selectionButton.onClick.RemoveListener(SelectQuest);
        }

        public override void SetQuest(QuestProgress questProgress)
        {
            base.SetQuest(questProgress);
            progress = questProgress;
            completionMoney.text = questProgress.completionMoney.ToString();
            questName.text = questProgress.quest.questName;
        }

        public override void ClearDetail()
        {
            base.ClearDetail();
            progress = null;
            completionMoney.text = "";
            questName.text = "";
        }
    }
}