using System;
using Events;
using PauseMenu;
using TMPro;
using UnityEngine;

namespace GameTime
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

        private void UpdateTimeUI(TimeSpan time)
        {
            int minutes = RoundDownToNearestFive(time.Minutes);
            TimeSpan roundedTimeSpan = new (time.Hours, minutes, time.Seconds);
            timeUIText.text = roundedTimeSpan.ToString(@"hh\:mm");
        }

        private static int RoundDownToNearestFive(int number)
        {
            //Since integers automatically round down there is no need to use mahtf.floor
            return number / 5 * 5;
        }

        private void UpdateDayUI(int newDay)
        {
            dayUIText.text = $"Day: {newDay}";
        }
        
        private void OnPauseStateChange(PauseState pauseState)
        {
            if (pauseState == PauseState.Playing)
            {
                gameObject.SetActive(true);
                return;
            }
            gameObject.SetActive(false);
        }
    }
}