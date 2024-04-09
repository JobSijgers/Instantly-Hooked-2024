using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.ShopScripts
{
    public class ShopItem : MonoBehaviour
    {
        public delegate void FFishButtonPressed(ShopItem shopitem);

        public event FFishButtonPressed OnFishButtonPressed;
        
        public FishData fishData;
        [SerializeField] private TMP_Text sellValue;
        [SerializeField] private Image fishImage;
        [SerializeField] private Button fishButton;
        
        public void Initialize(FishData fishData)
        {
            this.fishData = fishData;
            fishImage.sprite = fishData.fishVisual;
            sellValue.text = $"${fishData.minimumSellValue} - {fishData.maximumSellValue}";
            fishButton.onClick.AddListener(ButtonPressed);
        }

        private void ButtonPressed()
        {
            OnFishButtonPressed?.Invoke(this);
        }
    }
}