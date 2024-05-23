using System;
using Events;
using PauseMenu;
using UnityEngine;

namespace Timer
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;
        [SerializeField] private int dayStartMinutes;
        [SerializeField] private float minutesPerCycle;
        private bool timePassing = true;
        private int currentDay;
        private float timeMultiplier;
        private float currentTime;
        private float testCurrent;

        private void Start()
        {
            instance = this;
            timeMultiplier = 1440f / minutesPerCycle;
            EndDay();
            EventManager.PauseStateChange += OnPause;
            EventManager.LeftShore += EnableTime;
            EventManager.ArrivedAtShore += DisableTime;
            EventManager.PlayerDied += EndDay;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
            EventManager.LeftShore -= EnableTime;
            EventManager.ArrivedAtShore -= DisableTime;
            EventManager.PlayerDied -= EndDay;
        }

        private void Update()
        {
            if (!timePassing)
                return;
            currentTime += Time.deltaTime * timeMultiplier;

            EventManager.OnTimeUpdate(currentTime);
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
            if (timeSpan.Days >= 1)
            {
                EndDay();
            }
        }

        private void ResetTime()
        {
            currentTime = dayStartMinutes * 60;
        }

        public void EndDay()
        {
            currentDay++;
            ResetTime();
            EventManager.OnNewDay(currentDay);
            EventManager.OnTimeUpdate(currentTime);
        }

        private void OnPause(PauseState newState)
        {
            switch (newState)
            {
                case PauseState.Playing:
                    EnableTime();
                    break;
                case PauseState.InPauseMenu:
                    DisableTime();
                    break;
                case PauseState.InInventory:
                    DisableTime();
                    break;
                case PauseState.InCatalogue:
                    DisableTime();
                    break;
                case PauseState.InQuests:
                    DisableTime();
                    break;
            }
        }

        private void DisableTime()
        {
            timePassing = false;
        }

        private void EnableTime()
        {
            timePassing = true;
        }

        public void GetTimeState(out int currentday, out float currenttime)
        {
            currentday = currentDay;
            currenttime = currentTime;
        }
        public void SetTime(int currentday, float currenttime)
        {
            currentDay = currentday;
            currentTime = currenttime;
        }
    }
}