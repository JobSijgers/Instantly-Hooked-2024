using System;
using Fish;
using UnityEngine;

namespace Catalogue
{
    [Serializable]
    public class CatalogueItem
    {
        [SerializeField] private FishData fishToTrack;
        private int amountCaught;
            
        public FishData GetFish()
        {
            return fishToTrack;
        }
            
        public void AddFish()
        {
            amountCaught++;
        }
        
        public int GetAmount()
        {
            return amountCaught;
        }

        public void SetAmount(int fishamount)
        {
            amountCaught = fishamount;
        }
    }
}