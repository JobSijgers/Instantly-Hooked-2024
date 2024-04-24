using System;
using Events;
using Unity.VisualScripting;
using UnityEngine;

namespace PauseMenu
{
    public enum PauseState
    {
        Playing,
        InPauseMenu,
        InInventory
    }

    public class PauseManager : MonoBehaviour
    {
        private PauseState _currentState;

        private void Update()
        {
            CheckInventoryKey();
            CheckEscapeKey();
        }

        private void CheckInventoryKey()
        {
            if (!Input.GetKeyDown(KeyCode.I)) return;

            switch (_currentState)
            {
                case PauseState.Playing:
                    SetNewState(PauseState.InInventory);
                    break;
                case PauseState.InPauseMenu:
                    break;
                case PauseState.InInventory:
                    SetNewState(PauseState.Playing);

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
                    SetNewState(PauseState.InPauseMenu);
                    break;
                
                case PauseState.InPauseMenu:
                    SetNewState(PauseState.Playing);
                    break;
                
                case PauseState.InInventory:
                    SetNewState(PauseState.Playing);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetNewState(PauseState newState)
        {
            _currentState = newState;
            EventManager.OnPauseSateChange(_currentState);
        }

        public void UnPause()
        {
            SetNewState(PauseState.Playing);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}