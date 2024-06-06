using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

namespace Economy
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        private int targetMoney;
        private int currentMoney;
        private bool isAnimating;
        private void Start()
        {
            EventManager.MoneyUpdate += UpdateMoneyUI;
        }

        private void OnDestroy()
        {
            EventManager.MoneyUpdate -= UpdateMoneyUI;
        }
        
        private IEnumerator AnimateMoneyCount()
        {
            isAnimating = true;
            const float duration = 2.0f; // duration of the animation in seconds
            float elapsed = 0.0f;

            while (currentMoney < targetMoney)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float smoothStep = Mathf.SmoothStep(0, 1, t);

                currentMoney = (int)Mathf.Lerp(currentMoney, targetMoney, smoothStep);
                moneyText.text = currentMoney.ToString("N0");

                yield return null;
            }

            currentMoney = targetMoney; // ensure the final value is accurate
            isAnimating = false;
        }

        private void UpdateMoneyUI(int newMoney)
        {
            targetMoney = newMoney;
            if (!isAnimating)
            {
                StartCoroutine(AnimateMoneyCount());
            }
        }
    }
}