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
            text.text = newData.pages[0].description;
            image.sprite = newData.pages[0].image;
            data = newData;
            viewOnClose = newViewOnClose;
        }

        private void NextPage()
        {
            currentPage++;
            if (currentPage < data.pages.Length)
            {
                text.text = data.pages[currentPage].description;
                image.sprite = data.pages[currentPage].image;
                return;
            }
            ViewManager.ShowView(viewOnClose, false);
        }
    }
}