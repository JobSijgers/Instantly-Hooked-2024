using System;
using Events;
using UnityEngine;

namespace Depth
{
    public class DepthTracker : MonoBehaviour
    {
        [SerializeField] private float seaLevel;
        [SerializeField] private Transform hook;
        
        private void Update()
        {
            var depth = seaLevel + hook.transform.localPosition.y;
            EventManager.OnDepthUpdate(depth);
        }
    }
}