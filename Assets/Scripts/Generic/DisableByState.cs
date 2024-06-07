using Events;
using PauseMenu;
using UnityEngine;

namespace Generic
{
    public class DisableByState : MonoBehaviour
    {
        [SerializeField] private PauseState activeState;
        
        private void Start()
        {
            EventManager.PauseStateChange += ToggleActive;
        }
        
        private void OnDestroy()
        {
            EventManager.PauseStateChange -= ToggleActive;
        }

        private void ToggleActive(PauseState newState)
        {
            gameObject.SetActive(newState == activeState);
        }
    }
}