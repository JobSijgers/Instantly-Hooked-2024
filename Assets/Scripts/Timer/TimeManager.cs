using System;
using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Timer
{
    public class TimeManager : MonoBehaviour
    {
       
        public static TimeManager instance;
        [SerializeField] private float minutesPerCycle;
        private bool _timeProgressing = true;
        private float _timeMultiplier;
        private float _currentTime;
        private float _testCurrent;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _timeMultiplier = 1440 / minutesPerCycle;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime * _timeMultiplier;
            
            EventManager.OnTimeUpdate(_currentTime);
        }
    }
}