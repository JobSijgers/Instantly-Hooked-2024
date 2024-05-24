using System;
using System.Diagnostics.Contracts;
using Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Depth
{
    public class WaterPostChecker : MonoBehaviour
    {
        [Serializable]
        public class VignetteValues
        {
            public float intensity;
            public float smoothness;
            public Color color;

            public static VignetteValues LerpVignetteValues(VignetteValues a, VignetteValues b, float t)
            {
                return new VignetteValues
                {
                    intensity = Mathf.Lerp(a.intensity, b.intensity, t),
                    smoothness = Mathf.Lerp(a.smoothness, b.smoothness, t),
                    color = Color.Lerp(a.color, b.color, t)
                };
            }
        }
        [SerializeField] private GameObject underWaterPost;

        [SerializeField] private GameObject aboveWaterPost;


        

        [Header("Underwater Vignette")]
        private VignetteValues baseWaterVignetteValues;
        [SerializeField] private int baseDepth;
        
        [SerializeField] private VignetteValues maxWaterVignetteValues;
        [SerializeField] private int maxDepth;

        private Vignette underWaterVignette;


        private void Start()
        {
            Volume vol = underWaterPost.GetComponent<Volume>();
            
            if (!vol.profile.TryGet(out Vignette vignette)) return;
            underWaterVignette = vignette;
            baseWaterVignetteValues = new VignetteValues
            {
                intensity = vignette.intensity.value,
                smoothness = vignette.smoothness.value,
                color = vignette.color.value
            };
        }

        private void OnEnable()
        {
            EventManager.DepthUpdate += CheckWaterPost;
        }

        private void OnDisable()
        {
            EventManager.DepthUpdate -= CheckWaterPost;
        }
        private void CheckWaterPost(float depth)
        {
            bool aboveWater = depth > 0;
            
            if (aboveWater) return;
            
            float t = Mathf.InverseLerp(baseDepth, maxDepth, depth);
            VignetteValues vignette = VignetteValues.LerpVignetteValues(baseWaterVignetteValues, maxWaterVignetteValues, t);
            underWaterVignette.intensity.value = vignette.intensity;
            underWaterVignette.smoothness.value = vignette.smoothness;
            underWaterVignette.color.value = vignette.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Water")) return;
            underWaterPost.SetActive(true);
            aboveWaterPost.SetActive(false);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Water")) return;
            underWaterPost.SetActive(false);
            aboveWaterPost.SetActive(true);
        }
    }
}
