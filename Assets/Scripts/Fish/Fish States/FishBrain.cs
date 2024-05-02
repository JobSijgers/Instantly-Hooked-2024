using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public FishStates states;
    private float P_struggelSpeed;
    private float P_moveSpeed;
    public FishSpawner SetOriginSpawner(FishSpawner spawner) => OriginSpawner = spawner;
    public void DestroyVisual() => Destroy(Visual); 
    public Vector3 GetNewPosition() => OriginSpawner.GetRandomPos();
    public bool IsStruggeling() => states.Biting.IsStruggeling();
    public float moveSpeed { get { return P_moveSpeed; } set { P_moveSpeed = value; } }
    public float StruggelSpeed { get { return P_struggelSpeed; } set { P_struggelSpeed = value; } }
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
            moveSpeed = value.moveSpeed;
            StruggelSpeed = value.moveSpeed * 2;
            Visual = Instantiate(value.fishObject, transform.position,Quaternion.identity, transform);
        }
    }
    void Start()
    {
        CurrentState = GetComponent<IFishState>();
        CurrentState = states.Roaming;
    }

    void Update()
    {
        CurrentState = CurrentState.SwitchState();
        CurrentState.UpdateState();
    }
    public void OnDisable()
    {
        CurrentState = states.Roaming;
    }
}
