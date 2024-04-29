using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public struct FishStates
{
    public FishRoaming Roaming;
    public FishBiting Biting; 
} 
public class FishBrain : MonoBehaviour
{
    private FishData P_fishData;
    private IFishState P_CurrentState;
    private FishSpawner OriginSpawner;
    private GameObject Visual;
    public float RoamRange;
    public NavMeshAgent NavAgent;
    public FishStates states;
    public GameObject Hook;
    public FishSpawner SetOriginSpawner(FishSpawner spawner) => OriginSpawner = spawner;
    public void DestroyVisual() => Destroy(Visual); 
    public Vector3 GetNewPosition() => OriginSpawner.GetRandomPos();
    public bool IsStruggeling() => states.Biting.IsStruggeling();
    public IFishState CurrentState
    {
        get { return P_CurrentState; }
        set
        {
            IFishState currentstate = P_CurrentState;
            IFishState IncommingState = value;
            if (currentstate != IncommingState)
            {
                value.OnStateActivate();
            }
            P_CurrentState = value;
        }
    }
    public FishData fishData
    {
        get {  return P_fishData; }
        set
        {
            P_fishData = value;
            NavAgent.speed = value.moveSpeed;
            Visual = Instantiate(value.fishObject, transform.position,Quaternion.identity, transform);
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
        //Debug.Log(CurrentState);
        CurrentState = CurrentState.SwitchState();
        CurrentState.UpdateState();
    }
    public void OnDisable()
    {
        CurrentState = states.Roaming;
    }
}
