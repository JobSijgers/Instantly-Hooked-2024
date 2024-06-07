using System;
using Enums;
using Events;
using Fish;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FishBitingState
{
    goingforhook,
    struggeling,
    onhook
}
public class FishBiting : MonoBehaviour, IFishState
{
    //refs
    private FishBrain Brain;
    private BoxCollider bounds;
    private FishingRod.FishingRod Rod;

    // state
    private FishBitingState BiteState;
    public FishBitingState CurrentState => BiteState;

    //state change
    private bool OffHook = false;
    private bool endposisstruggelpos = false;

    [Header("Range")]
    [SerializeField] private float BitingRange;
    [SerializeField] private float IntresstLossafter;

    [Tooltip("hold multiplier")]
    [SerializeField] private float HoldMultiplier;
    [SerializeField] private float RestoreMultyplier;
    private float tention;

    [Range(10, 170)]
    [Tooltip("angle waarbinnen de vissen naar beneden gaan als ze aan het struggelen zijn")]
    [SerializeField] private float angle;

    [SerializeField] private float strafpunten;

    [Header("Stamina")]
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StamRegainMultiply;
    [SerializeField] private float StamDrainMultiply;
    [SerializeField] private float MinStaminaStruggelValue;
    [Range(1,10)]
    [SerializeField] private float StruggelHalfStamina;
    private float StamDrainUpgradePower_p = 1;
    private float Stamina;
    public float StamDrainUpgradePower { get { return StamDrainUpgradePower_p; } set { StamDrainUpgradePower = value; } }

    [Header("LayerMask")]
    [SerializeField] private LayerMask Ground;

    // coroutine 
    private Coroutine StruggelingC;
    private Coroutine ResetStateAfterTimeIntrest;
    private Coroutine CCD;
    private Coroutine ReGain;
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
        Stamina = MaxStamina;
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
            Hook.instance.ResetRodColor();
            EventManager.OnBoatControlsChanged(false);
            Brain.UI.ActiceState(false);
            return Brain.states.Roaming;
        }
        else return this;
    }
    public void UpdateState()
    {
        // zodra de fish bij de hook in de buurt is dan word de state veranderd naar fish on hook
        if (Vector2.Distance(transform.position, Hook.instance.hook.transform.position) < BitingRange && BiteState == FishBitingState.goingforhook)
        {
            EventManager.OnBoatControlsChanged(true);
            Brain.UI.ActiceState(true);
            BiteState = FishBitingState.onhook;
        }

        // is de vis buiten water terwijl er word gestruggelt dan word er nu niet meer gestruggelt
        if (BiteState == FishBitingState.struggeling && !IsInWater()) BiteState = FishBitingState.onhook;

        //refrech coroutine
        //niet nodig?
        //if (BiteState == FishBitingState.struggeling && StruggelingC == null) StruggelingC = StartCoroutine(FishStruggel());
        //if (BiteState == FishBitingState.onhook && ReGain == null) ReGain = StartCoroutine(RegainStamina());

        MoveMent();
        Struggeling();

       // anti fish baiting
        if (Input.GetMouseButton(1) && BiteState == FishBitingState.goingforhook && ResetStateAfterTimeIntrest == null) ResetStateAfterTimeIntrest = StartCoroutine(FishStateReset());
        else if (!Input.GetMouseButton(1) && ResetStateAfterTimeIntrest != null)
        {
            StopCoroutine(ResetStateAfterTimeIntrest);
            ResetStateAfterTimeIntrest = null;
        }

        // anti spam clikcing
        if (Input.GetMouseButtonUp(1) && CCD == null) CCD = StartCoroutine(ClickCoolDown());
    }
    private void Struggeling()
    {
        if (BiteState == FishBitingState.struggeling)
        {
            if (Input.GetMouseButton(1))
            {
                float tentionincrease = 0f;
                if (CCD == null) tentionincrease = Time.deltaTime * HoldMultiplier;
                else tentionincrease = strafpunten;
                if (tention > 0) tention -= tentionincrease;
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
            Hook.instance.ResetRodColor();
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
                if (ReGain == null)
                {
                    ReGain = StartCoroutine(RegainStamina());
                }
                break;

            case FishBitingState.struggeling:
                // zet positie als die nog niet bestaat
                if (!endposisstruggelpos)
                {
                    Vector3 newpos = Brain.EndPos;
                    newpos = ChooseSwimDirection();
                    FindGround(newpos,out Vector3 point);
                    if (IsPointWithinAngle(Hook.instance.HookOrigin.transform.position, Vector3.down, angle, point) && FishPooler.instance.WaterBlock.bounds.Contains(point))
                    {
                        endposisstruggelpos = true;
                        Brain.SetEndPos(point);
                    }
                }

                float speed = Input.GetMouseButton(1) == true ? 0 : Brain.StruggelSpeed;

                //zet de vis op hook positie als de vis omhook gehesen word
                //zet anders de positie van de vis op de hook en zet de nieuwe benodigde linelenght
                if (Input.GetMouseButton(1) && CCD == null) transform.position = Hook.instance.transform.position;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, speed * Time.deltaTime);
                    Hook.instance.hook.transform.position = transform.position;
                    Rod.SetLineLength(transform.position);
                }

                //build tention als de vis op de endpos zit
                if (transform.position == Brain.EndPos)
                {
                    if (tention > 0) tention -= Time.deltaTime * HoldMultiplier;
                }

                // corotine refresh
                if (StruggelingC == null)
                {
                    StruggelingC = StartCoroutine(FishStruggel());
                }
                break;
        }
    }
    private void FindGround(Vector3 point, out Vector3 newpoint)
    {
        if (Physics.Raycast(Hook.instance.HookOrigin.transform.position, point, out RaycastHit hit, 1000, Ground)) newpoint = hit.point;
        else newpoint = point;
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
    private IEnumerator FishStateReset()
    {
        float t = 0.0f;
        while (t < IntresstLossafter)
        {
            t += Time.deltaTime;
            if (!Input.GetMouseButton(1)) ResetStateAfterTimeIntrest = null;
            yield return null;
        }
        if (t >= IntresstLossafter && BiteState == FishBitingState.goingforhook) OffHook = true;
        ResetStateAfterTimeIntrest = null;
    }
    public IEnumerator RegainStamina()
    {
        while (Stamina < MaxStamina)
        {
            Stamina += Time.deltaTime * StamRegainMultiply;
            // kies of de vis met niet volle stamina kan gaan struggelen
            //if (Stamina > MinStaminaStruggelValue)
            //{
            //    float RandomValue = Random.value;
            //    if (RandomValue < StruggelHalfStamina)
            //    {
            //        BiteState = FishBitingState.struggeling;
            //        StruggelingC =  StartCoroutine(FishStruggel());
            //        ReGain = null;
            //    }
            //}
            yield return null;
        }
        BiteState = FishBitingState.struggeling;
        StruggelingC = StartCoroutine(FishStruggel());
        ReGain = null;
    }
    public IEnumerator FishStruggel()
    {
        while (Stamina > 0.1f)
        {
            Stamina -= Time.deltaTime * StamDrainMultiply * StamDrainUpgradePower;
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
    /// <summary>
    /// return if the fish is in a struggle
    /// </summary>
    /// <returns></returns>
    public bool IsStruggeling()
    {
        if (BiteState == FishBitingState.struggeling) return true;
        else return false;  
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

    public void GetStaminaStats(out float stamina, out float maxstamina)
    {
        stamina = Stamina;
        maxstamina = MaxStamina;
    }
}
