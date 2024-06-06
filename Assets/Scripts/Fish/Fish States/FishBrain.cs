using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Upgrades.Scriptable_Objects;
using Events;

[Serializable]
public struct FishStates
{
    public FishRoaming Roaming;
    public FishBiting Biting; 
}
public class FishBrain : MonoBehaviour
{
    private FishData P_fishData;

    [Header("states")]
    public FishStates states;
    private IFishState P_CurrentState;

    [Header("Movement")]
    [SerializeField] private float RotateSpeed;
    [SerializeField] private float WiggleAngle;
    private Vector3 P_EndPos;
    private float P_struggelSpeed;
    private float P_moveSpeed;

    [Header("visual")]
    [SerializeField] private GameObject EmptyObject;
    private GameObject Visual;

    private FishSpawner OriginSpawner;

    [Header("Particles")]
    [SerializeField] public ParticleSystem FishGought;

    [Header("Fish Size")]
    public FishSize fishSize;

    //corotines
    private Coroutine RotateC;

    //propeties
    public Vector3 EndPos { get { return P_EndPos; } }
    
    // spawners 
    public FishSpawner SetOriginSpawner(FishSpawner spawner) => OriginSpawner = spawner;
    public FishSpawner GetOriginSpawner() => OriginSpawner;

    //visual
    public void DestroyVisual() => Destroy(Visual);

    // 
    public bool IsStruggeling() => states.Biting.IsStruggeling();

    //movement
    public Vector3 GetNewPosition() => OriginSpawner.GetRandomPos();
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
        get { return P_fishData; }
        set
        {
            P_fishData = value;
            moveSpeed = value.moveSpeed;
            StruggelSpeed = value.moveSpeed * 1.5f;
            Visual = Instantiate(value.fishObject, transform.position, Quaternion.identity, transform);
        }
    }
    public void SetEndPos(Vector3 endpos)
    {
        P_EndPos = endpos;
        EmptyObject.transform.LookAt(EndPos);
        StopOldRotation();
    }
    private void Awake()
    {
        EventManager.UpgradeBought += OnBaitBought;
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
        Quaternion endpos = EmptyObject.transform.rotation;
        if (Visual.transform.rotation != endpos && RotateC == null) RotateC = StartCoroutine(RotateFish(endpos));
    }
    private void StopOldRotation()
    {
        StopAllCoroutines();
        RotateC = null;
    }
    private IEnumerator RotateFish(Quaternion endpos)
    {
        float t = 0.0f;
        Quaternion startpos = Visual.transform.rotation;
        while (t < 1)
        {
            t += Time.deltaTime * 2.5f;
            Visual.transform.rotation = Quaternion.Slerp(startpos, endpos, t);
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
    public void OnBaitBought(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case HookUpgrade hookupgrade:
                states.Roaming.BiteMultiply = hookupgrade.BiteMultiply;
                states.Biting.StamDrainUpgradePower = hookupgrade.StaminaDrain;
                break;
        }
    }
    private void OnDestroy()
    {
        EventManager.UpgradeBought += OnBaitBought;
    }
}


    /// <summary>
    /// dit verkloot de vissen. iets om later een keer werkent te krijgen
    /// </summary>
    //private void FishWiggle()
    //{
    //    float time = Time.time * 2;
    //    float wiggelangle = WiggleAngle;
    //    if (!states.Biting.IsInWater() || states.Biting.IsFishStruggeling())
    //    {
    //        time *= 3;
    //        wiggelangle *= 3;
    //    }
    //    float a = Visual.transform.localEulerAngles.y - (wiggelangle / 2);
    //    Vector3 startrot = new Vector3(Visual.transform.localEulerAngles.x, a, Visual.transform.localEulerAngles.z);
    //    float b = Visual.transform.localEulerAngles.y + (wiggelangle / 2);
    //    Vector3 endrot = new Vector3(Visual.transform.localEulerAngles.x, b, Visual.transform.localEulerAngles.z);
    //    p = Mathf.PingPong(time, 1);
    //    Vector3 newrot = Vector3.Lerp(endrot, startrot, p);
    //    Visual.transform.localEulerAngles = newrot;
    //}