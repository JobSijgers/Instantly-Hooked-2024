using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using PauseMenu;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace GameTime
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;
        [SerializeField] private int dayStartMinutes;
        [SerializeField] private float minutesPerCycle;
        [SerializeField] private Image fadeImage;

        private bool timePassing = true;
        private int currentDay;
        private float timeMultiplier;
        private float currentTime;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            // Calculate time multiplier based on minutes per cycle
            timeMultiplier = 1440f / minutesPerCycle;
            EndDay();
            
            ViewManager.instance.ViewShow += CheckPause;
        }


        private void OnDestroy()
        {
            ViewManager.instance.ViewShow -= CheckPause;
        }

        private void Update()
        {
            if (!timePassing)
                return;

            // Update current time
            currentTime += Time.deltaTime * timeMultiplier;
            TimeSpan span = TimeSpan.FromSeconds(currentTime);
            EventManager.OnTimeUpdate(span);
        }

        private void ResetTime()
        {
            // Reset current time to the start of the day
            currentTime = dayStartMinutes * 60;
        }

        public void NewDay()
        {
            StartCoroutine(EndDayTransition());
        }
        
        private void EndDay()
        {
            currentDay++;

            // Reset time and broadcast new day and time update events
            ResetTime();
            TimeSpan span = TimeSpan.FromSeconds(currentTime);
            EventManager.OnNewDay(currentDay);
            EventManager.OnTimeUpdate(span);
        }
        
        private IEnumerator EndDayTransition()
        {
            yield return Fade(0, 1, 1);
            yield return new WaitForSeconds(1.5f);
            EndDay();
            yield return Fade(1, 0, 1);
        }
        
        public void GetTimeState(out int currentday)
        {
            currentday = currentDay;
        }

        public void SetDay(int currentday)
        {
            currentDay = currentday;
        }
        
        private void CheckPause(View newView)
        {
            timePassing = newView is GameView;
        }
        
        
        private IEnumerator Fade(float start, float end, float duration)
        {
            float time = 0;
            Color startColor = new Color(0, 0, 0, start);
            Color endColor = new Color(0, 0, 0, end);
            while (time < duration)
            {
                fadeImage.color = Color.Lerp(startColor, endColor, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            fadeImage.color = endColor;
        }
    }
}