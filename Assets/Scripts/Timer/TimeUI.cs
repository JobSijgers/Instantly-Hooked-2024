using System;
using Events;
using TMPro;
using UnityEngine;

namespace Timer
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeUIText;
        [SerializeField] private TMP_Text dayUIText;

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
            timeUIText.text = timeSpan.ToString(@"hh\:mm");
        }

        private void UpdateDayUI(int newDay)
        {
            dayUIText.text = $"Day: {newDay}";
        }
    }
}