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
        public Quest quest;
        public bool isUse;

        public virtual void SetQuest(QuestProgress questProgress)
        {
            questIcon.sprite = questProgress.quest.questIcon;
            questProgressAmount.text = $"{questProgress.progress}/{questProgress.completionAmount}";
            quest = questProgress.quest;
            isUse = true;
        }

        public virtual void ClearDetail()
        {
            questIcon.sprite = null;
            questProgressAmount.text = "";
            quest = null;
            isUse = false;
        }
    }
}