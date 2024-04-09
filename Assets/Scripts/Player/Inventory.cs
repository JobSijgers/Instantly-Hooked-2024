using System.Collections.Generic;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private List<FishData> _caughtFish;


        public void AddFish(FishData fishToAdd)
        {
            if(fishToAdd == null)
                return;
            _caughtFish.Add(fishToAdd);
        }
    }
}