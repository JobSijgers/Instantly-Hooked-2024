using System;
using Events;
using UnityEngine;

namespace Shore
{
    public class ShoreUIManager : MonoBehaviour
    {
        
        private void Start()
        {
            EventManager.ArrivedAtShore += ShowShoreUI;
            EventManager.LeftShore += HideShoreUI;
        }

        private void OnDestroy()
        {
            EventManager.ArrivedAtShore -= ShowShoreUI;
            EventManager.LeftShore -= HideShoreUI;
        }
        
        private void ShowShoreUI()
        {
            
        }
        
        private void HideShoreUI()
        {
            
        }
    }
}