using Quests.ScriptableObjects;

namespace Quests
{
    public partial class QuestTracker
    {
        public class QuestProgress
        {
            public readonly Quest Quest;
            public readonly int CompletionAmount;
            public readonly int CompletionMoney;
            public int Progress;

            public QuestProgress(Quest quest, int completionAmount, int completionMoney)
            {
                Quest = quest;
                CompletionAmount = completionAmount;
                CompletionMoney = completionMoney;
                Progress = 0;
            }
        }
    }
}