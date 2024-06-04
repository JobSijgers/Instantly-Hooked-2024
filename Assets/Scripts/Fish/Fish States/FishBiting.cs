using System;
using Enums;
using Events;
using Fish;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Net;

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

    //state change
    private bool OffHook = false;

    [Header("Range")]
    [SerializeField] private float BitingRange;
    [SerializeField] private float IntresstLossafter;

    [Header("struggeling")]
    [SerializeField] private float StruggelTime;
    [SerializeField] private float StruggelAfterTime;

    [Tooltip("hold multiplier")]
    [SerializeField] private float HoldMultiplier;
    [SerializeField] private float RestoreMultyplier;


    [Range(10,170)]
    [Tooltip("angle waarin de vissen naar beneden gaan als ze aan het struggelen zijn")]
    [SerializeField] private float angle;

    [SerializeField] private float strafpunten;

    private bool endposisstruggelpos = false;
    private float tention;

    // coroutine 
    private Coroutine StruggelingC;
    private Coroutine waitForStruggelC;
    private Coroutine ResetStateAfterTimeIntrest;
    private Coroutine CCD;
    void Awake()
    {
        bounds = GetComponent<BoxCollider>();
        Brain = GetComponent<FishBrain>();
        Rod = FindObjectOfType<FishingRod.FishingRod>();
    }
    private void OnEnable()
    {
        EventManager.FishCaught += OnGaught;
    }
    public void OnStateActivate()
    {
        Brain.FishGought.Play();
        BiteState = FishBitingState.goingforhook;
    }
    public IFishState SwitchState()
    {
        if (Hook.instance.FishOnHook != null && Hook.instance.FishOnHook.gameObject != gameObject) return Brain.states.Roaming;
        if (OffHook)
        {
            GetOffHook();
            ResetState();
            EventManager.OnBoatControlsChanged(false);
            return Brain.states.Roaming;
        }
        else return this;
    }
    public void UpdateState()
    {
        if (Vector2.Distance(transform.position, Hook.instance.hook.transform.position) < BitingRange && BiteState == FishBitingState.goingforhook)
        {
            EventManager.OnBoatControlsChanged(true);
            BiteState = FishBitingState.onhook;
            waitForStruggelC = StartCoroutine(WaitForStruggel(0.3f));
        }

        if (BiteState == FishBitingState.struggeling && !IsInWater()) BiteState = FishBitingState.onhook;

        MoveMent();
        Struggeling();

        if (Input.GetMouseButton(1) && BiteState == FishBitingState.goingforhook && ResetStateAfterTimeIntrest == null) ResetStateAfterTimeIntrest = StartCoroutine(FishStateReset());

        if (Input.GetMouseButtonUp(1) && CCD == null) CCD = StartCoroutine(ClickCoolDown());
    }
    private void Struggeling()
    {
        if (BiteState == FishBitingState.struggeling)
        {
            if (Input.GetMouseButton(1))
            {
                float p = 0f;
                if (CCD == null) p = Time.deltaTime * HoldMultiplier;
                else p = strafpunten;
                if (tention > 0) tention -= p;
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

                if (!FishPooler.instance.WaterBlock.bounds.Intersects(bounds.bounds))
                {
                    OffHook = true;
                }
                Brain.SetEndPos(Hook.instance.hook.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, Brain.moveSpeed * Time.deltaTime);
                break;

            case FishBitingState.onhook:

                transform.position = Hook.instance.hook.transform.position;
                break;

            case FishBitingState.struggeling:

                if (!endposisstruggelpos)
                {
                    Vector3 newpos = Brain.EndPos;
                    newpos = ChooseSwimDirection();
                    if (IsPointWithinAngle(Hook.instance.HookOrigin.transform.position, Vector3.down, angle, newpos) && FishPooler.instance.WaterBlock.bounds.Contains(newpos))
                    {
                        endposisstruggelpos = true;
                        Brain.SetEndPos(newpos);
                    }
                }
                float speed = Input.GetMouseButton(1) == true ? 0 : Brain.StruggelSpeed;
                if (Input.GetMouseButton(1) && CCD == null) transform.position = Hook.instance.transform.position;
                else
                {
                    Hook.instance.hook.transform.position = transform.position;
                    transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, speed * Time.deltaTime);
                    Rod.SetLineLength(transform.position);
                }
                if (transform.position == Brain.EndPos)
                {
                    if (tention > 0) tention -= Time.deltaTime * HoldMultiplier;
                }

                if (StruggelingC == null)
                {
                    StruggelingC = StartCoroutine(FishStruggel());
                }
                break;
        }
    }

    bool IsPointWithinAngle(Vector3 origin, Vector3 forward, float angle, Vector3 point)
    {
        Vector3 directionToPoint = (point - origin).normalized;
        float angleToTarget = Vector3.Angle(forward, directionToPoint);

        return angleToTarget < angle / 2;
    }
    private Vector2 ChooseSwimDirection()
    {
        float angle = Random.Range(130, 255);
        Vector2 positionOnCircle = (Vector2)Hook.instance.HookOrigin.transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Rod.GetLineLength() * Hook.instance.Offset;
        return positionOnCircle;
    }
    public bool IsInWater()
    {
        if (FishPooler.instance.WaterBlock.bounds.Intersects(bounds.bounds))
        {
            return true;
        }
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
    private IEnumerator FishStateReset()
    {
        float t = 0.0f;
        while (t < IntresstLossafter)
        {
            t += Time.deltaTime;
            Debug.Log($"intress loss timer : {t}");
            if (!Input.GetMouseButton(1)) ResetStateAfterTimeIntrest = null;
            yield return null;
        }
        if (t >= IntresstLossafter && BiteState == FishBitingState.goingforhook) OffHook = true;
        ResetStateAfterTimeIntrest = null;
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
        endposisstruggelpos = false;
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
    private void OnGaught(FishData t, FishSize s)
    {
        if (Hook.instance.FishOnHook != null)
        {
            EventManager.OnBoatControlsChanged(false);
            FishPooler.instance.ReturnFish(Hook.instance.FishOnHook);
            GetOffHook();
        }
    }
    public void OnDisable()
    {
        ResetState();
        EventManager.FishCaught -= OnGaught;
    }
    public void ResetState()
    {
        StopAllCoroutines();
        endposisstruggelpos = false;
        Brain.SetEndPos(Vector3.zero);
        OffHook = false;
        BiteState = FishBitingState.goingforhook;
        StruggelingC = null;
        waitForStruggelC = null;
    }
    private IEnumerator ClickCoolDown()
    {
        yield return new WaitForSeconds(0.3f);
        CCD = null;
    }
    public bool IsFishStruggeling()
    {
        if (BiteState == FishBitingState.struggeling) return true;
        else return false;
    }
}
