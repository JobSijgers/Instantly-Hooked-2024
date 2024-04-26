using FishingRod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public enum FishBitingState
{
    goingforhook,
    struggeling,
    onhook
}
public class FishBiting : MonoBehaviour,IFishState
{
    //refs
    private FishBrain Brain;
    private BoxCollider bounds;
    private FishingRod.FishingRod Rod;

    [Header("Range")]
    private FishBitingState fishState;
    [SerializeField] private float BitingRange;
    [Tooltip("de vis pakt binnen deze range een nieuwe positie voor struggeling")]
    [SerializeField] private float RepositionRange;

    [Header("struggeling")]
    [SerializeField] private float StruggelTime;
    [SerializeField] private float StruggelAfterTime;
    [Tooltip("max time de player de vis kan inreelen waneer struggeling")]
    [SerializeField] private float ToLongHold;
    [Tooltip("hoe lang kan je vasthouden na max hold time")]
    [SerializeField] private float holdoffset;
    [Tooltip("waneer valt de vis als er niet gereeled word")]
    [SerializeField] private float OffHookAfter;
    private float HoldTimer;
    private float struggelTimer;

    // coroutine 
    private Coroutine Struggeling;
    private Coroutine waitForStruggel;

    //state change
    private bool OffHook = false;
    private bool struggelreset = false;

    void Awake()
    {
        bounds = GetComponent<BoxCollider>();
        Brain = GetComponent<FishBrain>();
        Rod = FindObjectOfType<FishingRod.FishingRod>();
    }
    private bool IsInWater()
    {
        if (FishPooler.instance.WaterBlock.bounds.Intersects(bounds.bounds))
        {
            return true;
        }
        else return false;  
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
        Debug.Log(IsInWater());
        transform.LookAt(Brain.states.Roaming.EndPos);
        Vector3 FixedRot = transform.eulerAngles;
        FixedRot.z = 0;
        transform.eulerAngles = FixedRot;
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
            Hook.instance.fishline.startColor = Color.white;
            Hook.instance.fishline.endColor = Color.white;
        }
        // set position
        switch (fishState)
        {
            case FishBitingState.goingforhook:
                Brain.NavAgent.SetDestination(Hook.hook.transform.position);
                Brain.states.Roaming.EndPos = Hook.hook.transform.position;
                break;
            case FishBitingState.onhook:
                transform.position = Hook.hook.transform.position;
                Brain.states.Roaming.EndPos = Hook.hook.transform.position;
                break;
            case FishBitingState.struggeling:
                Hook.hook.transform.position = transform.position;
                Rod.SetLineLength(transform.position);
                if (Brain.NavAgent.remainingDistance < Brain.NavAgent.stoppingDistance)
                {
                    Vector2 newpos = GetRandomPos();
                    if (IsPositionInLineRange(newpos) && newpos != Vector2.zero)
                    {
                        Brain.NavAgent.SetDestination(newpos);
                        Brain.states.Roaming.EndPos = newpos;
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
        waitForStruggel = null;
        if (IsInWater()) fishState = FishBitingState.struggeling;
        else waitForStruggel = StartCoroutine(WaitForStruggel(StruggelAfterTime));
        Brain.NavAgent.isStopped = false;
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
    private Vector2 GetRandomPos()
    {
        if (RandomPoint(out Vector2 point))
        {
            return point;
        }
        return Vector2.zero;
    }
    private bool RandomPoint(out Vector2 point)
    {
        NavMeshHit hit;
        Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * RepositionRange;
        if (NavMesh.SamplePosition(pos, out hit, RepositionRange, NavMesh.AllAreas))
        {
            point = hit.position;
            return true;
        }
        else
        {
            point = Vector2.zero;
            return false;
        }
    }
    public void OnDisable()
    {
        ResetState();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, RepositionRange);
    }
#endif
}
