using Events;
using TMPro;
using UnityEngine;
using Views;

namespace Depth
{
    public class DepthUI : ViewComponent
    {
        [SerializeField] private TMP_Text depthText;
        [SerializeField] private TMP_Text pressureText;

        private void OnEnable()
        {
            EventManager.DepthUpdate += UpdateDepthUI;
        }

        private void OnDisable()
        {
            EventManager.DepthUpdate -= UpdateDepthUI;
        }

        private void UpdateDepthUI(float depth)
        {
            depthText.text = $"{depth:F1} M";
            if (depth > 0)
                return;
            pressureText.text = $"Pressure: {Mathf.Clamp(Mathf.Abs(depth) / 10f, 0, Mathf.Infinity):F1} BAR";
        }
    }
}