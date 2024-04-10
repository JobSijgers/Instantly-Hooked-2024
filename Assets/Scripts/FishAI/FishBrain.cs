using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct States
{
    public FishRoamingBehaviour roaming;
    public TrackBobberBehaviour trackBobber;
}
public class FishBrain : MonoBehaviour
{
    public FishData data;
    private FishManager FM;
    public IFishAI currentState;
    public States states;
    public Bobber bobber;
    private void Start()
    {
        FM = FishManager.instance;
        bobber = FM.bobber;
        currentState = states.roaming;
        currentState.Initialize(data);
    }
    private void Update()
    {
        Debug.Log(currentState);
        currentState.UpdateState(FM);
        if (currentState.switchState().Item2 == true)
        {
            currentState = currentState.switchState().Item1;
            currentState.Initialize(data);
        }
        else currentState = currentState.switchState().Item1;
    }
}
