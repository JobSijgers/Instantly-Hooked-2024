using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Catalogue
{
    public class CatalogueUIItem : MonoBehaviour
    {
        [SerializeField] private GameObject holder;
        [SerializeField] private TMP_Text fishNameText;
        [SerializeField] private TMP_Text fishDescriptionText;
        [SerializeField] private TMP_Text fishAmountText;
        [SerializeField] private TMP_Text fishHabitatText;
        [SerializeField] private Image fishRarityImage;
        [SerializeField] private Image fishImage;

        public void Initialize(string fishName, string fishDescription, int fishAmount, string fishHabitat,
            Sprite fishRaritySprite, Sprite fishSprite)
        {
            fishNameText.text = fishName;
            fishDescriptionText.text = fishDescription;
            fishAmountText.text = $"{fishAmount} Caught";
            fishHabitatText.text = fishHabitat;
            fishRarityImage.sprite = fishRaritySprite;
            fishImage.sprite = fishSprite;
        }

        public void DisableHolder()
        {
            holder.gameObject.SetActive(false);
        }
        public void EnableHolder()
        {
            holder.gameObject.SetActive(true);
        }
    }
}