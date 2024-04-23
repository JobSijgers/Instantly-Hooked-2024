using System;
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
        private int _currentDay;
        private float _timeMultiplier;
        private float _currentTime;
        private float _testCurrent;

        private void Start()
        {
            _timeMultiplier = 1440f / minutesPerCycle;
            EndDay();
            EventManager.PauseStateChange += OnPause;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime * _timeMultiplier;

            EventManager.OnTimeUpdate(_currentTime);
            var timeSpan = TimeSpan.FromSeconds(_currentTime);
            if (timeSpan.Days >= 1)
            {
                EndDay();
            }
        }

        private void ResetTime()
        {
            _currentTime = dayStartMinutes * 60;
        }

        private void EndDay()
        {
            _currentDay++;
            ResetTime();
            EventManager.OnNewDay(_currentDay);
        }
        
        private void OnPause(PauseState newState)
        {
            enabled = newState switch
            {
                PauseState.Playing => true,
                PauseState.InPauseMenu => false,
                PauseState.InInventory => false,
                _ => throw new ArgumentOutOfRangeException(nameof(newState), newState, null)
            };
        }
    }
}