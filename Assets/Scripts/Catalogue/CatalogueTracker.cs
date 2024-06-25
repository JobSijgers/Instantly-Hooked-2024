using System.Collections.Generic;
using System.Linq;
using Enums;
using Events;
using Fish;
using UnityEngine;
using UnityEngine.Events;

namespace Catalogue
{
    public class CatalogueTracker : MonoBehaviour
    {
        public static CatalogueTracker instance;
        [SerializeField] private CatalogueItem[] catalogueItems;
        private readonly Dictionary<FishData, CatalogueItem> catalogue = new();
        
        private int totalCollectedFish;
        public UnityAction<int> CatalogueUpdated;
        private void OnCatalogueUpdate() => CatalogueUpdated?.Invoke(totalCollectedFish);


        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            foreach (CatalogueItem catalogueItem in catalogueItems)
            {
                catalogue.Add(catalogueItem.GetFish(), catalogueItem);
            }
            EventManager.FishCaught += AddFishToCatalogue;
        }

        private void OnDestroy()
        {
            EventManager.FishCaught -= AddFishToCatalogue;
        }

        private void AddFishToCatalogue(FishData fish, FishSize size)
        {
            if (!catalogue.TryGetValue(fish, out CatalogueItem item)) 
                return;
            item.AddFish();
            totalCollectedFish++;
            OnCatalogueUpdate();
        }

        /// <summary>
        /// returns a CatalogueItem at a given index.
        /// </summary>
        public CatalogueItem GetCatalogueItem(int index)
        {
            return catalogueItems[index];
        }

        public int GetCatalogueItemsLength()
        {
            return catalogue.Count;
        }

        public void GetCurrentCatalogueNotes(out int totalfish, out int[] amountcollectedPF)
        {
            amountcollectedPF = new int[GetCatalogueItemsLength() - 1];
            totalfish = totalCollectedFish;
            for (int i = 0; i < GetCatalogueItemsLength() - 1; i++)
            {
                amountcollectedPF[i] = catalogueItems[i].GetAmount();
            }
        }


        public void SetCatalogueNotes(int totalfish, int[] amountcollectedPF)
        {
            totalCollectedFish = totalfish;
            for (int i = 0; i < GetCatalogueItemsLength() - 1; i++)
            {
                catalogueItems[i].SetAmount(amountcollectedPF[i]);
            }
        }

        public int GetCatalogueProgress()
        {
            int progress = 0;
            foreach (CatalogueItem fish in catalogueItems)
            {
                if (fish.GetAmount() > 0)
                {
                    progress++;
                }
            }

            return progress;
        }
    }
}