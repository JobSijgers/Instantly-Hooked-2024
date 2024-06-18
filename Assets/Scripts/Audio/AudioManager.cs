using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 0.7f;
            [Range(0.5f, 1.5f)] public float pitch = 1f;
            public bool loop;
            public bool playOnAwake;
            public AudioMixerGroup mixerGroup;
            [HideInInspector] public AudioSource source;
        }

        public static AudioManager instance;
        public Sound[] sounds;

        private void Awake() => instance = this;

        private void Start()
        {
            //play sounds that are set to play on awake
            foreach (Sound sound in sounds)
            {
                if (sound.clip == null)
                {
                    Debug.LogWarning($"Sound clip: {sound.name} is null");
                    continue;
                }
                CreateAudioSource(sound);
            }
        }

        //find sound and play it
        public void PlaySound(string soundName)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.name != soundName)
                    continue;
                sound.source.Play();
                return;
            }
        }
        
        public void StopSound(string soundName)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.name != soundName)
                    continue;
                sound.source.Stop();
                return;
            }
        }

        private void CreateAudioSource(Sound sound)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;
            audioSource.outputAudioMixerGroup = sound.mixerGroup;
            sound.source = audioSource;
            if (sound.playOnAwake)
            {
                audioSource.Play();
            }
        }
    }
}