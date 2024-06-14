using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Upgrades.Scriptable_Objects;
using Events;
using PauseMenu;

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

    [Header("scripts")]
    public FishWiggle wiggle;

    [Header("states")]
    public FishStates states;
    private IFishState P_CurrentState;
    private bool activeState = true;

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
    [SerializeField] public ParticleSystem FishIntresst;

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

    // struggeling
    public bool IsStruggeling() => states.Biting.IsStruggeling();

    //movement
    public Vector3 GetNewPosition() => OriginSpawner.GetRandomPos();
    public float moveSpeed { get { return P_moveSpeed; } set { P_moveSpeed = value; } }
    public float StruggelSpeed { get { return P_struggelSpeed; } set { P_struggelSpeed = value; } }

    // state
    public bool ActiveState => activeState;

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
            Vector3 scale = fishData.GetScale(fishSize);
            Visual.transform.localScale = scale;
            RotationObject.transform.localScale = scale;
            states.Biting.Stamina = value.stamina;

            wiggle.SetWiggle();
        }
    }
    public void SetEndPos(Vector3 endpos)
    {
        P_EndPos = endpos;
        RotationObject.transform.LookAt(EndPos);
        StopOldRotation();
    }
    void Start()
    {
        UI = GetComponent<FishUI>();
        CurrentState = GetComponent<IFishState>();
        CurrentState = states.Roaming;
        EventManager.PauseStateChange += PauseFish;
    }
    void Update()
    {
        if (activeState)
        {
            CurrentState = CurrentState.SwitchState();
            CurrentState.UpdateState();
            ManageRoation();
        }
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
    private void PauseFish(PauseState state)
    {
        switch (state)
        {
            case PauseState.InPauseMenu:
                activeState = false; 
                break;
            case PauseState.Playing:
                activeState = true;
                break;
        }
    }
    public void OnDisable()
    {
        OriginSpawner = null;
        P_EndPos = Vector3.zero;
        CurrentState = states.Roaming;
    }
    private void OnDestroy()
    {
        EventManager.PauseStateChange -= PauseFish;
    }
}