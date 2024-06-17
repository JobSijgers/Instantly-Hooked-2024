using System;
using System.Collections;
using Events;
using UnityEngine;
using Views;

namespace Shore
{
    public class ShoreManager : MonoBehaviour
    {
        [SerializeField] private GameObject shoreCamera;
        [SerializeField] private float transitionTime = 2f;

        private void Start()
        {
            EventManager.DockSuccess += TransitionToShore;
            EventManager.LeftShore += TransitionToSea;
        }

        private void TransitionToShore()
        {
            shoreCamera.SetActive(true);
            StartCoroutine(TransitionViewToShore());
        }

        private void TransitionToSea()
        {
            shoreCamera.SetActive(false);
            StartCoroutine(TransitionViewToSea());
        }

        private IEnumerator TransitionViewToShore()
        {
            ViewManager.HideActiveView();
            yield return new WaitForSeconds(transitionTime);
            ViewManager.ShowView<ShoreUI>();
        }
        
        private IEnumerator TransitionViewToSea()
        {
            ViewManager.HideActiveView();
            yield return new WaitForSeconds(transitionTime);
            ViewManager.ShowView<GameView>();
        }
    }
}