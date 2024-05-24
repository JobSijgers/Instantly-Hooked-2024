using System;
using Fish;
using UnityEngine;

namespace Catalogue
{
    [Serializable]
    public class CatalogueItem
    {
        [SerializeField] private FishData fishToTrack;
        private int _amountCaught;
            
        public FishData GetFish()
        {
            return fishToTrack;
        }
            
        public void AddFish()
        {
            _amountCaught++;
        }
        
        public int GetAmount()
        {
            return _amountCaught;
        }

        public void SetAmount(int fishamount)
        {
            _amountCaught = fishamount;
        }
    }
}