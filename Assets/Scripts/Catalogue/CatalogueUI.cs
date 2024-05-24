using System;
using Events;
using PauseMenu;
using TMPro;
using UnityEngine;

namespace Catalogue
{
    [RequireComponent(typeof(CatalogueTracker))]
    public class CatalogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject catalogueUIParent;
        [SerializeField] private CatalogueUIItem[] itemsInPage;
        [SerializeField] private Sprite[] raritySprites;

        [SerializeField] private TMP_Text totalFishCollectedText;
        [SerializeField] private GameObject nextPageButton;
        [SerializeField] private GameObject previousPageButton;

        private CatalogueTracker tracker;
        private int itemsPerPage;
        private int currentPage;

        private void Start()
        {
            EventManager.PauseStateChange += OnPauseStateChange;
            tracker = GetComponent<CatalogueTracker>();
            itemsPerPage = itemsInPage.Length;
            LoadPage(0);
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPauseStateChange;
        }

        private void LoadPage(int index)
        {
            for (int i = index * itemsPerPage; i < index * itemsPerPage + itemsPerPage; i++)
            {
                CatalogueItem item = tracker.GetCatalogueItem(i);
                if (itemsInPage[i % itemsPerPage] == null)
                    continue;
                if (item == null || item.GetAmount() <= 0)
                {
                    itemsInPage[i % itemsPerPage].DisableHolder();
                    continue;
                }

                string fishName = $"#{i + 1}  {item.GetFish().fishName}";
            
                itemsInPage[i % itemsPerPage].EnableHolder();
                itemsInPage[i % itemsPerPage].Initialize(fishName, item.GetFish().fishDescription,
                    item.GetAmount(), item.GetFish().habitat, raritySprites[(int)item.GetFish().fishRarity],
                    item.GetFish().fishVisual);
            }
        }

        public void OpenCatalogue(bool suppressEvent)
        {
            catalogueUIParent.SetActive(true);
            currentPage = 0;
            LoadPage(currentPage);
            CheckPreviousPage();
            CheckNextPage();

            totalFishCollectedText.text = tracker.GetTotalFishCollected().ToString();
            PauseManager.SetState(PauseState.InCatalogue, suppressEvent);
        }

        public void CloseCatalogue(bool suppressEvent)
        {
            if (!catalogueUIParent.activeSelf)
                return;
            catalogueUIParent.SetActive(false);
            PauseManager.SetState(PauseState.Playing, suppressEvent);
        }

        private void CheckPreviousPage()
        {
            if (currentPage <= 0)
            {
                previousPageButton.SetActive(false);
            }
        }

        private void CheckNextPage()
        {
            if ((currentPage + 1) * itemsPerPage >= tracker.GetCatalogueItemsLength())
            {
                nextPageButton.SetActive(false);
            }
        }

        public void LoadNextPage()
        {
            currentPage++;
            LoadPage(currentPage);

            if ((currentPage + 1) * itemsPerPage >= tracker.GetCatalogueItemsLength())
            {
                nextPageButton.SetActive(false);
            }

            previousPageButton.SetActive(true);
        }

        public void LoadPreviousPage()
        {
            currentPage--;
            LoadPage(currentPage);
            if (currentPage <= 0)
            {
                previousPageButton.SetActive(false);
            }

            nextPageButton.SetActive(true);
        }

        private void OnPauseStateChange(PauseState newState)
        {
            if (newState == PauseState.InCatalogue)
            {
                OpenCatalogue(true);
            }
            else
            {
                CloseCatalogue(true);
            }
        }
    }
}