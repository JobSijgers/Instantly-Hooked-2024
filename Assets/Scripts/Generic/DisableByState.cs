using Events;
using PauseMenu;
using UnityEngine;
using Views;

namespace Generic
{
    public class DisableByState : MonoBehaviour
    {
        [SerializeField] private View activeState;
        
        private void Start()
        {
            ViewManager.instance.ViewShow += ToggleActive;
        }
        
        private void OnDestroy()
        {
            ViewManager.instance.ViewShow -= ToggleActive;
        }

        private void ToggleActive(View view)
        {
            gameObject.SetActive(view == activeState);
        }
    }
}