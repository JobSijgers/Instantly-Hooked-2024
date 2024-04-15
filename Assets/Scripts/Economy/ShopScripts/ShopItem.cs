using Enums;
using Fish;
using Player.Inventory;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Economy.ShopScripts
{
    public class ShopItem : InventoryItem
    {
        public delegate void FSelectedAmountChanged(ShopItem item, int change);

        public event FSelectedAmountChanged OnSelectedAmountChanged;

        private int _currentSelected = 0;

        [SerializeField] private TMP_InputField inputField;
        
        
        public void Initialize(FishData fishData, FishSize fishSize, int amountInStack, Color backgroundColor)
        {
            base.Initialize(fishData, fishSize, backgroundColor);
            StackSize = amountInStack;
            UpdateStackUI();
        }

        public void PlusButtonPressed()
        {
            if (_currentSelected + 1> StackSize)
                return;
            if (_currentSelected + 1 > FishData.maxStackAmount)
                return;
            _currentSelected += 1;
            inputField.text = _currentSelected.ToString();
            OnSelectedAmountChanged?.Invoke(this, 1);
        }

        public void MinusButtonPressed()
        {
            if (_currentSelected - 1 < 0)
                return;
            _currentSelected -= 1;
            inputField.text = _currentSelected.ToString();
            OnSelectedAmountChanged?.Invoke(this, -1);
        }

        public void OnInputFieldChanged()
        {
            var newInt = int.Parse(inputField.text);
            if (newInt >= StackSize)
            {
                OnSelectedAmountChanged?.Invoke(this, StackSize - _currentSelected);
                _currentSelected = StackSize;
                inputField.text = StackSize.ToString();
            }
            else if (newInt >= FishData.maxStackAmount)
            {
                OnSelectedAmountChanged?.Invoke(this, FishData.maxStackAmount - _currentSelected);
                _currentSelected = FishData.maxStackAmount;
                inputField.text = FishData.maxStackAmount.ToString();
            }
            else if (newInt < 0)
            {
                OnSelectedAmountChanged?.Invoke(this, 0 - _currentSelected);
                _currentSelected = 0;
                inputField.text = 0.ToString();
            }
            else
            {
                OnSelectedAmountChanged?.Invoke(this, newInt - _currentSelected);
                _currentSelected = newInt;
            }
        }

        public void SetInputField(int newSelected)
        {
            _currentSelected = newSelected;
            inputField.text = _currentSelected.ToString();
        }
    }
}