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

    [Tooltip("hold multiplier")]
    [SerializeField] private float HoldMultiplier;
    [SerializeField] private float RestoreMultyplier;

    [Tooltip("hoe lang kan je vasthouden na max hold time")]
    [SerializeField] private float holdoffset;

    [Tooltip("waneer valt de vis als er niet gereeled word")]
    [SerializeField] private float OffHookAfter;
    private float tention;
    private float struggelTimer;

    private bool endposisstruggelpos = false;

    // coroutine 
    private Coroutine StruggelingC;
    private Coroutine waitForStruggelC;

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
            waitForStruggelC = StartCoroutine(WaitForStruggel(0.3f));
        }

        if (BiteState == FishBitingState.struggeling && !IsInWater()) BiteState = FishBitingState.onhook;

        MoveMent();
        Struggeling();

        if (Vector3.Distance(transform.position, Hook.instance.hook.transform.position) > IntresstLoseDistance)
        {
            OffHook = true;
        }
    }
    private void Struggeling()
    {
        if (BiteState == FishBitingState.struggeling)
        {
            if (Input.GetMouseButton(1))
            {
                if (tention > 0) tention -= Time.deltaTime * HoldMultiplier;
            }
            else if (tention < 255 && transform.position != Brain.EndPos)
            {
                tention += Time.deltaTime * RestoreMultyplier;
            }
            tention = Mathf.Clamp(tention, 0, 255);
            float normalizedHoldTimer = tention / 255.0f;
            Color color = new Color(1.0f, normalizedHoldTimer, normalizedHoldTimer);
            Hook.instance.fishline.startColor = color;
            Hook.instance.fishline.endColor = color;

            if (tention <= 0)
            {
                OffHook = true;
            }
        }
        else
        {
            tention = 255;
            Hook.instance.fishline.startColor = Color.white;
            Hook.instance.fishline.endColor = Color.white;
        }
    }
    private void MoveMent()
    {
        switch (BiteState)
        {
            case FishBitingState.goingforhook:

                Brain.SetEndPos(Hook.instance.hook.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, Brain.moveSpeed * Time.deltaTime);
                break;

            case FishBitingState.onhook:

                transform.position = Hook.instance.hook.transform.position;
                break;

            case FishBitingState.struggeling:

                if (!endposisstruggelpos)
                {
                    Vector2 newpos = Brain.EndPos;
                    newpos = ChooseSwimDirection();
                    if (FishPooler.instance.WaterBlock.bounds.Contains(newpos))
                    {
                        endposisstruggelpos = true;
                        Brain.SetEndPos(newpos);
                    }
                }
                if (transform.position == Brain.EndPos)
                {
                    if (tention > 0) tention -= Time.deltaTime * HoldMultiplier;
                }
                float speed = Input.GetMouseButton(1) == true ? 0 : Brain.StruggelSpeed;
                if (Input.GetMouseButton(1))
                {
                    transform.position = Hook.instance.transform.position;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, speed * Time.deltaTime);
                    Hook.instance.hook.transform.position = transform.position;
                    Rod.SetLineLength(transform.position);
                }

                if (StruggelingC == null)
                {
                    StruggelingC = StartCoroutine(FishStruggel());
                }
                break;
        }
    }
    private Vector2 ChooseSwimDirection()
    {
        float angle = Random.Range(0, 90);
        Vector2 positionOnCircle = (Vector2)Hook.instance.HookOrigin.transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Rod.GetLineLength() * Hook.instance.Offset;
        return positionOnCircle;
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
        if (dis < Rod.GetLineLength()) return true;
        else return false;
    }
    public void GetOffHook()
    {
        Hook.instance.FishOnHook = null;
    }
    public IEnumerator WaitForStruggel(float time)
    {
        yield return new WaitForSeconds(time);
        waitForStruggelC = null;
        if (IsInWater())
        {
            BiteState = FishBitingState.struggeling;
            Brain.SetEndPos(transform.position);
        }
        else waitForStruggelC = StartCoroutine(WaitForStruggel(StruggelAfterTime));
    }
    public IEnumerator FishStruggel()
    {
        float t = 0.0f;
        while (t < StruggelTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        StruggelingC = null;
        StruggelReset();
        yield return null;
    }
    private void StruggelReset()
    {
        Brain.SetEndPos(Vector3.zero);
        BiteState = FishBitingState.onhook;
        waitForStruggelC = StartCoroutine(WaitForStruggel(StruggelAfterTime));
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
        endposisstruggelpos = false;
        Brain.SetEndPos(Vector3.zero);
        OffHook = false;
        BiteState = FishBitingState.goingforhook;
        StruggelingC = null;
        waitForStruggelC = null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, Struggelrange);
    }
#endif
}
