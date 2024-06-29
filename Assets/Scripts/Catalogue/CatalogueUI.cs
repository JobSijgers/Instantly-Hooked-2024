using System;
using System.Text;
using Events;
using Fish;
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
                CatalogueItem item = CatalogueTracker.instance.GetCatalogueItem(i);
                FishData fish = item?.GetFish();
                if (itemsInPage[i % itemsPerPage] == null)
                    continue;
                if (item == null || item.GetAmount() <= 0)
                {
                    itemsInPage[i % itemsPerPage].DisableHolder();
                    continue;
                }

                string fishName = $"#{i + 1}  {fish.fishName}";

                itemsInPage[i % itemsPerPage].EnableHolder();
                // Initialize the corresponding UI element with the item's details
                itemsInPage[i % itemsPerPage].Initialize(fishName, fish.fishDescription, item.GetAmount(), fish.habitat,
                    raritySprites[(int)fish.fishRarity], fish.fishVisual);
            }
        }

        public override void Show()
        {
            base.Show();
            currentPage = 0;
            LoadPage(currentPage);
            CheckPreviousPageButton();
            CheckNextPageButton();
            SetTotalCollectFishText();
        }

        /// <summary>
        /// Checks if the previous page button should be active.
        /// </summary>
        private void CheckPreviousPageButton()
        {
            previousPageButton.SetActive(currentPage > 0);
        }

        /// <summary>
        /// Checks if the next page button should be active.
        /// </summary>
        private void CheckNextPageButton()
        {
            bool isActive = (currentPage + 1) * itemsPerPage < CatalogueTracker.instance.GetCatalogueItemsLength();
            nextPageButton.SetActive(isActive);
        }

        public void LoadNextPage()
        {
            currentPage++;
            LoadPage(currentPage);
            CheckNextPageButton();
            CheckPreviousPageButton();
        }

        public void LoadPreviousPage()
        {
            currentPage--;
            LoadPage(currentPage);
            CheckNextPageButton();
            CheckPreviousPageButton();
        }

        private void SetTotalCollectFishText()
        {
            StringBuilder sb = new();
            sb.Append(CatalogueTracker.instance.GetCatalogueProgress());
            sb.Append(" / ");
            sb.Append(CatalogueTracker.instance.GetCatalogueItemsLength());
            totalFishCollectedText.text = sb.ToString();
        }

        public void ChangeBookPage(View newView)
        {
            ViewManager.ShowView(newView, false);
        }
    }
}