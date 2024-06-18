﻿using System;
using System.Collections;
using Events;
using TMPro;
using UnityEngine;
using Views;

namespace Economy
{
    public class MoneyUI : ViewComponent
    {
        [SerializeField] private TMP_Text moneyText;
        private int targetMoney;
        private int currentMoney;

        private void Awake()
        {
            EventManager.MoneyUpdate += UpdateMoneyUI;
        }

        private void OnDestroy()
        {
            EventManager.MoneyUpdate -= UpdateMoneyUI;
        }

        private float EaseOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }

        private IEnumerator UpdateMoneySmoothly(int targetMoney, float duration)
        {
            float elapsed = 0;
            int startingMoney = currentMoney;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = EaseOutCubic(elapsed / duration);
                currentMoney = Mathf.RoundToInt(Mathf.Lerp(startingMoney, targetMoney, t));
                moneyText.text = currentMoney.ToString();
                yield return null;
            }

            currentMoney = targetMoney;
            moneyText.text = currentMoney.ToString();
        }

        private void UpdateMoneyUI(int newMoney)
        {
            targetMoney = newMoney;
            if (!gameObject.activeInHierarchy)
                return;
            targetMoney = newMoney;
            if (currentMoney == newMoney) return;

            StopAllCoroutines();
            StartCoroutine(UpdateMoneySmoothly(newMoney, 1.0f));
        }

        private void OnEnable()
        {
            StartCoroutine(UpdateMoneySmoothly(targetMoney, 1.0f));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}