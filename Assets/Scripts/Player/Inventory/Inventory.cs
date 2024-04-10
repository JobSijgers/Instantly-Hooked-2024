using System;
using System.Collections.Generic;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        
        [SerializeField] private List<FishData> caughtFish;


        private void Awake()
        {
            instance = this;
        }

        public void AddFish(FishData fishToAdd)
        {
            if(fishToAdd == null)
                return;
            caughtFish.Add(fishToAdd);
        }

        public void RemoveFish(FishData fishToRemove)
        {
            if(fishToRemove == null)
                return;
            caughtFish.Remove(fishToRemove);
        }

        public void ClearFish()
        {
            caughtFish.Clear();
        }

        public FishData[] GetFishInInventory()
        {
            return caughtFish.ToArray();
        }
    }
}