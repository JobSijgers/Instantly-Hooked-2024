using System;
using Cinemachine;
using Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Boat
{
    public class BoatCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera boatCamera;
        [SerializeField] private CinemachineVirtualCamera hookCamera;
        [SerializeField] private float transitionDepth;
        
        private void OnEnable()
        {
            EventManager.DepthUpdate += TransitionToHook;
        }

        private void OnDisable()
        {
            EventManager.DepthUpdate -= TransitionToHook;
        }
        
        private void TransitionToHook(float depth)
        {
            if (depth < transitionDepth)
            {
                boatCamera.Priority = -1;
                hookCamera.Priority = 1;
            }
            else
            {
                boatCamera.Priority = 1;
                hookCamera.Priority = -1;
            }
        }
    }
}