using System;
using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Shore
{
    public class ShoreManager : MonoBehaviour
    {
        [SerializeField] private float transitionDuration;
        [SerializeField] private AnimationCurve transitionCurve;
        [SerializeField] private Image fadeScreen;
        [SerializeField] private GameObject shore;
        [SerializeField] private GameObject sea;
        private void Start()
        {
            EventManager.Dock += TransitionToShore;
        }

        private void TransitionToShore()
        {
            StartCoroutine(TransitionToShoreRoutine());
        }

        public void LeaveShore()
        {
            StartCoroutine(TransitionToBoatRoutine());
        }

        private IEnumerator TransitionToShoreRoutine()
        {
            yield return FadeOutRoutine();
            EventManager.OnArrivedAtShore();
            shore.SetActive(true);
            sea.SetActive(false);
            yield return FadeInRoutine();
        }
        
        private IEnumerator TransitionToBoatRoutine()
        {
            yield return FadeOutRoutine();
            EventManager.OnLeftShore();
            shore.SetActive(false);
            sea.SetActive(true);
            yield return FadeInRoutine();
        }

        private IEnumerator FadeOutRoutine()
        {
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / (transitionDuration / 2f);
                var curveValue = transitionCurve.Evaluate(t);
                fadeScreen.color = Color.Lerp(Color.clear, Color.black, curveValue);
                yield return null;
            }
        }

        private IEnumerator FadeInRoutine()
        {
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / (transitionDuration / 2f);
                var curveValue = transitionCurve.Evaluate(t);
                fadeScreen.color = Color.Lerp(Color.black, Color.clear, curveValue);
                yield return null;
            }
        }
    }
}