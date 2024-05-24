using PathCreation;
using UnityEngine;

namespace Interfaces
{
    public interface IBackgroundFish
    {
        public void Initialize(PathCreator path, Vector3 offset, float speed);
    }
}