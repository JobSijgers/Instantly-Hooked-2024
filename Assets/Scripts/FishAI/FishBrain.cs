using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct States
{
    public FishRoamingBehaviour roaming;
}
public class FishBrain : MonoBehaviour
{
    private FishManager FM;
    public IFishAI currentState;
    public States states;
    [SerializeField] private FishData data;
    private void Start()
    {
        FM = FishManager.instance;
        currentState = states.roaming;
        currentState.Initialzie(data);
    }
    private void Update()
    {
        currentState.UpdateState(FM);
        currentState = currentState.switchState();
    }
}
