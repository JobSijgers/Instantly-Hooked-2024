using Events;
using Views;

namespace Quests
{
    public class HUDQuest : QuestDetailUI
    {
        public void ButtonPressed()
        {
            ViewManager.ShowView<QuestBookUI>();
        }
    }
}