using Events;
using PauseMenu;
using TMPro;
using UnityEngine;

namespace Depth
{
    public class DepthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text depthText;
        [SerializeField] private TMP_Text pressureText;

        private void Start()
        {
            EventManager.PauseStateChange += OnPause;
        }

        private void OnDestroy()
        {
            EventManager.PauseStateChange -= OnPause;
        }

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

        private void OnPause(PauseState state)
        {
            switch (state)
            {
                case PauseState.Playing:
                    gameObject.SetActive(true);
                    break;
                case PauseState.InPauseMenu:
                    gameObject.SetActive(false);
                    break;
                case PauseState.InInventory:
                    gameObject.SetActive(false);
                    break;
                case PauseState.InCatalogue:
                    gameObject.SetActive(false);
                    break;
                case PauseState.InQuests:
                    gameObject.SetActive(false);
                    break;
            }   
        }
    }
}