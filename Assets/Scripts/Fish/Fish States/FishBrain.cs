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
    private FishSpawner OriginSpawner;

    [Header("states")]
    public FishStates states;
    private IFishState P_CurrentState;

    [Header("Movement")]
    [SerializeField] private float RotateSpeed;
    private Vector3 P_EndPos;
    private float P_struggelSpeed;
    private float P_moveSpeed;

    [Header("visual")]
    [SerializeField] private GameObject RotationObject;
    private GameObject inner;
    private GameObject Visual;
    private FishUI UI;

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
    public GameObject innerVisual => inner;
    public FishUI FishUI => UI;

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
            inner = Visual.transform.GetChild(0).gameObject;
        }
    }
    public void SetEndPos(Vector3 endpos)
    {
        P_EndPos = endpos;
        RotationObject.transform.LookAt(EndPos);
        StopOldRotation();
    }
    private void Awake()
    {
        EventManager.UpgradeBought += OnBaitBought;
    }
    void Start()
    {
        UI = GetComponent<FishUI>();
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
        Quaternion endpos;
        if (states.Biting.CurrentState == FishBitingState.OnHook) RotationObject.transform.LookAt(Hook.instance.HookOrigin.transform.position);
        else RotationObject.transform.LookAt(EndPos);
        endpos = RotationObject.transform.rotation;
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
                states.Biting.StaminaDrainUpgradePower = hookupgrade.StaminaDrain;
                break;
        }
    }
    private void OnDestroy()
    {
        EventManager.UpgradeBought += OnBaitBought;
    }
}