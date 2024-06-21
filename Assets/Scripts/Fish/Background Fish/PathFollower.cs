using Interfaces;
using PathCreation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fish.Background_Fish
{
    public class PathFollower : MonoBehaviour, IBackgroundFish
    {
        private Vector3 pathOffset;
        private PathCreator pathToFollow;
        private float followSpeed = 5;
        private float distanceTravelled;

        private void Update()
        {
            if (pathToFollow == null) 
                return;
            
            distanceTravelled += followSpeed * Time.deltaTime;
            transform.position = pathToFollow.path.GetPointAtDistance(distanceTravelled) + pathOffset;
            transform.rotation = pathToFollow.path.GetRotationAtDistance(distanceTravelled);
        }

        public void Initialize(PathCreator path, Vector3 offset, float speed)
        {
            pathToFollow = path;
            pathOffset = offset;
            followSpeed = speed;
            distanceTravelled = Random.Range(0, path.path.length);
        }
    }
}