using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Unity.VisualScripting;
using UnityEngine;

public class WaterPostChecker : MonoBehaviour
{
    [SerializeField] private GameObject underWaterPost;
    [SerializeField] private GameObject aboveWaterPost;
    private void OnEnable()
    {
        EventManager.DepthUpdate += CheckWaterPost;
    }

    private void OnDisable()
    {
        EventManager.DepthUpdate -= CheckWaterPost;
    }

    private void CheckWaterPost(float depth)
    {
        bool aboveWater = depth > 0;
        underWaterPost.SetActive(!aboveWater);
        aboveWaterPost.SetActive(aboveWater);
    }
}
