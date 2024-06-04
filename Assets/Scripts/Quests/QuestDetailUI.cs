using System;
using Quests.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quests
{
    public class QuestDetailUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text questProgressAmount;
        [SerializeField] private TMP_Text questCompletionMoney;
        [FormerlySerializedAs("questProgress")] public Quest quest;
        public bool isUse;

        public void SetQuest(QuestProgress questProgress)
        {
            questCompletionMoney.text = questProgress.completionMoney.ToString();
            questProgressAmount.text = $"{questProgress.progress}/{questProgress.completionAmount}";
            quest = questProgress.quest;
            isUse = true;
        }

        public void ClearDetail()
        {
            questCompletionMoney.text = "";
            questProgressAmount.text = "";
            quest = null;
            isUse = false;
        }
    }
}