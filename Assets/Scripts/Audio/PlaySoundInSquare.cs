using Generic;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySoundInSquare : MonoBehaviour
    {
        [SerializeField] private Vector2 squareSize;
        [SerializeField] private float maxVolume;
        [SerializeField] private float maxVolumeTime;
        [SerializeField] private AnimationCurve volumeCurve;

        private Transform mainCamera;
        private AudioSource audioSource;
        private float timeInSquare;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            mainCamera = MainCamera.instance.mainCamera;
        }

        private void Update()
        {
            Vector2 cameraPosition = new Vector2(mainCamera.position.x, mainCamera.position.y);
            Vector2 squareCenter = new Vector2(transform.position.x, transform.position.y);
            

            if (IsInsideSquare(cameraPosition, squareCenter, squareSize))
            {
                timeInSquare += Time.deltaTime;
                audioSource.volume = CalculateVolume(timeInSquare);
            }
            else
            {
                audioSource.volume = 0;
                timeInSquare -= Time.deltaTime;
                timeInSquare = Mathf.Clamp(timeInSquare, 0, maxVolumeTime);
                if (timeInSquare != 0)
                {
                    audioSource.volume = CalculateVolume(timeInSquare);

                }
            }
        }
        
        private bool IsInsideSquare(Vector2 point, Vector2 center, Vector2 size)
        {
            return Mathf.Abs(point.x - center.x) <= size.x / 2 && Mathf.Abs(point.y - center.y) <= size.y / 2;
        }

        private float CalculateVolume(float time)
        {
            float t = time / maxVolumeTime;
            return Mathf.Lerp(0, maxVolume, volumeCurve.Evaluate(t));
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(squareSize.x, squareSize.y, 0));
        }
#endif
    }
}