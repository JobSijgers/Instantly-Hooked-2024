using Enums;
using Events;
using Fish;
using UnityEngine;

namespace Catalogue
{
    public class CatalogueTracker : MonoBehaviour
    {
        [SerializeField] private CatalogueItem[] catalogueItems;
        private int totalCollectedFish;
        private void Start()
        {
            EventManager.FishCaught += AddFishToCatalogue;
        } 
        
        private void OnDestroy()
        {
            EventManager.FishCaught -= AddFishToCatalogue;
        }
        
        private void AddFishToCatalogue(FishData fish, FishSize size)
        {
            if (catalogueItems == null || catalogueItems.Length == 0) return;
            
            foreach (CatalogueItem item in catalogueItems)
            {
                if (item.GetFish() != fish) continue;
                
                item.AddFish();
                totalCollectedFish++;
                return;
            }
        }
        
        public CatalogueItem GetCatalogueItem(int index)
        {
            if (index < 0 || index >= catalogueItems.Length)
                return null;
            
            return catalogueItems[index];
        }
        
        public int GetCatalogueItemsLength()
        {
            return catalogueItems.Length;
        }

        public int GetTotalFishCollected()
        {
            return totalCollectedFish;
        }
    }
}