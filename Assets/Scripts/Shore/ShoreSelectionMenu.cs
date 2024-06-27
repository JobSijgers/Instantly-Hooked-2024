using System;
using Events;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Shore
{
    public class ShoreSelectionMenu : ViewComponent
    {
        [SerializeField] private int notToSeaAfterTime;
        [SerializeField] private Button seaButton;

        private bool isDead;

        private void Start()
        {
            EventManager.TimeUpdate += CheckTime;
            EventManager.NewDay += EnableSeaButton;
            EventManager.PlayerDied += PlayerDied;
        }

        private void OnDestroy()
        {
            EventManager.TimeUpdate -= CheckTime;
            EventManager.NewDay -= EnableSeaButton;
            EventManager.PlayerDied -= PlayerDied;
        }

        private void CheckTime(TimeSpan time)
        {
            if (isDead)
                return;
            seaButton.gameObject.SetActive((time.TotalMinutes < notToSeaAfterTime));
        }

        private void EnableSeaButton(int arg0)
        {
            isDead = false;
            seaButton.gameObject.SetActive(true);
        }

        private void PlayerDied()
        {
            isDead = true;
            seaButton.gameObject.SetActive(false);
        }

        public void GoToSea()
        {
            EventManager.OnLeftShore();
        }
    }
}