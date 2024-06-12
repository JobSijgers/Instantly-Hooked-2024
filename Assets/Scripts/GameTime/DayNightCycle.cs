using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

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
        public Gradient waterGradient;
    }

    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private List<DayPhase> dayPhases;
        [SerializeField] private Light sun;
        [SerializeField] private Material water;

        private readonly Vector3 startRotation = new Vector3(50, -180, 0);
        private readonly Vector3 endRotation = new Vector3(50, 180, 0);

        private void Start()
        {
            EventManager.TimeUpdate += UpdateSkybox;
        }

        private void OnDestroy()
        {
            EventManager.TimeUpdate -= UpdateSkybox;
            LerpSkybox(dayPhases[0].startTexture, dayPhases[0].endTexture, dayPhases[0].sunGradient, 0f);
            SetSeaColor(dayPhases[0].waterGradient, 0f);
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
                SetSeaColor(phase.waterGradient, a);
                return;
            }
        }

        private void SetSeaColor(Gradient gradient, float t)
        {
            water.SetColor("_Horizon_Color", gradient.Evaluate(t));
        }

        private void LerpSkybox(Texture a, Texture b, Gradient gradient, float t)
        {
            // Set textures if they are not already set
            if (RenderSettings.skybox.GetTexture("_Texture1") != a)
                RenderSettings.skybox.SetTexture("_Texture1", a);
            if (RenderSettings.skybox.GetTexture("_Texture2") != b)
                RenderSettings.skybox.SetTexture("_Texture2", b);

            RenderSettings.skybox.SetFloat("_Blend", t);

            sun.color = gradient.Evaluate(t);
        }

        private void RotateLight(float t)
        {
            sun.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));
        }
    }
}