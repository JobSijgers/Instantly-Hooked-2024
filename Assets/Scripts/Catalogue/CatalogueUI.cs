using UnityEngine;

namespace Catalogue
{
    [RequireComponent(typeof(CatalogueTracker))]
    public class CatalogueUI : MonoBehaviour
    {
        [SerializeField] private CatalogueUIItem[] itemsInPage;
        [SerializeField] private Sprite[] raritySprites;

        [SerializeField] private GameObject nextPageButton;
        [SerializeField] private GameObject previousPageButton;
        
        private CatalogueTracker _tracker;
        private int _itemsPerPage;
        private int _currentPage;

        private void Start()
        {
            _tracker = GetComponent<CatalogueTracker>();
            _itemsPerPage = itemsInPage.Length;
            LoadPage(0);
        }

        private void LoadPage(int index)
        {
            for (int i = index * _itemsPerPage; i < index * _itemsPerPage + _itemsPerPage; i++)
            {
                CatalogueItem item = _tracker.GetCatalogueItem(i);
                if (itemsInPage[i % _itemsPerPage] == null)
                    continue;
                if (item == null)
                {
                    itemsInPage[i % _itemsPerPage].gameObject.SetActive(false);
                    continue;
                }

                itemsInPage[i % _itemsPerPage].gameObject.SetActive(true);
                itemsInPage[i % _itemsPerPage].Initialize(item.GetFish().fishName, item.GetFish().fishDescription,
                    item.GetAmount(), item.GetFish().habitat, raritySprites[(int)item.GetFish().fishRarity],
                    item.GetFish().fishVisual);
            }
        }
        
        private void OpenCatalogue()
        {
            gameObject.SetActive(true);
            _currentPage = 0;
            LoadPage(_currentPage);
            CheckPreviousPage();
            CheckNextPage();
        }
        
        private void CheckPreviousPage()
        {
            if (_currentPage <= 0)
            {
                previousPageButton.SetActive(false);
            }
        }
        private void CheckNextPage()
        {
            if ((_currentPage + 1) * _itemsPerPage >= _tracker.GetCatalogueItemsLength())
            {
                nextPageButton.SetActive(false);
            }
        }
        public void LoadNextPage()
        {
            _currentPage++;
            LoadPage(_currentPage);

            if ((_currentPage + 1) * _itemsPerPage >= _tracker.GetCatalogueItemsLength())
            {
                nextPageButton.SetActive(false);
            }

            previousPageButton.SetActive(true);

        }

        public void LoadPreviousPage()
        {
            _currentPage--;
            LoadPage(_currentPage);
            if (_currentPage <= 0)
            {
                previousPageButton.SetActive(false);
            }
            nextPageButton.SetActive(true);
        }
    }
}