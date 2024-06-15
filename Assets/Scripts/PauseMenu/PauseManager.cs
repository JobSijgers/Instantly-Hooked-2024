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
        InQuests,
    }

    public class PauseManager : MonoBehaviour
    {
        private static PauseState _currentState;

        private void Update()
        {
            CheckInventoryKey();
            CheckEscapeKey();
            CheckJournalKey();
            CheckQKey();
        }

        private void CheckJournalKey()
        {
            if (!Input.GetKeyDown(KeyCode.J)) return;

            switch (_currentState)
            {
                case PauseState.Playing:
                    SetState(PauseState.InCatalogue);
                    break;
                case PauseState.InPauseMenu:
                    break;
                case PauseState.InInventory:
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InCatalogue:
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InQuests:
                    SetState(PauseState.Playing);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckInventoryKey()
        {
            if (!Input.GetKeyDown(KeyCode.I)) return;

            switch (_currentState)
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
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InQuests:
                    SetState(PauseState.Playing);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckEscapeKey()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            switch (_currentState)
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
                case PauseState.InCatalogue:
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InQuests:
                    SetState(PauseState.Playing);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckQKey()
        {
            if (!Input.GetKeyDown(KeyCode.Q)) return;
            switch (_currentState)
            {
                case PauseState.Playing:
                    SetState(PauseState.InQuests);
                    break;
                case PauseState.InPauseMenu:
                    break;
                case PauseState.InInventory:
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InCatalogue:
                    SetState(PauseState.Playing);
                    break;
                case PauseState.InQuests:
                    SetState(PauseState.Playing);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void SetState(PauseState newState, bool suppressEvent = false)
        {
            _currentState = newState;

            if (!suppressEvent)
                EventManager.OnPauseSateChange(_currentState);
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