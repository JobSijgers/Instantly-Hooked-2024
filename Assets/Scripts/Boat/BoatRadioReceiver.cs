using System;
using Events;
using UnityEngine;

namespace Boat
{
    public class BoatRadioReceiver : MonoBehaviour
    {
        [SerializeField] private int messageArriveTime;
        [SerializeField] private GameObject message;
        private bool messageArrived = false;
        
        private void Start()
        {
            EventManager.NewDay += ResetMessage;
        }
        private void OnEnable()
        {
            EventManager.TimeUpdate += CheckMessage;
        }

        private void OnDisable()
        {
            EventManager.TimeUpdate -= CheckMessage;
        }

        private void OnDestroy()
        {
            EventManager.NewDay -= ResetMessage;
        }

        private void ResetMessage(int newDay)
        {
            messageArrived = false;
            message.SetActive(false);
        }
        private void CheckMessage(float time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            if (span.TotalMinutes >=  messageArriveTime && !messageArrived)
            {
                BroadcastMessage();
            }
        }
        
        private void BroadcastMessage()
        {
            message.SetActive(true);
            messageArrived = true;
        }

        public void DisableMessage()
        {
            message.SetActive(false);
        }
    }
}