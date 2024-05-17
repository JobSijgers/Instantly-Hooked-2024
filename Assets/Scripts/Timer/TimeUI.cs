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
            int minutes = RoundDownToNearestFive(timeSpan.Minutes);
            TimeSpan roundedTimeSpan = new (timeSpan.Hours, minutes, timeSpan.Seconds);
            timeUIText.text = roundedTimeSpan.ToString(@"hh\:mm");
        }

        private int RoundDownToNearestFive(int number)
        {
            return number / 5 * 5;
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