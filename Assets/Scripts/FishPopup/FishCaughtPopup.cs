using Fish;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FishPopup
{
    public class FishCaughtPopup : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text fishName;
        [SerializeField] private Image fishImage;
        [SerializeField] private Image sizeImage;
        [SerializeField] private ParticleSystem[] particleSystems;
        private const float DeleteAfterSeconds = 2f;

        public void InitPopup(FishData data, Sprite sprite, Sprite sizeSprite, Color color)
        {
            background.sprite = sprite;
            fishName.text = data.fishName;
            fishImage.sprite = data.fishVisual;
            sizeImage.sprite = sizeSprite;

            Destroy(gameObject, DeleteAfterSeconds);
            foreach (ParticleSystem particle in particleSystems)
            {   
                ParticleSystem.MainModule particleSystemMain = particle.main;
                particleSystemMain.startColor = color;
            }
        }
    }
}