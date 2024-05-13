using System;
using Events;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tutorial
{
    public class NoInputVisual : MonoBehaviour
    {
        [SerializeField] private Image visual;
        [SerializeField] private float display;
        private float t;
        private float depth;

        private void Start()
        {
            EventManager.DepthUpdate += OnDepthUpdate;
        }

        private void OnDestroy()
        {
            EventManager.DepthUpdate -= OnDepthUpdate;
        }


        private void Update()
        {
            if (depth <= 0 )
            {
                visual.enabled = false;
                return;
            }
            
            if (!Input.anyKey)
            {
                float a = t + Time.deltaTime;
                t = Mathf.Clamp(a, 0, display);
            }
            else
            {
                t = 0;
            }

            visual.enabled = t >= display;
        }
        
        private void OnDepthUpdate(float depth)
        {
            this.depth = depth;
        }
        
    }
}