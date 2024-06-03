using System;
using Quests.ScriptableObjects;
using Random = UnityEngine.Random;

namespace Quests
{
    public partial class QuestTracker
    {
        [Serializable]
        public class QuestState
        {
            public QuestDifficulty difficulty;
            public Quest[] quests;
            public int maxQuestsActive;
            
            public Quest GetRandomQuest()
            {
                return quests[Random.Range(0, quests.Length)];
            }
        }
    }
}