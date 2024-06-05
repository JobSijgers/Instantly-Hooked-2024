using System;
using Quests.ScriptableObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Quests
{
    public class QuestDetailUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text questProgressAmount;
        [SerializeField] private Image questIcon;
        private Quest quest; 
        private bool inUse;

        public virtual void SetQuest(QuestProgress questProgress)
        {
            questIcon.sprite = questProgress.quest.questIcon;
            questProgressAmount.text = $"{questProgress.progress}/{questProgress.completionAmount}";
            quest = questProgress.quest;
            inUse = true;
        }

        public virtual void ClearDetail()
        {
            questIcon.sprite = null;
            questProgressAmount.text = "";
            quest = null;
            inUse = false;
        }

        public bool IsInUse() => inUse;
        public Quest GetQuest() => quest;
    }
}