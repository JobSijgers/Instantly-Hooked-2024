using FishingRod;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum FishBitingState
{
    goingforhook,
    struggeling,
    onhook
}
public class FishBiting : MonoBehaviour,IFishState
{
    private FishBrain Brain;
    private FishingRod.FishingRod Rod;
    [SerializeField] private float BitingRange;
    [SerializeField] private FishBitingState fishState;
    [SerializeField] private float StruggelTime;
    [SerializeField] private float StruggelAfterTime;

    private float HoldTimer;
    private float struggelTimer;
    [Tooltip("max time de player de vis kan inreelen waneer struggeling")]
    [SerializeField] private float ToLongHold;
    [Tooltip("hoe lang kan je vasthouden na max hold time")]
    [SerializeField] private float holdoffset;
    [Tooltip("waneer valt de vis als er niet gereeled word")]
    [SerializeField] private float OffHookAfter;

    // coroutine 
    private Coroutine Struggeling;
    private Coroutine waitForStruggel;

    //state change
    private bool OffHook = false;
    private bool struggelreset = false;

    void Awake()
    {
        Brain = GetComponent<FishBrain>();
        Rod = FindObjectOfType<FishingRod.FishingRod>();
    }
    public void OnStateActivate()
    {
        if (Hook.FishOnHook == null) Hook.FishOnHook = Brain;
        else OffHook = true;
        fishState = FishBitingState.goingforhook;
        waitForStruggel = StartCoroutine(WaitForStruggel(1));
    }
    public IFishState SwitchState()
    {
        if (Hook.FishOnHook != null && Hook.FishOnHook.gameObject != gameObject) return Brain.states.Roaming;
        if (OffHook)
        {
            GetOffHook();
            ResetState();
            return Brain.states.Roaming;
        }
        else return this;
    }
    public void UpdateState()
    {
        if (Vector2.Distance(transform.position, Hook.hook.transform.position) < BitingRange && fishState == FishBitingState.goingforhook)
        {
            fishState = FishBitingState.onhook;
        }
        if (struggelreset)
        {
            fishState = FishBitingState.onhook;
            waitForStruggel = StartCoroutine(WaitForStruggel(StruggelAfterTime));
            struggelreset = false;
        }
        if (Struggeling != null)
        {
            if (Input.GetMouseButton(1))
            {
                HoldTimer += Time.deltaTime;
            }
            else
            {
                struggelTimer += Time.deltaTime;
                HoldTimer = 0;
            }
            if (HoldTimer >= ToLongHold)
            {
                Hook.instance.fishline.startColor = Color.red;
                Hook.instance.fishline.endColor = Color.red;
            }
            else
            {
                Hook.instance.fishline.startColor = Color.white;
                Hook.instance.fishline.endColor = Color.white;
            }
            if (HoldTimer >= ToLongHold + holdoffset)
            {
                OffHook = true;
            }
            if (struggelTimer >= OffHookAfter)
            {
                OffHook = true;
            }
        }else
        {
            HoldTimer = 0;
            struggelTimer = 0;
        }
        // set position
        switch (fishState)
        {
            case FishBitingState.goingforhook:
                Brain.NavAgent.SetDestination(Hook.hook.transform.position);
                break;
            case FishBitingState.onhook:
                transform.position = Hook.hook.transform.position;
                break;
            case FishBitingState.struggeling:
                Hook.hook.transform.position = transform.position;
                Rod.SetLineLength(transform.position);
                if (Brain.NavAgent.remainingDistance < Brain.NavAgent.stoppingDistance)
                {
                    Vector2 newpos = Brain.GetNewPosition();
                    if (IsPositionInLineRange(newpos))
                    {
                        Brain.NavAgent.SetDestination(newpos);
                    }
                }
                if (Struggeling == null)
                {
                    Struggeling = StartCoroutine(WaitForEndOfStruggel());
                }
                break;
        }
    }
    private bool IsPositionInLineRange(Vector2 targetpos)
    {
        float dis = Vector2.Distance(transform.position, targetpos);
        if (dis < Rod.GetLineLenght()) return true;
        else return false;
    }
    public void GetOffHook()
    {
        Hook.FishOnHook = null;
    }

    public IEnumerator WaitForStruggel(float time)
    {
        yield return new WaitForSeconds(time);
        fishState = FishBitingState.struggeling;
        Brain.NavAgent.isStopped = false;
        waitForStruggel = null;
    }
    public IEnumerator WaitForEndOfStruggel()
    {
        float t = 0.0f;
        while (t < StruggelTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Struggeling = null;
        struggelreset = true;
        //StruggelReset();
        yield return null;
    }
    private void StruggelReset()
    {
        fishState = FishBitingState.onhook;
        Brain.NavAgent.SetDestination(transform.position);
        Brain.NavAgent.isStopped = true;
    }
    /// <summary>
    /// return if the fish is in a struggle
    /// </summary>
    /// <returns></returns>
    public bool IsStruggeling()
    {
        if (fishState == FishBitingState.struggeling) return true;
        else return false;  
    }
    public void ResetState()
    {
        OffHook = false;
        struggelreset = false;
        fishState = FishBitingState.goingforhook;
        Struggeling = null;
        waitForStruggel = null;
    }
    public void OnDisable()
    {
        ResetState();
    }
}
