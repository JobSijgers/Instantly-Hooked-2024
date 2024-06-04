﻿using System;
using UnityEngine;
using Economy.ShopScripts;
using Events;
using Quests;
using Unity.VisualScripting;
using Upgrades.Scriptable_Objects;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager instance;

        private int currentMoney = 0;
        public int GetCurrentMoneyAmount() => currentMoney;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            EventManager.ShopSell += AddMoney;
            EventManager.QuestCompleted += AddMoney;
        }

        private void OnDestroy()
        {
            EventManager.ShopSell -= AddMoney;
            EventManager.QuestCompleted -= AddMoney;
        }

        public bool HasEnoughMoney(int purchaseAmount)
        {
            return currentMoney - purchaseAmount >= 0;
        }

        private void AddMoney(int addAmount)
        {
            currentMoney += addAmount;
            EventManager.OnMoneyUpdate(currentMoney); 
        }

        private void AddMoney(QuestProgress questProgress)
        {
            AddMoney(questProgress.completionMoney);
        }

        public void RemoveMoney(int removeMoney)
        {
            currentMoney -= removeMoney;
            EventManager.OnMoneyUpdate(currentMoney);
        }
    }
}