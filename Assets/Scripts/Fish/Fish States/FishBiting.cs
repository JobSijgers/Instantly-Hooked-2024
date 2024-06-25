using System;
using Enums;
using Events;
using Fish;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Audio;

public enum FishBitingState
{
    GoingForHook,
    Struggling,
    OnHook
}
public class FishBiting : MonoBehaviour, IFishState
{
    //refs
    private FishBrain brain;
    private BoxCollider bounds;
    private FishingRod.FishingRod rod;

    // state
    private FishBitingState biteState;
    public FishBitingState CurrentState => biteState;

    //state change
    private bool offHook = false;
    private bool endPosIsStrugglePos = false;

    [Header("Range")] [SerializeField] private float BitingRange;
    [SerializeField] private float OffsetFromGround = 0.2f;

    [Header("Intrest")]
    [SerializeField] private float IntrestLossafter;

    [Header("Tention")]
    [SerializeField] private float HoldMultiplier;
    [SerializeField] private float RestoreMultyplier;
    private float tension;

    [Range(1, 180)]
    [SerializeField]
    private float angle;

    [Header("Anitie spam")]
    [SerializeField] private float strafpunten;

    [Header("Stamina")]
    [SerializeField] private float StamRegainMultiply;
    [SerializeField] private float StamDrainMultiply;
    [SerializeField] private float MinStaminaStruggelValue;
    [Range(0.01f, 1f)] [SerializeField] private float StruggelHalfStaminaKans;
    private float MaxStamina;
    private float stamina;

    [Header("LayerMask")] [SerializeField] private LayerMask Ground;

    // coroutine 
    private Coroutine struggelingC;
    private Coroutine resetStateAfterTimeIntrest;
    private Coroutine ccd;
    private Coroutine reGain;
    
    public UnityAction FishStruggleStart;
    private void OnFishStruggleStart() => FishStruggleStart.Invoke();
    public UnityAction FishStruggleEnd;
    private void OnFishStruggleEnd() => FishStruggleEnd.Invoke();
    

    // propeties
    public float Stamina { set { MaxStamina = value; } } 

    void Awake()
    {
        bounds = GetComponent<BoxCollider>();
        brain = GetComponent<FishBrain>();
        rod = FindObjectOfType<FishingRod.FishingRod>();
    }

    private void OnEnable()
    {
        EventManager.FishCaught += OnGaught;
    }

    public void OnStateActivate()
    {
        stamina = MaxStamina;
        brain.FishIntresst.Play();
        biteState = FishBitingState.GoingForHook;
        Hook.instance.FishTargetinglist.Add(brain);
    }

    public IFishState SwitchState()
    {
        if (Hook.instance.FishOnHook != null && Hook.instance.FishOnHook.gameObject != gameObject)
        {
            ResetState();
            return brain.states.Roaming;
        }
        if (offHook)
        {
            GetOffHook();
            ResetState();
            Hook.instance.ResetRodColor();
            EventManager.OnBoatControlsChanged(false);
            FishStruggleEnd?.Invoke();
            return brain.states.Roaming;
        }
        else return this;
    }

    public void UpdateState()
    {
        float dist = Vector2.Distance(transform.position, Hook.instance.hook.transform.position);
        // zodra de fish bij de hook in de buurt is dan word de state veranderd naar fish on hook
        if (dist < BitingRange && biteState == FishBitingState.GoingForHook)
        {
            Hook.instance.FishOnHook = brain;
            FishStruggleStart?.Invoke();
            biteState = FishBitingState.OnHook;
            brain.FishGought.Play();
            if (IsInWater()) EventManager.OnBoatControlsChanged(true);
        }

        if (!brain.GetOriginSpawner().IsInSpwanArea(transform.position))
        {
            brain.FreePass = true;
        }

        if (!brain.GetOriginSpawner().IsInSpwanArea(Hook.instance.hook.transform.position) &&
             Hook.instance.FishOnHook != null && Hook.instance.FishOnHook.gameObject != gameObject) offHook = true;

        // is de vis buiten water terwijl er word gestruggelt dan word er nu niet meer gestruggelt. de boot kan weer varen
        if (biteState == FishBitingState.Struggling && !IsInWater())
        {
            biteState = FishBitingState.OnHook;
            EventManager.OnBoatControlsChanged(false);
        }

        MoveMent();
        Struggeling();

        // anti fish baiting
        if (biteState == FishBitingState.GoingForHook && resetStateAfterTimeIntrest == null)
        {
            resetStateAfterTimeIntrest = StartCoroutine(FishStateReset());
        }

        // anti spam clikcing
        if (Input.GetMouseButtonUp(1) && ccd == null) ccd = StartCoroutine(ClickCoolDown());

        // stop struggel coroutine als vis buiten het water is
        if (struggelingC != null && !IsInWater())
        {
            if (struggelingC != null) StopCoroutine(struggelingC);
            struggelingC = null;
        }
    }

    private void Struggeling()
    {
        if (biteState == FishBitingState.Struggling)
        {
            if (Input.GetMouseButton(1))
            {
                float tentionincrease = 0f;
                if (ccd == null) tentionincrease = Time.deltaTime * HoldMultiplier;
                else tentionincrease = strafpunten;
                if (tension > 0) tension -= tentionincrease;
            }
            else if (tension < 255 && transform.position != brain.EndPos)
            {
                tension += Time.deltaTime * RestoreMultyplier;
            }

            tension = Mathf.Clamp(tension, 0, 255);
            float normalizedHoldTimer = tension / 255.0f;
            Color color = new Color(1.0f, normalizedHoldTimer, normalizedHoldTimer);
            Hook.instance.fishline.startColor = color;
            Hook.instance.fishline.endColor = color;

            if (tension <= 0)
            {
                offHook = true;
                AudioManager.instance.PlaySound("LoseFish");
                EventManager.OnReelFailed();
            }
        }
        else
        {
            tension = 255;
            Hook.instance.ResetRodColor();
        }
    }

    private void MoveMent()
    {
        switch (biteState)
        {
            case FishBitingState.GoingForHook:

                if (!brain.FishAproach.isPlaying) brain.FishAproach.Play();
                if (!FishPooler.instance.WaterBlock.bounds.Intersects(Hook.instance.bounds.bounds))
                {
                    offHook = true;
                }
                brain.SetEndPos(Hook.instance.hook.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, brain.EndPos, brain.moveSpeed * Time.deltaTime);
                break;

            case FishBitingState.OnHook:

                transform.position = Hook.instance.hook.transform.position;
                if (reGain == null)
                {
                    reGain = StartCoroutine(RegainStamina());
                }

                break;

            case FishBitingState.Struggling:
                // zet positie als die nog niet bestaat
                if (!endPosIsStrugglePos || brain.EndPos == Vector3.zero)
                {
                    Vector2 dir = brain.EndPos;
                    dir = ChooseSwimDirection();
                    FindGround(dir, out Vector2 point);
                    if (FishPooler.instance.WaterBlock.bounds.Contains(point))
                    {
                        endPosIsStrugglePos = true;
                        brain.SetEndPos(point);
                    }
                }

                float speed = Input.GetMouseButton(1) == true ? 0 : brain.StruggelSpeed;

                //zet de vis op hook positie als de vis omhook gehesen word
                //zet anders de positie van de vis op de hook en zet de nieuwe benodigde linelenght
                if (Input.GetMouseButton(1) && ccd == null) transform.position = Hook.instance.transform.position;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, brain.EndPos, speed * Time.deltaTime);
                    Hook.instance.hook.transform.position = transform.position;
                    rod.SetLineLength(transform.position);
                }

                //build tention als de vis op de endpos zit
                if (transform.position == brain.EndPos)
                {
                    if (tension > 0) tension -= Time.deltaTime * HoldMultiplier;
                }

                // corotine refresh
                if (struggelingC == null)
                {
                    struggelingC = StartCoroutine(FishStruggel());
                }

                break;
        }
    }

    private Vector2 ChooseSwimDirection()
    {
        float angle = Random.Range(240, 300);
        float angleInRadians = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
        return dir;
    }
    private void FindGround(Vector2 direction, out Vector2 newpoint)
    {
        Debug.DrawRay(Hook.instance.HookOrigin.transform.position, direction * rod.GetLineLength(), Color.red, 2);
        if (Physics.Raycast(Hook.instance.HookOrigin.transform.position, direction, out RaycastHit hit,
                rod.GetLineLength(), Ground))
        {
            if (hit.collider.CompareTag("GeenEndPos"))
            {
                newpoint = Vector2.zero;
                return;
            }
            Vector3 fixedpoint = hit.point;
            fixedpoint.y += OffsetFromGround;
            newpoint = fixedpoint;
        }
        else
        {
            newpoint = direction * rod.GetLineLength();
            newpoint += (Vector2)Hook.instance.HookOrigin.transform.position;
        }
    }
    bool IsPointWithinAngle(Vector3 origin, Vector3 forward, float angle, Vector3 point)
    {
        Vector3 directionToPoint = (point - origin).normalized;
        float angleToTarget = Vector3.Angle(forward, directionToPoint);
        return angleToTarget < angle / 2;
    }
    private IEnumerator FishStateReset()
    {
        float t = 0.0f;
        while (t < IntrestLossafter)
        {
            if (brain.ActiveState)
            {
                t += Time.deltaTime;
                if (!Input.GetMouseButton(1)) resetStateAfterTimeIntrest = null;
            }
            yield return null;
        }

        if (t >= IntrestLossafter && biteState == FishBitingState.GoingForHook) offHook = true;
        resetStateAfterTimeIntrest = null;
    }

    public IEnumerator RegainStamina()
    {
        while (stamina < MaxStamina)
        {
            if (brain.ActiveState)
            {
                stamina += Time.deltaTime * StamRegainMultiply;
                if (BeginStruggelBeforeFullC == null) BeginStruggelBeforeFullC = StartCoroutine(BeginStruggelBeforeFull());
            }
            yield return null;
        }
        if (IsInWater())
        {
            biteState = FishBitingState.Struggling;
            struggelingC = StartCoroutine(FishStruggel());
        }
        reGain = null;
    }

    Coroutine BeginStruggelBeforeFullC;

    private IEnumerator BeginStruggelBeforeFull()
    {
        // kies of de vis met niet volle stamina kan gaan struggelen
        if (stamina > MinStaminaStruggelValue)
        {
            float RandomValue = Random.value;
            if (RandomValue < StruggelHalfStaminaKans)
            {
                biteState = FishBitingState.Struggling;
                if (reGain != null) StopCoroutine(reGain);
                reGain = null;
            }
        }

        // zet een colldown van een seconden op deze functie omdat anders de vis altijd struggelt op StruggelHalfStaminaKans value
        yield return new WaitForSeconds(1);
        BeginStruggelBeforeFullC = null;
    }

    public IEnumerator FishStruggel()
    {
        
        while (stamina > 0.1f)
        {
            if (brain.ActiveState) stamina -= Time.deltaTime * StamDrainMultiply * FishUpgradeCheck.instance.staminaDrainUpgradePower;
            yield return null;
        }

        struggelingC = null;
        StruggelReset();
        yield return null;
    }

    private void StruggelReset()
    {
        brain.SetEndPos(Vector3.zero);
        biteState = FishBitingState.OnHook;
        endPosIsStrugglePos = false;
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
        if (biteState == FishBitingState.Struggling) return true;
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
        Hook.instance.FishTargetinglist.Remove(brain);
        ResetState();
        EventManager.FishCaught -= OnGaught;
    }

    public void ResetState()
    {
        StopAllCoroutines();
        endPosIsStrugglePos = false;
        brain.SetEndPos(Vector3.zero);
        offHook = false;
        biteState = FishBitingState.GoingForHook;
        struggelingC = null;
        resetStateAfterTimeIntrest = null;
        ccd = null;
        reGain = null;
        Hook.instance.FishTargetinglist.Remove(brain);
    }

    private IEnumerator ClickCoolDown()
    {
        yield return new WaitForSeconds(0.3f);
        ccd = null;
    }

    public bool IsFishStruggeling()
    {
        if (biteState == FishBitingState.Struggling) return true;
        else return false;
    }

    public void GetStaminaStats(out float stamina, out float maxstamina)
    {
        stamina = this.stamina;
        maxstamina = MaxStamina;
    }

    public float GetTension()
    {
        return tension / 255;
    }
}