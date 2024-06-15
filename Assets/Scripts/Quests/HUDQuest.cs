using Events;

namespace Quests
{
    public class HUDQuest : QuestDetailUI
    {
        private QuestProgress progress;

        public override void SetQuest(QuestProgress questProgress)
        {
            base.SetQuest(questProgress);
            progress = questProgress;
        }

        public override void ClearDetail()
        {
            base.ClearDetail();
            progress = null;
        }

        public void ButtonPressed()
        {
            EventManager.OnHUDQuestSelected(progress);
        }
    }
}