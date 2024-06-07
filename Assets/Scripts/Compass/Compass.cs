using System;
using Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Compass
{
    public class CompassUI : MonoBehaviour
    {
        [Header("Transforms")] [SerializeField]
        private Transform shore;

        [SerializeField] private Transform player;
        [SerializeField] private Transform endOfMap;

        [Header("Ui Elements")] [SerializeField]
        private RectTransform background;

        [SerializeField] private RectTransform playerIcon;
        [SerializeField] private RectTransform stormIcon;
        private Transform storm;

        private void OnEnable()
        {
            EventManager.StormSpawned += OnStormSpawned;
            EventManager.NewDay += OnNewDay;
        }

        private void OnDisable()
        {
            EventManager.StormSpawned -= OnStormSpawned;
            EventManager.NewDay -= OnNewDay;
        }

        private void Update()
        {
            RenderUI();
        }

        private void OnNewDay(int dayCount)
        {
            storm = null;
            stormIcon.gameObject.SetActive(false);
        }

        private void OnStormSpawned(Transform storm)
        {
            this.storm = storm;
            stormIcon.gameObject.SetActive(true);
        }


        private float GetPositionOnCompass(Transform target)
        {
            float totalDistance = Vector2.Distance(shore.position, endOfMap.position);
            float targetDistance = Vector2.Distance(shore.position, target.position);
            float normalizedDistance = targetDistance / totalDistance;

            float leftBound = 0;
            float rightBound = background.rect.width;

            return Mathf.Lerp(leftBound, rightBound, normalizedDistance) - background.rect.width / 2;
        }

        private void RenderUI()
        {
            playerIcon.localPosition =
                new Vector2(GetPositionOnCompass(player), 0);
            if (storm == null) return;
            stormIcon.localPosition =
                new Vector2(GetPositionOnCompass(storm), 0);
        }
    }
}