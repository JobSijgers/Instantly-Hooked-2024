using System.Collections;
using System.Drawing;
using Enums;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.ShopScripts
{
    public class ShopItem : InventoryItem
    {
        public delegate void FSelectedAmountChanged(ShopItem item, int change, int total);

        public event FSelectedAmountChanged OnSelectedAmountChanged;

        private int _currentSelected = 0;

        [SerializeField] private TMP_InputField inputField;
        
        
        public void Initialize(FishData fishData, FishSize fishSize, int amountInStack)
        {
            base.Initialize(fishData, fishSize);
            stackSize = amountInStack;
            UpdateStackUI();
        }

        public void PlusButtonPressed()
        {
            if (_currentSelected + 1> stackSize)
                return;
            if (_currentSelected + 1 > fishData.maxStackAmount)
                return;
            _currentSelected += 1;
            inputField.text = _currentSelected.ToString();
            OnSelectedAmountChanged?.Invoke(this, 1, _currentSelected);
        }

        public void MinusButtonPressed()
        {
            if (_currentSelected - 1 < 0)
                return;
            _currentSelected -= 1;
            inputField.text = _currentSelected.ToString();
            OnSelectedAmountChanged?.Invoke(this, -1, _currentSelected);
        }

        public void OnInputFieldChanged()
        {
            var newInt = int.Parse(inputField.text);
            if (newInt >= stackSize)
            {
                OnSelectedAmountChanged?.Invoke(this, stackSize - _currentSelected, stackSize);
                _currentSelected = stackSize;
                inputField.text = stackSize.ToString();
            }
            else if (newInt >= fishData.maxStackAmount)
            {
                OnSelectedAmountChanged?.Invoke(this, fishData.maxStackAmount - _currentSelected, fishData.maxStackAmount);
                _currentSelected = fishData.maxStackAmount;
                inputField.text = fishData.maxStackAmount.ToString();
            }
            else if (newInt < 0)
            {
                OnSelectedAmountChanged?.Invoke(this, 0 - _currentSelected, 0);
                _currentSelected = 0;
                inputField.text = 0.ToString();
            }
            else
            {
                OnSelectedAmountChanged?.Invoke(this, newInt - _currentSelected, newInt);
                _currentSelected = newInt;
            }
        }
    }
}