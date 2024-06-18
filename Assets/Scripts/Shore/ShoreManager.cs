using System;
using System.Collections;
using Cinemachine;
using Events;
using Unity.VisualScripting;
using UnityEngine;
using Views;

namespace Shore
{
    public class ShoreManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera shoreCamera;
        [SerializeField] private float transitionTime = 2f;
        private bool atShore;

        private void Start()
        {
            EventManager.DockSuccess += TransitionToShore;
            EventManager.LeftShore += TransitionToSea;
        }

        private void Update()
        {
            if (!atShore)
                return;
            if (!Input.GetKeyDown(KeyCode.E))
                return;
            if (ViewManager.GetActiveView() != typeof(ShoreUI))
                return;
            EventManager.OnLeftShore();
        }

        private void TransitionToShore()
        {
            StartCoroutine(TransitionViewToShore());
        }

        private void TransitionToSea()
        {
            StartCoroutine(TransitionViewToSea());
        }

        private IEnumerator TransitionViewToShore()
        {
            shoreCamera.Priority = 2;
            ViewManager.ClearHistory();
            ViewManager.HideActiveView();
            yield return new WaitForSeconds(transitionTime);
            ViewManager.ShowView<ShoreUI>();
            atShore = true;
        }

        private IEnumerator TransitionViewToSea()
        {
            shoreCamera.Priority = -2;
            atShore = false;
            ViewManager.ClearHistory();
            ViewManager.HideActiveView();
            yield return new WaitForSeconds(transitionTime);
            ViewManager.ShowView<GameView>();
            EventManager.OnLeftShoreSuccess();
        }
    }
}