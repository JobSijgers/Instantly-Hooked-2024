using UnityEngine;
using Views;

namespace Generic
{
    public class QuickActionsUI : ViewComponent
    {
        public void OpenView(View view)
        {
            ViewManager.ShowView(view);
        }
    }
}