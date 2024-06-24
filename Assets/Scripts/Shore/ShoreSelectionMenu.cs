using Events;
using Views;

namespace Shore
{
    public class ShoreSelectionMenu : ViewComponent
    {
        public void GoToSea()
        {
            EventManager.OnLeftShore();
        }
    }
}