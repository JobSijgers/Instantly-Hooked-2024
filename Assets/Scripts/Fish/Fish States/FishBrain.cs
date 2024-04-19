using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public struct FishStates
{
    public FishRoaming Roaming;
    //public 
} 
public class FishBrain : MonoBehaviour
{
    private FishData P_fishData;
    private IFishState CurrentState;
    private FishSpawner OriginSpawner;
    private GameObject Visual;
    public float RoamRange;
    public NavMeshAgent NavAgent;
    public FishStates states;
    public FishSpawner SetOriginSpawner(FishSpawner spawner) => OriginSpawner = spawner;
    public void DestroyVisual() => Destroy(Visual); 
    public Vector3 GetNewPos() => OriginSpawner.GetRandomPos();
    public FishData fishData
    {
        get {  return P_fishData; }
        set
        {
            P_fishData = value;
            NavAgent.speed = value.moveSpeed;
            Visual = Instantiate(value.fishObject, transform.position,Quaternion.identity, this.transform);
        }
    }
    void Start()
    {
        CurrentState = GetComponent<IFishState>();
        NavAgent = GetComponent<NavMeshAgent>();
        CurrentState = states.Roaming;
    }

    void Update()
    {
        CurrentState.UpdateState();
        CurrentState = CurrentState.SwitchState();
    }
}
