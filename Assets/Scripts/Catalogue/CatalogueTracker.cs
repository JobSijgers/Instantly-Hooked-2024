using Enums;
using Events;
using Fish;
using UnityEngine;

namespace Catalogue
{
    public class CatalogueTracker : MonoBehaviour
    {
        public static CatalogueTracker Instance;
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

        public void GetCurrentCatalogueNotes(out int totalfish, out int[] amountcollectedPF)
        {
            amountcollectedPF = new int[GetCatalogueItemsLength() -1];
            totalfish = totalCollectedFish;
            for (int i = 0; i < GetCatalogueItemsLength() -1; i++)
            {
                amountcollectedPF[i] = catalogueItems[i].GetAmount();
            }
        }

        public void SetCatalogueNotes(int totalfish,int[] amountcollectedPF)
        {
            totalCollectedFish = totalfish;
            for (int i = 0; i < GetCatalogueItemsLength() -1; i++)
            {
                catalogueItems[i].SetAmount(amountcollectedPF[i]);
            }
        }
    }
}