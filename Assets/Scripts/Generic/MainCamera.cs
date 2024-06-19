using UnityEngine;

namespace Generic
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera instance;
        public Camera mainCamera;
        
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