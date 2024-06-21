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
        
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0);
            mixer.SetFloat("Master", ConvertRange(volumeSlider.value));
            mixer.SetFloat("MusicVolume", ConvertRange(musicSlider.value));
            mixer.SetFloat("SFXVolume", ConvertRange(sfxSlider.value));
        }

        private void OnDestroy()
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
            sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
        }

        private void SetVolume(float volume)
        {
            PlayerPrefs.SetFloat("MasterVolume", volume);
            mixer.SetFloat("Master", ConvertRange(volume));
        }

        private void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat("MusicVolume", volume);
            mixer.SetFloat("MusicVolume", ConvertRange(volume));
        }

        private void SetSfxVolume(float volume)
        {
            PlayerPrefs.SetFloat("SFXVolume", volume);
            mixer.SetFloat("SFXVolume", ConvertRange(volume));
        }
        
        float ConvertRange(float value)
        {
            return Mathf.Lerp(-80, 0, value);
        }
    }
}
