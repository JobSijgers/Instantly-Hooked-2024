using Audio;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Tutorial
{
    public class TutorialPopup : View
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image image;
        [SerializeField] private Button nextButton;
        private int currentPage = 0;
        private TutorialData data;
        private View viewOnClose;

        public override void Initialize()
        {
            base.Initialize();
            nextButton.onClick.AddListener(NextPage);
        }

        public void Init(TutorialData newData, View newViewOnClose)
        {
            currentPage = 0;
            data = newData;
            viewOnClose = newViewOnClose;
            LoadPage(currentPage);
        }

        private void NextPage()
        {
            currentPage++;
            if (currentPage < data.pages.Length)
            {
                LoadPage(currentPage);
                return;
            }
            ViewManager.ShowView(viewOnClose, false);
            AudioManager.instance.PlaySound(data.pages[currentPage].sound);
        }
        
        private void LoadPage(int page)
        {
            text.text = data.pages[page].description;
            image.sprite = data.pages[page].image;
            image.gameObject.SetActive(data.pages[page].image != null);
            image.rectTransform.sizeDelta = data.pages[page].imageSize;
        }
    }
}