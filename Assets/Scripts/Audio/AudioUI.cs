using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class AudioUI : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private AudioMixer mixer;

    
        private void Start()
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
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
            return Mathf.Lerp(-80, 20, value);
        }
    }
}
