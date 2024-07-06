using UnityEditor.Rendering;
using UnityEngine;
using Views;

namespace Book
{
    public class BookPage : View
    {
        public void ChangeBookPage(View view)
        {
            ViewManager.ShowView(view, false);
        }
    }
}