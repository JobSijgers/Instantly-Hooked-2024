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
            EventManager.ArrivedAtShore += DisableCompass;
            EventManager.LeftShore += EnableCompass;
        }

        private void OnDisable()
        {
            EventManager.StormSpawned -= OnStormSpawned;
            EventManager.NewDay -= OnNewDay;
            EventManager.ArrivedAtShore -= DisableCompass;
            EventManager.LeftShore -= EnableCompass;
        }

        private void Update()
        {
            if (background.gameObject.activeSelf == false)
                return;
            RenderUI();
        }

        private void DisableCompass()
        {
            background.gameObject.SetActive(false);
        }

        private void EnableCompass()
        {
            background.gameObject.SetActive(true);
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
            Vector2 targetPos = new Vector2(target.position.x, endOfMap.position.y);
            float totalDistance = Vector2.Distance(shore.position, endOfMap.position);
            float targetDistance = Vector2.Distance(shore.position, targetPos);
            float normalizedDistance = targetDistance / totalDistance;

            float leftBound = 0;
            float rightBound = background.rect.width;

            return Mathf.Lerp(leftBound, rightBound, normalizedDistance) - background.rect.width / 2;
        }

        private void RenderUI()
        {
            playerIcon.localPosition =
                new Vector2(GetPositionOnCompass(player), -background.rect.height / 2);
            if (storm == null) return;

            float position = GetPositionOnCompass(storm);
            if (position >= background.rect.width / 2 - stormIcon.rect.width)
            {
                stormIcon.localPosition =new Vector2(background.rect.width / 2 - stormIcon.rect.width, -background.rect.height / 2);
                return;
            }
            stormIcon.localPosition =
                new Vector2(GetPositionOnCompass(storm), -background.rect.height / 2);
        }
    }
}