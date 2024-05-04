using FishingRod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // state
    private FishBitingState BiteState;

    [Header("Range")]
    [SerializeField] private float BitingRange;
    [Tooltip("de vis pakt binnen deze range een nieuwe positie voor struggeling")]
    [SerializeField] private float Struggelrange;
    [SerializeField] private float IntresstLoseDistance;

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

    void Awake()
    {
        bounds = GetComponent<BoxCollider>();
        Brain = GetComponent<FishBrain>();
        Rod = FindObjectOfType<FishingRod.FishingRod>();
    }
    public void OnStateActivate()
    {
        BiteState = FishBitingState.goingforhook;
    }
    public IFishState SwitchState()
    {
        if (Hook.instance.FishOnHook != null && Hook.instance.FishOnHook.gameObject != gameObject) return Brain.states.Roaming;
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
        if (Vector2.Distance(transform.position, Hook.instance.hook.transform.position) < BitingRange && BiteState == FishBitingState.goingforhook)
        {
            BiteState = FishBitingState.onhook;
            waitForStruggel = StartCoroutine(WaitForStruggel(0.3f));
        }
        if (BiteState == FishBitingState.struggeling)
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
        // movement
        switch (BiteState)
        {
            case FishBitingState.goingforhook:

                Brain.SetEndPos(Hook.instance.hook.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos,Brain.moveSpeed * Time.deltaTime);
                break;

            case FishBitingState.onhook:

                transform.position = Hook.instance.hook.transform.position;
                break;

            case FishBitingState.struggeling:

                if (transform.position == Brain.EndPos)
                {
                    Vector2 newpos = Brain.GetNewPosition();
                    if (IsPositionInLineRange(newpos))
                    {
                        Brain.SetEndPos(newpos);
                    }
                }

                transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, Brain.moveSpeed * Time.deltaTime);
                Hook.instance.hook.transform.position = transform.position;
                Rod.SetLineLength(transform.position);

                if (Struggeling == null)
                {
                    Struggeling = StartCoroutine(FishStruggel());
                }
                break;
        }
        if (Vector3.Distance(transform.position, Hook.instance.hook.transform.position) > IntresstLoseDistance)
        {
            OffHook = true;
        }
    }
    private bool IsInWater()
    {
        if (FishPooler.instance.WaterBlock.bounds.Intersects(bounds.bounds))
        {
            return true;
        }
        else return false;  
    }
    private bool IsPositionInLineRange(Vector2 targetpos)
    {
        float dis = Vector2.Distance(Hook.instance.HookOrigin.transform.position, targetpos);
        dis -= 0.1f;
        if (dis < Rod.GetLineLenght()) return true;
        else return false;
    }
    public void GetOffHook()
    {
        Hook.instance.FishOnHook = null;
    }
    public IEnumerator WaitForStruggel(float time)
    {
        yield return new WaitForSeconds(time);
        waitForStruggel = null;
        if (IsInWater())
        {
            BiteState = FishBitingState.struggeling;
            Brain.SetEndPos(transform.position);
        }
        else waitForStruggel = StartCoroutine(WaitForStruggel(StruggelAfterTime));
    }
    public IEnumerator FishStruggel()
    {
        float t = 0.0f;
        while (t < StruggelTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Struggeling = null;
        StruggelReset();
        yield return null;
    }
    private void StruggelReset()
    {
        BiteState = FishBitingState.onhook;
        waitForStruggel = StartCoroutine(WaitForStruggel(StruggelAfterTime));
    }
    /// <summary>
    /// return if the fish is in a struggle
    /// </summary>
    /// <returns></returns>
    public bool IsStruggeling()
    {
        if (BiteState == FishBitingState.struggeling) return true;
        else return false;  
    }
    public void OnDisable()
    {
        ResetState();
    }
    public void ResetState()
    {
        OffHook = false;
        BiteState = FishBitingState.goingforhook;
        Struggeling = null;
        waitForStruggel = null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, Struggelrange);
    }
#endif
}
