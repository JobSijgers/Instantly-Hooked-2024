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

        private int _currentSelectedAmount = 0;

        [SerializeField] private TMP_InputField inputField;
        private Button _itemButton;

        private void Start()
        {
            _itemButton = GetComponent<Button>();
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

        public void Initialize(FishData fishData, FishSize fishSize, int amountInStack, Color backgroundColor)
        {
            base.Initialize(fishData, fishSize, backgroundColor);
            stackSize = amountInStack;
            UpdateStackUI();
        }

        public void PlusButtonPressed()
        {
            if (_currentSelectedAmount + 1 > stackSize)
                return;
            if (_currentSelectedAmount + 1 > fishData.maxStackAmount)
                return;
            _currentSelectedAmount += 1;
            inputField.text = _currentSelectedAmount.ToString();
            OnSelectedAmountChanged?.Invoke(this, 1);
        }

        public void MinusButtonPressed()
        {
            if (_currentSelectedAmount - 1 < 0)
                return;
            _currentSelectedAmount -= 1;
            inputField.text = _currentSelectedAmount.ToString();
            OnSelectedAmountChanged?.Invoke(this, -1);
        }

        public void OnInputFieldChanged()
        {
            var newInt = int.Parse(inputField.text);
            if (newInt == _currentSelectedAmount)
                return;

            if (newInt >= stackSize)
            {
                OnSelectedAmountChanged?.Invoke(this, stackSize - _currentSelectedAmount);
                _currentSelectedAmount = stackSize;
                inputField.text = stackSize.ToString();
            }
            else if (newInt >= fishData.maxStackAmount)
            {
                OnSelectedAmountChanged?.Invoke(this, fishData.maxStackAmount - _currentSelectedAmount);
                _currentSelectedAmount = fishData.maxStackAmount;
                inputField.text = fishData.maxStackAmount.ToString();
            }
            else if (newInt <= 0)
            {
                _currentSelectedAmount = 0;
                inputField.text = 0.ToString();
                OnSelectedAmountChanged?.Invoke(this, 0 - _currentSelectedAmount);
            }
            else
            {
                OnSelectedAmountChanged?.Invoke(this, newInt - _currentSelectedAmount);
                _currentSelectedAmount = newInt;
            }
        }

        public void SetInputField(int newSelected)
        {
            _currentSelectedAmount = newSelected;
            inputField.text = _currentSelectedAmount.ToString();
        }
    }
}