using System;
using Enums;
using Fish;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShopScripts
{
    public class SellShopItem : MonoBehaviour, IPointerClickHandler
    {
        private Action<SellShopItem, int> onSelectedAmountChanged;
        public FishData FishData { get; private set; }
        public FishSize Size { get; private set; }

        [SerializeField] private TMP_Text stackSizeText;
        [SerializeField] private TMP_Text fishSizeText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Image fishImage;
        [SerializeField] private Image background;
        private int currentSelectedAmount = 0;
        private int stackSize;

        public void Initialize(FishData newFishData, FishSize newFishSize, Color backgroundColor, int newStacksize,
            Action<SellShopItem, int> amountChanged)
        {
            FishData = newFishData;
            Size = newFishSize;
            fishSizeText.text = newFishSize.ToString();
            fishImage.sprite = newFishData.fishVisual;
            background.color = backgroundColor;
            stackSize = newStacksize;
            stackSizeText.text = newStacksize.ToString();
            onSelectedAmountChanged = amountChanged;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    PlusButtonPressed();
                    break;
                case PointerEventData.InputButton.Right:
                    MinusButtonPressed();
                    break;
            }
        }

        public void PlusButtonPressed()
        {
            if (currentSelectedAmount + 1 > stackSize)
                return;
            if (currentSelectedAmount + 1 > FishData.maxStackAmount)
                return;
            currentSelectedAmount += 1;
            inputField.text = currentSelectedAmount.ToString();
            onSelectedAmountChanged?.Invoke(this, 1);
        }

        public void MinusButtonPressed()
        {
            if (currentSelectedAmount - 1 < 0)
                return;
            currentSelectedAmount -= 1;
            inputField.text = currentSelectedAmount.ToString();
            onSelectedAmountChanged?.Invoke(this, -1);
        }

        /// <summary>
        /// Handles the input field changed event.
        /// </summary>
        public void OnInputFieldChanged()
        {
            int newInt = int.Parse(inputField.text);
            if (newInt == currentSelectedAmount)
                return;

            int oldSelectedAmount = currentSelectedAmount;

            if (newInt >= stackSize)
            {
                currentSelectedAmount = stackSize;
            }
            else if (newInt >= FishData.maxStackAmount)
            {
                currentSelectedAmount = FishData.maxStackAmount;
            }
            else if (newInt <= 0)
            {
                currentSelectedAmount = 0;
            }
            else
            {
                currentSelectedAmount = newInt;
            }

            inputField.text = currentSelectedAmount.ToString();
            onSelectedAmountChanged?.Invoke(this, currentSelectedAmount - oldSelectedAmount);
        }

        public void SetInputField(int newSelected)
        {
            currentSelectedAmount = newSelected;
            inputField.text = currentSelectedAmount.ToString();
        }
        
        public int GetStackSize()
        {
            return stackSize;
        }
    }
}