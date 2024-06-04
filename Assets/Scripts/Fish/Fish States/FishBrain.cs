using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject VisualHolder; 
    private GameObject Visual;
    public FishStates states;
    private float P_struggelSpeed;
    private float P_moveSpeed;
    [SerializeField] private GameObject EmptyObject;
    [SerializeField] private float RotateSpeed;
    private Vector3 P_EndPos;
    [SerializeField] private float WiggleAngle;

    [Header("Wiggle")]
    [SerializeField] private float Wigggleincrease;
    [SerializeField] private float WiggleSpeed;

    //corotines
    private Coroutine RotateC;

    [Header("Particles")]
    [SerializeField] public ParticleSystem FishGought;

    public FishSize fishSize;
    public Vector3 EndPos { get { return P_EndPos; } }
    public FishSpawner SetOriginSpawner(FishSpawner spawner) => OriginSpawner = spawner;
    public FishSpawner GetOriginSpawner() => OriginSpawner;
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

    public void Initialize(FishData data, FishSize size)
    {
        fishData = data;
        fishSize = size;
    }
    
    public FishData fishData
    {
        get {  return P_fishData; }
        set
        {
            P_fishData = value;
            moveSpeed = value.moveSpeed;
            StruggelSpeed = value.moveSpeed * 1.5f;
            Visual = Instantiate(value.fishObject, transform.position,Quaternion.identity, VisualHolder.transform);
        }
    }
    public void SetEndPos(Vector3 endpos)
    {
        P_EndPos = endpos;
        EmptyObject.transform.LookAt(EndPos);
        StopOldRotation();
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
    float p = 0;
    private void ManageRoation()
    {
        Quaternion endpos = EmptyObject.transform.rotation;
        if (VisualHolder.transform.rotation != endpos && RotateC == null) RotateC = StartCoroutine(RotateFish(endpos));


        float time = Time.time * 2;
        float wiggelangle = WiggleAngle;
        if (!states.Biting.IsInWater() || states.Biting.IsFishStruggeling())
        {
            time *= 3;
            wiggelangle *= 3;
        }
        float a = Visual.transform.localEulerAngles.y - (wiggelangle / 2);
        Vector3 startrot = new Vector3(Visual.transform.localEulerAngles.x, a, Visual.transform.localEulerAngles.z);
        float b = Visual.transform.localEulerAngles.y + (wiggelangle / 2);
        Vector3 endrot = new Vector3(Visual.transform.localEulerAngles.x, b, Visual.transform.localEulerAngles.z);
        p = Mathf.PingPong(time, 1);
        Vector3 newrot = Vector3.Lerp(endrot, startrot, p);
        Visual.transform.localEulerAngles = newrot;
    }
    private void StopOldRotation()
    {
        StopAllCoroutines();
        RotateC = null; 
    }
    private IEnumerator RotateFish(Quaternion endpos)
    {
        float t = 0.0f;
        Quaternion startpos = VisualHolder.transform.rotation;
        while (t < 1)
        {
            t += Time.deltaTime * 2.5f;
            VisualHolder.transform.rotation = Quaternion.Slerp(startpos, endpos, t);
            yield return null;
        }
        yield return null;
        RotateC = null;
    }
    public void OnDisable()
    {
        OriginSpawner = null;
        P_EndPos = Vector3.zero;
        CurrentState = states.Roaming;
    }
}
