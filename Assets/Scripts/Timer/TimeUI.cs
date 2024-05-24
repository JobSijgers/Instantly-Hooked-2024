using System;
using Events;
using PauseMenu;
using TMPro;
using UnityEngine;

namespace Timer
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeUIText;
        [SerializeField] private TMP_Text dayUIText;

        private void Start()
        {
            EventManager.PauseStateChange += OnPauseStateChange;

        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPauseStateChange;
        }
        
        private void OnEnable()
        {
            EventManager.TimeUpdate += UpdateTimeUI;
            EventManager.NewDay += UpdateDayUI;
        }

        private void OnDisable()
        {
            EventManager.TimeUpdate -= UpdateTimeUI;
            EventManager.NewDay -= UpdateDayUI;
        }

        private void UpdateTimeUI(float time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            //timeUIText.text = timeSpan.ToString(@"hh\:mm");
        }

        private void UpdateDayUI(int newDay)
        {
            dayUIText.text = $"Day: {newDay}";
        }

        private void OnPauseStateChange(PauseState pauseState)
        {
            switch (pauseState)
            {
                case PauseState.Playing:
                    gameObject.SetActive(true);
                    break;
                case PauseState.InPauseMenu:
                    gameObject.SetActive(false);
                    break;
                case PauseState.InInventory:
                    gameObject.SetActive(false);
                    break;
                case PauseState.InCatalogue:
                    gameObject.SetActive(false);
                    break;
                case PauseState.InQuests:
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}