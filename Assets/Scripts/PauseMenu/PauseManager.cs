using System;
using Events;
using UnityEngine;

namespace PauseMenu
{
    public enum PauseState
    {
        Playing,
        InPauseMenu,
        InInventory,
        InCatalogue,
        InQuests
    }

    public class PauseManager : MonoBehaviour
    {
        private bool isEnabled = true;
        private static PauseState currentState;


        private void Start()
        {
            EventManager.ArrivedAtShore += DisableManager;
            EventManager.LeftShore += EnableManager;
        }

        private void OnDestroy()
        {
            EventManager.ArrivedAtShore -= DisableManager;
            EventManager.LeftShore -= EnableManager;
        }

        private void Update()
        {
            if (!isEnabled)
                return;

            CheckInventoryKey();
            CheckEscapeKey();
            CheckJournalKey();
        }

        private void DisableManager()
        {
            isEnabled = false;
        }

        private void EnableManager()
        {
            isEnabled = true;
        }

        private void CheckJournalKey()
        {
            if (!Input.GetKeyDown(KeyCode.J)) return;

            switch (currentState)
            {
                case PauseState.Playing:
                    SetState(PauseState.InCatalogue);
                    break;
                case PauseState.InPauseMenu:
                    break;
                case PauseState.InInventory:
                    break;
                case PauseState.InCatalogue:
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InQuests:
                    break;
            }
        }

        private void CheckInventoryKey()
        {
            if (!Input.GetKeyDown(KeyCode.I)) return;

            switch (currentState)
            {
                case PauseState.Playing:
                    SetState(PauseState.InInventory);
                    break;
                case PauseState.InPauseMenu:
                    break;
                case PauseState.InInventory:
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InCatalogue:
                    break;
                case PauseState.InQuests:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckEscapeKey()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            switch (currentState)
            {
                case PauseState.Playing:
                    SetState(PauseState.InPauseMenu);
                    break;

                case PauseState.InPauseMenu:
                    SetState(PauseState.Playing);
                    break;

                case PauseState.InInventory:
                    SetState(PauseState.Playing);
                    break;
            }
        }

        public static void SetState(PauseState newState, bool suppressEvent = false)
        {
            currentState = newState;

            if (!suppressEvent)
                EventManager.OnPauseSateChange(currentState);
        }

        public void UnPause()
        {
            SetState(PauseState.Playing);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}