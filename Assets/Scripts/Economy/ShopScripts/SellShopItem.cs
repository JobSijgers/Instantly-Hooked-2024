using System;
using Enums;
using Fish;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace Economy.ShopScripts
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

        public void Initialize(FishData fishData, FishSize fishSize, int amountInStack, Color backgroundColor)
        {
            base.Initialize(fishData, fishSize, backgroundColor);
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

        public void OnInputFieldChanged()
        {
            int newInt = int.Parse(inputField.text);
            if (newInt == currentSelectedAmount)
                return;

            if (newInt >= stackSize)
            {
                OnSelectedAmountChanged?.Invoke(this, stackSize - currentSelectedAmount);
                currentSelectedAmount = stackSize;
                inputField.text = stackSize.ToString();
            }
            else if (newInt >= fishData.maxStackAmount)
            {
                OnSelectedAmountChanged?.Invoke(this, fishData.maxStackAmount - currentSelectedAmount);
                currentSelectedAmount = fishData.maxStackAmount;
                inputField.text = fishData.maxStackAmount.ToString();
            }
            else if (newInt <= 0)
            {
                OnSelectedAmountChanged?.Invoke(this, 0 - currentSelectedAmount);
                currentSelectedAmount = 0;
                inputField.text = 0.ToString();
            }
            else
            {
                OnSelectedAmountChanged?.Invoke(this, newInt - currentSelectedAmount);
                currentSelectedAmount = newInt;
            }
        }

        public void SetInputField(int newSelected)
        {
            currentSelectedAmount = newSelected;
            inputField.text = currentSelectedAmount.ToString();
        }
    }
}