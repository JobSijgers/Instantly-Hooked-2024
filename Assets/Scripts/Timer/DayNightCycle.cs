using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Timer
{
    [Serializable]
    public struct DayPhase
    {
        public string name;
        public int startHour;
        public int endHour;
        public Texture2D startTexture;
        public Texture2D endTexture;
    }

    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private List<DayPhase> dayPhases;

        private void Start()
        {
            EventManager.TimeUpdate += UpdateSkybox;
        }

        private void OnDestroy()
        {
            EventManager.TimeUpdate -= UpdateSkybox;
        }

        private void UpdateSkybox(float time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            float currentTimeInMinutes = (int)timeSpan.TotalMinutes;

            foreach (DayPhase phase in dayPhases)
            {
                if (timeSpan.Hours < phase.startHour || timeSpan.Hours >= phase.endHour) continue;
                
                float startTimeInMinutes = phase.startHour * 60;
                float endTimeInMinutes = phase.endHour * 60;
                float t = (currentTimeInMinutes - startTimeInMinutes) / (endTimeInMinutes - startTimeInMinutes);
                float a = Mathf.Clamp(t, 0f, 1f);
                LerpSkybox(phase.startTexture, phase.endTexture, a);
                return;
            }
        }

        private void LerpSkybox(Texture a, Texture b, float t)
        {
            if (RenderSettings.skybox.GetTexture("_Texture1") != a)
                RenderSettings.skybox.SetTexture("_Texture1", a);
            if (RenderSettings.skybox.GetTexture("_Texture2") != b)
                RenderSettings.skybox.SetTexture("_Texture2", b);

            RenderSettings.skybox.SetFloat("_Blend", t);
        }
    }
}