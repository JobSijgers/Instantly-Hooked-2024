using System;
using Quests.ScriptableObjects;
using Unity.VisualScripting;

namespace Quests
{
    [Serializable]
    public class QuestProgress
    {
        public readonly Quest quest;
        public readonly int completionAmount;
        public readonly int completionMoney;
        public QuestDifficulty difficulty;
        public int progress;
        public bool highlighted;

        public QuestProgress(Quest quest, int completionAmount, int completionMoney, QuestDifficulty difficulty)
        {
            this.quest = quest;
            this.completionAmount = completionAmount;
            this.completionMoney = completionMoney;
            this.difficulty = difficulty;
            highlighted = false;
            progress = 0;
        }
    }
}