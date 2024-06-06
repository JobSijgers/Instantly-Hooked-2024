using Fish;
using UnityEngine;
using UnityEngine.UI;

namespace FishPopup
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FishCaughtPopup : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image fishImage;
        [SerializeField] private Image sizeImage;
        [SerializeField] private ParticleSystem particleSystem;

        public void InitPopup(FishData data, Sprite sprite, Sprite sizeSprite, Color color)
        {
            background.sprite = sprite;
            fishImage.sprite = data.fishVisual;
            sizeImage.sprite = sizeSprite;
            ParticleSystem.MainModule particleSystemMain = particleSystem.main;
            particleSystemMain.startColor = color;
        }
    }
}