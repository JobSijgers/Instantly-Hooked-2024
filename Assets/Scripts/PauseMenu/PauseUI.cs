using System;
using System.Diagnostics;
using Events;
using UnityEngine;

namespace PauseMenu
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUI;
        private void Start()
        {
            EventManager.PauseStateChange += OnPause;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
        }

        private void OnPause(PauseState newState)
        {
            switch (newState)
            {
                case PauseState.Playing:
                    pauseUI.SetActive(false);
                    break;
                case PauseState.InPauseMenu:
                    pauseUI.SetActive(true);
                    break;
                case PauseState.InInventory:
                    pauseUI.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }
}