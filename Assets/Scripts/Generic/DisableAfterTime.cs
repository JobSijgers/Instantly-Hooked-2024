using System;
using Events;
using UnityEngine;

namespace Generic
{
    public class DisableAfterTime : MonoBehaviour
    {
        [SerializeField] private float timeToDisable;
        
        private void Start()
        {
            EventManager.TimeUpdate += CheckTime;
        }
        
        private void OnDestroy()
        {
            EventManager.TimeUpdate -= CheckTime;
        }

        private void CheckTime(TimeSpan arg0)
        {
            gameObject.SetActive(arg0.TotalMinutes < timeToDisable);
        }
    }
}