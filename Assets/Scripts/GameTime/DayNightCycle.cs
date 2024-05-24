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
        public Gradient gradient;
    }

    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private List<DayPhase> dayPhases;
        [SerializeField] private Light sun;

        private readonly Vector3 startRotation = new Vector3(50, -180, 0);
        private readonly Vector3 endRotation = new Vector3(50, 180, 0);

        private void Start()
        {
            // Subscribe to TimeUpdate event
            EventManager.TimeUpdate += UpdateSkybox;
        }

        private void OnDestroy()
        {
            // Unsubscribe from TimeUpdate event
            EventManager.TimeUpdate -= UpdateSkybox;
        }

        private void UpdateSkybox(float time)
        {
            // Convert time to TimeSpan
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            float currentTimeInMinutes = (int)timeSpan.TotalMinutes;
            RotateLight(currentTimeInMinutes / 1440f);

            // Iterate over each phase
            foreach (DayPhase phase in dayPhases)
            {
                // Skip if current time is not within the phase
                if (timeSpan.Hours < phase.startHour || timeSpan.Hours >= phase.endHour) continue;

                // Calculate start and end time in minutes
                float startTimeInMinutes = phase.startHour * 60;
                float endTimeInMinutes = phase.endHour * 60;

                // Calculate lerp factor
                float t = (currentTimeInMinutes - startTimeInMinutes) / (endTimeInMinutes - startTimeInMinutes);
                float a = Mathf.Clamp(t, 0f, 1f);

                // lerp between start and end textures
                LerpSkybox(phase.startTexture, phase.endTexture, phase.gradient, a);
                return;
            }
        }

        private void LerpSkybox(Texture a, Texture b, Gradient gradient, float t)
        {
            // Set textures if they are not already set
            if (RenderSettings.skybox.GetTexture("_Texture1") != a)
                RenderSettings.skybox.SetTexture("_Texture1", a);
            if (RenderSettings.skybox.GetTexture("_Texture2") != b)
                RenderSettings.skybox.SetTexture("_Texture2", b);
            
            // Set blend factor
            RenderSettings.skybox.SetFloat("_Blend", t);

            //update directional light color and rotation
            sun.color = gradient.Evaluate(t);
        }
        
        private void RotateLight(float t)
        {
            sun.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));
        }
    }
}