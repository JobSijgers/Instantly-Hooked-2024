using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
using Views;

namespace Audio
{
    public class AudioUI : ViewComponent
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private AudioMixer mixer;


        public override void Initialize(UnityEvent onShow, UnityEvent onHide)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);

            volumeSlider.value = 1;
            musicSlider.value = 1;
            sfxSlider.value = 1;
        }

        private void OnDestroy()
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
            sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
        }

        private void SetVolume(float volume)
        {
            mixer.SetFloat("Master", ConvertRange(volume));
        }

        private void SetMusicVolume(float volume)
        {
            mixer.SetFloat("MusicVolume", ConvertRange(volume));
        }

        private void SetSfxVolume(float volume)
        {
            mixer.SetFloat("SFXVolume", ConvertRange(volume));
        }

        private float ConvertRange(float value)
        {
            if (value == 0)
            {
                return -80;
            }

            return Mathf.Log10(value) * 20;
        }
    }
}