using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class NoInputVisual : MonoBehaviour
    {
        [SerializeField] private Image visual;
        [SerializeField] private float display;
        private float t;
        private float depth;
        private bool onShore;

        private void Start()
        {
            EventManager.DepthUpdate += OnDepthUpdate;
            EventManager.ArrivedAtShore += OnShore;
            EventManager.LeftShore += LeftShore;
        }

        private void OnDestroy()
        {
            EventManager.DepthUpdate -= OnDepthUpdate;
            EventManager.ArrivedAtShore -= OnShore;
            EventManager.LeftShore -= LeftShore;
        }


        private void Update()
        {
            if (onShore)
                return;
            
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

        private void OnShore()
        {
            onShore = true;
        }

        private void LeftShore()
        {
            onShore = false;
        }
        
    }
}