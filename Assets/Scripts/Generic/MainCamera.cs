using UnityEngine;

namespace Generic
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera instance;
        public Transform mainCamera;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}