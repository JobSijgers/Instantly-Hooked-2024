using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameTime
{
    // Class to represent each phase of the day
    [Serializable]
    public struct DayPhase
    {
        public string name;
        public int startHour;
        public int endHour;
        public Texture2D startTexture;
        public Texture2D endTexture;
        public Gradient sunGradient;
        public Gradient horizonWaterGradient;
        public Gradient waterGradient;
    }

    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private List<DayPhase> dayPhases;
        [SerializeField] private Light sun;
        [SerializeField] private Material water;

        private readonly Vector3 startRotation = new Vector3(50, -180, 0);
        private readonly Vector3 endRotation = new Vector3(50, 180, 0);


        private static readonly int Texture1 = Shader.PropertyToID("_Texture1");
        private static readonly int Texture2 = Shader.PropertyToID("_Texture2");
        private static readonly int Blend = Shader.PropertyToID("_Blend");
        private static readonly int HorizonColor = Shader.PropertyToID("_Horizon_Color");
        private static readonly int DeepColor = Shader.PropertyToID("_Deep_Color");
        
        private Texture2D currentStartTexture;
        private Texture2D currentEndTexture;

        private void Start()
        {
            EventManager.TimeUpdate += UpdateSkybox;
        }

        private void OnDestroy()
        {
            ResetMaterials();
            EventManager.TimeUpdate -= UpdateSkybox;
        }

        private void UpdateSkybox(TimeSpan time)
        {
            // Convert time to TimeSpan
            float currentTimeInMinutes = (int)time.TotalMinutes;
            RotateLight(currentTimeInMinutes / 1440f);


            foreach (DayPhase phase in dayPhases)
            {
                // Skip if current time is not within the phase
                if (time.Hours < phase.startHour || time.Hours >= phase.endHour) continue;

                // Calculate start and end time in minutes
                float startTimeInMinutes = phase.startHour * 60;
                float endTimeInMinutes = phase.endHour * 60;

                // Calculate lerp factor
                float t = (currentTimeInMinutes - startTimeInMinutes) / (endTimeInMinutes - startTimeInMinutes);
                float a = Mathf.Clamp(t, 0f, 1f);

                LerpSkybox(phase.startTexture, phase.endTexture, phase.sunGradient, a);
                SetSeaHorizonColor(phase.horizonWaterGradient, a);
                SetDeepSeaColor(phase.waterGradient, a);
                return;
            }
        }

        private void SetSeaHorizonColor(Gradient gradient, float t)
        {
            water.SetColor(HorizonColor, gradient.Evaluate(t));
        }

        private void SetDeepSeaColor(Gradient gradient, float t)
        {
            water.SetColor(DeepColor, gradient.Evaluate(t));
        }

        private void LerpSkybox(Texture a, Texture b, Gradient gradient, float t)
        {
            // Set textures if they are not already set
            if (currentStartTexture != a || currentEndTexture != b)
            {
                RenderSettings.skybox.SetTexture(Texture1, a);
                RenderSettings.skybox.SetTexture(Texture2, b);
            }
            
            RenderSettings.skybox.SetFloat(Blend, t);

            sun.color = gradient.Evaluate(t);
        }

        private void RotateLight(float t)
        {
            sun.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));
        }

        private void ResetMaterials()
        {
            RenderSettings.skybox.SetTexture(Texture1, dayPhases[0].startTexture);
            RenderSettings.skybox.SetTexture(Texture2, dayPhases[0].endTexture);
            RenderSettings.skybox.SetFloat(Blend, 0);
            SetSeaHorizonColor(dayPhases[0].horizonWaterGradient, 0f);
            SetDeepSeaColor(dayPhases[0].waterGradient, 0f);
        }
    }
}