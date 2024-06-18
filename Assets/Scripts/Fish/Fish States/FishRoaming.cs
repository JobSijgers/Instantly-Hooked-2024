using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Events;
using System;
using Random = UnityEngine.Random;

public class FishRoaming : MonoBehaviour, IFishState
{
    private FishBrain brain;

    [Header("fish intresst")]
    [SerializeField] private float IntresstDistanceToHook;
    [SerializeField] private float[] BiteWait;

    private Coroutine BiteC;

    [Header("Editor")]
    [SerializeField] private bool ShowGizmos;

    //state change
    private bool BiteState = false;

    void Awake()
    {
        brain = GetComponent<FishBrain>();
    }
    public void OnStateActivate()
    {
        SetRandomPosition();
    }
    public IFishState SwitchState()
    {
        if (BiteState)
        {
            ResetState();
            return brain.states.Biting;
        }
        else return this;
    }
    public void UpdateState()
    {
        // zet nieuwe End positie als vis positie gelijk staat aan end positie
        if (transform.position == brain.EndPos)
        {
            if (brain.FreePass && !brain.GetOriginSpawner().IsThisSpawnerActive)
            {
                brain.FreePass = false;
                FishPooler.instance.ReturnFish(brain);
                return;
            }
            else brain.FreePass = false;
            SetRandomPosition();
        }
        // zet nieuwe position als End position zero is
        if (brain.EndPos == Vector3.zero) { SetRandomPosition(); }
        if (Vector2.Distance(transform.position, Hook.instance.hook.transform.position) < IntresstDistanceToHook 
            && 
            FishPooler.instance.WaterBlock.bounds.Intersects(Hook.instance.bounds.bounds))
        {
            if (BiteC == null) BiteC = StartCoroutine(ChoseToBite());
        }

        // decelaration
        float movespeed = brain.moveSpeed;
        float dist = Vector2.Distance(transform.position, brain.EndPos);
        if (dist < 0.6f)
        {
            movespeed *= dist + 0.3f;
        }

        // move fish
        transform.position = Vector3.MoveTowards(transform.position, brain.EndPos, movespeed * Time.deltaTime);
    }   
    public void SetRandomPosition()
    {
        if (brain.GetOriginSpawner() == null) brain.SetEndPos(Vector3.zero);
        else brain.SetEndPos(brain.GetNewPosition());
    }
    private IEnumerator ChoseToBite()
    {
        float wait = Random.Range(BiteWait[0], BiteWait[1]);
        yield return new WaitForSeconds(wait);
        float br = brain.fishData.biteRate + FishUpgradeCheck.instance.BiteMultiply;
        br /= 10f;
        float randomvalue = Random.value;
        if (randomvalue < br && Hook.instance.FishOnHook == null && Hook.instance.IsFishAllowtToTarget())
        {
            BiteState = true;
        }
        BiteC = null;
    }
    public void OnEnable()
    {
        if (brain.EndPos == Vector3.zero) SetRandomPosition();
    }
    public void OnDisable()
    {
        ResetState();
    }
    public void ResetState()
    {
        BiteState = false;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (ShowGizmos) Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, IntresstDistanceToHook);
    }
#endif
}
