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
        public TutorialData data;

        public override void Initialize()
        {
            base.Initialize();
            nextButton.onClick.AddListener(NextPage);
            Init(data);
        }

        public void Init(TutorialData newData)
        {
            currentPage = 0;
            text.text = newData.pages[0].description;
            image.sprite = newData.pages[0].image;
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
            ViewManager.ShowLastView();
        }
    }
}