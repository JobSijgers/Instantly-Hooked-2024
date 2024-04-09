using System;
using System.Collections.Generic;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Economy
{
    public class Shop : MonoBehaviour, IShop
    {
        // TEST
        public List<FishData> tempfishtosell;
        // TEST
        
        public static Shop instance;

        public delegate void FSuccessfulSell(int sellAmount);

        public event FSuccessfulSell OnSuccessfulSell;
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SellFish(tempfishtosell);
            }
        }

        public int SellFish(List<FishData> fishToSell)
        {
            var totalSellAmount = 0;
            if (fishToSell == null) return 0;
            foreach (var fish in fishToSell)
            {
                totalSellAmount += Random.Range(fish.minimumSellValue, fish.maximumSellValue + 1);
            }
            OnSuccessfulSell?.Invoke(totalSellAmount);
            return totalSellAmount;
        }
    }
}