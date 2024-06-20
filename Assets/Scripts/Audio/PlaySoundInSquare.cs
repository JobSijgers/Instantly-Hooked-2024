using Generic;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySoundInSquare : MonoBehaviour
    {
        [SerializeField] private Vector2 squareSize;
        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume;
        [SerializeField] private AnimationCurve volumeCurve;

        private Transform mainCamera;
        private AudioSource audioSource;

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
                float distance = CalculateDistanceToBorder(cameraPosition, squareCenter, squareSize);
                audioSource.volume = CalculateVolume(distance);
            }
            else
            {
                audioSource.volume = 0;
            }
        }

        private float CalculateDistanceToBorder(Vector2 point, Vector2 center, Vector2 size)
        {
            float dx = Mathf.Abs(Mathf.Abs(point.x - center.x) - size.x / 2);
            float dy = Mathf.Abs(Mathf.Abs(point.y - center.y) - size.y / 2);
            return Mathf.Max(dx, dy);
        }
        private bool IsInsideSquare(Vector2 point, Vector2 center, Vector2 size)
        {
            return Mathf.Abs(point.x - center.x) <= size.x / 2 && Mathf.Abs(point.y - center.y) <= size.y / 2;
        }

        private float CalculateVolume(float distance)
        {
            float t = distance / Mathf.Max(squareSize.x, squareSize.y);
            return Mathf.Lerp(minVolume, maxVolume, volumeCurve.Evaluate(t));
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