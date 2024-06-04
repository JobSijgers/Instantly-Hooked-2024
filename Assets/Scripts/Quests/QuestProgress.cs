using Quests.ScriptableObjects;

namespace Quests
{
    public class QuestProgress
    {
        public readonly Quest quest;
        public readonly int completionAmount;
        public readonly int completionMoney;
        public int progress;

        public QuestProgress(Quest quest, int completionAmount, int completionMoney)
        {
            this.quest = quest;
            this.completionAmount = completionAmount;
            this.completionMoney = completionMoney;
            progress = 0;
        }
    }
}