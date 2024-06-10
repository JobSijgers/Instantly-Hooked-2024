using System;
using Events;
using PauseMenu;
using UnityEngine;

namespace GameTime
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
        private bool atShore;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            // Calculate time multiplier based on minutes per cycle
            timeMultiplier = 1440f / minutesPerCycle;
            EndDay();

            // Subscribe to various game events
            EventManager.PauseStateChange += OnPause;
            EventManager.LeftShore += LeftShore;
            EventManager.ArrivedAtShore += ArrivedAtShore;
            EventManager.PlayerDied += EndDay;
        }


        private void OnDestroy()
        {
            // Unsubscribe from game events when this object is destroyed
            EventManager.PauseStateChange -= OnPause;
            EventManager.LeftShore -= LeftShore;
            EventManager.ArrivedAtShore -= ArrivedAtShore;
            EventManager.PlayerDied -= EndDay;
        }

        private void Update()
        {
            if (!timePassing)
                return;

            // Update current time
            currentTime += Time.deltaTime * timeMultiplier;
            TimeSpan span = TimeSpan.FromSeconds(currentTime);
            EventManager.OnTimeUpdate(span);

            // Check if a new day has started
            if (span.Days >= 1)
            {
                EndDay();
            }
        }

        private void ResetTime()
        {
            // Reset current time to the start of the day
            currentTime = dayStartMinutes * 60;
        }

        public void EndDay()
        {
            currentDay++;

            // Reset time and broadcast new day and time update events
            ResetTime();
            TimeSpan span = TimeSpan.FromSeconds(currentTime);
            EventManager.OnNewDay(currentDay);
            EventManager.OnTimeUpdate(span);
        }

        private void OnPause(PauseState newState)
        {
            // Handle pause state changes
            switch (newState)
            {
                case PauseState.Playing:
                    EnableTime();
                    break;
                case PauseState.InPauseMenu:
                case PauseState.InInventory:
                case PauseState.InCatalogue:
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
            if (atShore)
                return;
            timePassing = true;
        }

        public void GetTimeState(out int currentday)
        {
            currentday = currentDay;
        }

        public void SetDay(int currentday)
        {
            currentDay = currentday;
        }

        private void LeftShore()
        {
            atShore = false;
            EnableTime();
        }

        private void ArrivedAtShore()
        {
            atShore = true;
            DisableTime();
        }
    }
}