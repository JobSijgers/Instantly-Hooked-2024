using System;
using Audio;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Generic
{
    public class PlaySoundAtRandomTimes : MonoBehaviour
    {
        [Serializable]
        public class Sound
        {
            public string sound;
            [Range(0f, 1f)] public float probability;
        }

        [SerializeField] private Sound[] sounds;
        [SerializeField] private float minTimeBetweenPlays;
        [SerializeField] private float maxTimeBetweenPlays;
        [SerializeField] private bool playAtShore;
        [SerializeField] private bool playAtSea;
        [SerializeField] private bool startActive = true;
        private float timeToNextPlay;
        private bool canPlay;


        private void Start()
        {
            canPlay = startActive;
            timeToNextPlay = Time.time + Random.Range(minTimeBetweenPlays, maxTimeBetweenPlays);
            EventManager.DockSuccess += OnDock;
            EventManager.LeftShoreSuccess += OnLeftShore;
        }

        private void Update()
        {
            if (!canPlay)
                return;
            if (timeToNextPlay <= 0)
            {
                AudioManager.instance.PlaySound(SelectRandomSound());
                timeToNextPlay = Random.Range(minTimeBetweenPlays, maxTimeBetweenPlays);
            }
            else
            {
                timeToNextPlay -= Time.deltaTime;
            }
        }

        private void OnDock()
        {
            canPlay = playAtShore;
        }

        private void OnLeftShore()
        {
            canPlay = playAtSea;
        }

        private string SelectRandomSound()
        {
            float random = Random.Range(0f, 1f);
            float sum = 0;
            foreach (Sound sound in sounds)
            {
                sum += sound.probability;
                if (random <= sum)
                {
                    return sound.sound;
                }
            }

            return sounds[0].sound;
        }
        
    }
}