using System;
using Events;
using UnityEngine;

namespace Boat
{
    public class BoatAutoDocker : MonoBehaviour
    {
        [SerializeField] private int dockTime;
        
        private void Start()
        {
            EventManager.TimeUpdate += CheckTime;
        }

        private void OnDestroy()
        {
            EventManager.TimeUpdate -= CheckTime;
        }
        
        private void CheckTime(float time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            if (span.TotalMinutes >= dockTime)
            {
                EventManager.OnBoatAutoDock();
            }
        }
    }
}