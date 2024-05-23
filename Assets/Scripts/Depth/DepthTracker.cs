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
            float depth = seaLevel + hook.transform.position.y;
            EventManager.OnDepthUpdate(depth);
        }
    }
}