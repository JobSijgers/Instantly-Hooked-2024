using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fish
{
    public class FishUIHighlight : MonoBehaviour
    {
        [SerializeField] private GameObject holder;
        [SerializeField] private TMP_Text fishNameText;
        [SerializeField] private TMP_Text fishDescriptionText;
        [SerializeField] private TMP_Text fishAmountText;
        [SerializeField] private TMP_Text fishHabitatText;
        [SerializeField] private Image fishRarityImage;
        [SerializeField] private Image fishImage;

        public void Initialize(FishData data, int amount, Sprite fishRaritySprite)
        {
            fishNameText.text = data.fishName;
            fishDescriptionText.text = data.fishDescription;
            fishAmountText.text = $"{amount} Caught";
            fishHabitatText.text = data.habitat;
            fishImage.sprite = data.fishVisual;
            fishRarityImage.sprite = fishRaritySprite;
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