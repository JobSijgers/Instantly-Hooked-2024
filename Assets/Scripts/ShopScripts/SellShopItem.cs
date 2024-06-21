using Enums;
using Fish;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShopScripts
{
    public class SellShopItem : InventoryItem, IPointerClickHandler
    {
        public delegate void FSelectedAmountChanged(SellShopItem item, int change);

        public event FSelectedAmountChanged OnSelectedAmountChanged;

        private int currentSelectedAmount = 0;

        [SerializeField] private TMP_InputField inputField;

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

        public void Initialize(FishData newFishData, FishSize fishSize, int amountInStack, Color backgroundColor)
        {
            base.Initialize(newFishData, fishSize, backgroundColor);
            stackSize = amountInStack;
            UpdateStackUI();
        }

        public void PlusButtonPressed()
        {
            if (currentSelectedAmount + 1 > stackSize)
                return;
            if (currentSelectedAmount + 1 > fishData.maxStackAmount)
                return;
            currentSelectedAmount += 1;
            inputField.text = currentSelectedAmount.ToString();
            OnSelectedAmountChanged?.Invoke(this, 1);
        }

        public void MinusButtonPressed()
        {
            if (currentSelectedAmount - 1 < 0)
                return;
            currentSelectedAmount -= 1;
            inputField.text = currentSelectedAmount.ToString();
            OnSelectedAmountChanged?.Invoke(this, -1);
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
            else if (newInt >= fishData.maxStackAmount)
            {
                currentSelectedAmount = fishData.maxStackAmount;
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
            OnSelectedAmountChanged?.Invoke(this, currentSelectedAmount - oldSelectedAmount);
        }

        public void SetInputField(int newSelected)
        {
            currentSelectedAmount = newSelected;
            inputField.text = currentSelectedAmount.ToString();
        }
    }
}