using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBiting : MonoBehaviour,IFishState
{
    private FishBrain Brain;
    void Awake()
    {
        Brain = GetComponent<FishBrain>();
    }
    public IFishState SwitchState()
    {
        return this;
    }

    public void UpdateState()
    {
        Brain.NavAgent.SetDestination(Brain.Hook.transform.position);
    }
}
