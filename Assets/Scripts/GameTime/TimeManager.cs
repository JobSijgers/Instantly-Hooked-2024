using System;
using Events;
using PauseMenu;
using UnityEngine;

namespace GameTime
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private int dayStartMinutes;
        [SerializeField] private float minutesPerCycle;

        private bool timePassing = true;
        private int currentDay;
        private float timeMultiplier;
        private float currentTime;

        private void Start()
        {
            // Calculate time multiplier based on minutes per cycle
            timeMultiplier = 1440f / minutesPerCycle;
            EndDay();
            
            // Subscribe to various game events
            EventManager.PauseStateChange += OnPause;
            EventManager.LeftShore += EnableTime;
            EventManager.ArrivedAtShore += DisableTime;
            EventManager.PlayerDied += EndDay;
        }

        private void OnDestroy()
        {
            // Unsubscribe from game events when this object is destroyed
            EventManager.PauseStateChange -= OnPause;
            EventManager.LeftShore -= EnableTime;
            EventManager.ArrivedAtShore -= DisableTime;
            EventManager.PlayerDied -= EndDay;
        }

        private void Update()
        {
            if (!timePassing)
                return;

            // Update current time
            currentTime += Time.deltaTime * timeMultiplier;
            EventManager.OnTimeUpdate(currentTime);

            // Check if a new day has started
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
            if (timeSpan.Days >= 1)
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
            EventManager.OnNewDay(currentDay);
            EventManager.OnTimeUpdate(currentTime);
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
            timePassing = true;
        }
    }
}