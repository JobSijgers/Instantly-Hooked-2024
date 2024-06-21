using System;
using Events;
using UnityEngine;
using Views;

namespace PauseMenu
{
    public class PauseUI : View
    {
        public void Resume()
        {
            ViewManager.ShowLastView();
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}