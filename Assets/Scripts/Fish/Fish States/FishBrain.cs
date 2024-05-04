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
    [SerializeField] private GameObject EmptyObject;
    [SerializeField] private float RotateSpeed;
    private Coroutine RotateC;
    private Vector3 P_EndPos;
    public Vector3 EndPos { get { return P_EndPos; } }
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
    public void SetEndPos(Vector3 endpos)
    {
        P_EndPos = endpos;
        EmptyObject.transform.LookAt(EndPos);
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
        ManageRoation();
    }
    private void ManageRoation()
    {
        //Vector3 endpos = EmptyObject.transform.eulerAngles;
        Quaternion endpos = EmptyObject.transform.rotation;
        if (Visual.transform.rotation != endpos && RotateC == null) RotateC = StartCoroutine(RotateFish(endpos));
    }
    private IEnumerator RotateFish(Quaternion endpos)
    {
        float t = 0.0f;
        while (t < 1)
        {
            t += Time.deltaTime * 2.5f;
            Visual.transform.rotation = Quaternion.Slerp(Visual.transform.rotation, endpos, t);
            yield return null;
        }
        yield return null;
        RotateC = null;
    }
    public void OnDisable()
    {
        CurrentState = states.Roaming;
    }
}
