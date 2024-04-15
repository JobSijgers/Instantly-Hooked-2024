using System;
using System.Collections;
using System.Collections.Generic;
using Fish;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct States
{
    public FishRoamingBehaviour roaming;
    public TrackBobberBehaviour trackBobber;
    public FishFightBehaviour fishFight;
}
public class FishBrain : MonoBehaviour
{
    public FishData data;
    public FishManager FM;
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
        var switchResult = currentState.switchState();

        currentState.UpdateState(FM);

        if (switchResult.Item2)
        {
            currentState = switchResult.Item1;
            currentState.Initialize(data);
        }
    }
}
