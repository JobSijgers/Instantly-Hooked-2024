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
            ViewManager.ShowView<GameView>();
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}