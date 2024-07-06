using System;
using Enums;
using Fish;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] protected TMP_Text stackSizeText;
        [SerializeField] private TMP_Text fishSizeText;
        [SerializeField] protected Image fishImage;
        [SerializeField] private Image background;

        private Action<FishData> onHover;
        private FishData fishData;

        public InventoryItem(Action<FishData> onHover)
        {
            this.onHover = onHover;
        }

        public void Initialize(FishData newFishData, FishSize newFishSize, Color backgroundColor, int stackSize, Action<FishData> onHoverAction)
        {
            fishData = newFishData;
            fishImage.sprite = newFishData.fishVisual;
            background.color = backgroundColor;
            fishSizeText.text = newFishSize.ToString();
            stackSizeText.text = stackSize.ToString();
            onHover = onHoverAction;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            onHover?.Invoke(fishData);
        }
    }
}