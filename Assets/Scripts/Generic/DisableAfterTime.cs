using System;
using Events;
using UnityEngine;

namespace Generic
{
    public class DisableAfterTime : MonoBehaviour
    {
        [SerializeField] private float timeToDisable;
        [SerializeField] private bool resetOnNewDay;

        private void Start()
        {
            EventManager.TimeUpdate += CheckTime;
            if (resetOnNewDay)
            {
                EventManager.NewDay += Reset;
            }
        }

        private void OnDestroy()
        {
            EventManager.TimeUpdate -= CheckTime;
            if (resetOnNewDay)
            {
                EventManager.NewDay -= Reset;
            }
        }

        private void CheckTime(TimeSpan time)
        {
            gameObject.SetActive(time.TotalMinutes < timeToDisable);
        }

        private void Reset(int day)
        {
            gameObject.SetActive(true);
        }
    }
}