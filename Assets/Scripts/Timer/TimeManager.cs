using System;
using System.Globalization;
using Events;
using PauseMenu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Timer
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private int dayStartMinutes;
        [SerializeField] private float minutesPerCycle;
        private bool timePassing = true;
        private int currentDay;
        private float timeMultiplier;
        private float currentTime;
        private float testCurrent;

        private void Start()
        {
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
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
    }
}