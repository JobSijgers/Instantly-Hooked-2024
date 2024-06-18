using System;
using Events;
using PauseMenu;
using TMPro;
using UnityEngine;
using Views;

namespace Catalogue
{
    public class CatalogueUI : View
    {
        [SerializeField] private CatalogueUIItem[] itemsInPage;
        [SerializeField] private Sprite[] raritySprites;

        [SerializeField] private TMP_Text totalFishCollectedText;
        [SerializeField] private GameObject nextPageButton;
        [SerializeField] private GameObject previousPageButton;

        private int itemsPerPage;
        private int currentPage;

        public override void Initialize()
        {
            base.Initialize();
            itemsPerPage = itemsInPage.Length;
        }

        /// <summary>
        /// Loads a page of the catalogue with the given index.
        /// </summary>
        private void LoadPage(int index)
        {
            // Loop through the items in the page based on the given index and the number of items per page
            for (int i = index * itemsPerPage; i < index * itemsPerPage + itemsPerPage; i++)
            {
                CatalogueItem item = CatalogueTracker.Instance.GetCatalogueItem(i);
                if (itemsInPage[i % itemsPerPage] == null)
                    continue;
                if (item == null || item.GetAmount() <= 0)
                {
                    itemsInPage[i % itemsPerPage].DisableHolder();
                    continue;
                }

                string fishName = $"#{i + 1}  {item.GetFish().fishName}";
            
                itemsInPage[i % itemsPerPage].EnableHolder();
                // Initialize the corresponding UI element with the item's details
                itemsInPage[i % itemsPerPage].Initialize(fishName, item.GetFish().fishDescription,
                    item.GetAmount(), item.GetFish().habitat, raritySprites[(int)item.GetFish().fishRarity],
                    item.GetFish().fishVisual);
            }
        }

        public override void Show()
        {
            base.Show();
            currentPage = 0;
            LoadPage(currentPage);
            CheckPreviousPageButton();
            CheckNextPageButton();
            totalFishCollectedText.text = CatalogueTracker.Instance.GetTotalFishCollected().ToString();
        }
        /// <summary>
        /// Checks if the previous page button should be active.
        /// </summary>
        private void CheckPreviousPageButton()
        {
            if (currentPage <= 0)
            {
                previousPageButton.SetActive(false);
            }
        }

        /// <summary>
        /// Checks if the next page button should be active.
        /// </summary>
        private void CheckNextPageButton()
        {
            if ((currentPage + 1) * itemsPerPage >= CatalogueTracker.Instance.GetCatalogueItemsLength())
            {
                nextPageButton.SetActive(false);
            }
        }
        
        public void LoadNextPage()
        {
            currentPage++;
            LoadPage(currentPage);

            if ((currentPage + 1) * itemsPerPage >= CatalogueTracker.Instance.GetCatalogueItemsLength())
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

        public void ChangeBookPage(View newView)
        {
            ViewManager.ShowView(newView, false);
        }
    }
}