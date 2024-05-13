using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;

public class FishRoaming : MonoBehaviour, IFishState
{
    private FishBrain Brain;
    [SerializeField] private float RotateSpeed;
    [SerializeField] private float IntresstDistanceToHook;
    [SerializeField] private float BiteWait;
    private Coroutine BiteC;

    //state change
    private bool BiteState = false;
    void Awake()
    {
        Brain = GetComponent<FishBrain>();  
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
            return Brain.states.Biting;
        }
        else return this;
    }
    public void UpdateState()
    {
        if (transform.position == Brain.EndPos)
        {
             SetRandomPosition();
        }
        if (Vector2.Distance(transform.position, Hook.instance.hook.transform.position) < IntresstDistanceToHook 
            && 
            FishPooler.instance.WaterBlock.bounds.Intersects(Hook.instance.bounds.bounds))
        {
            if (BiteC == null) BiteC = StartCoroutine(ChoseToBite());
        }
        float movespeed = Brain.moveSpeed;
        float dist = Vector2.Distance(transform.position, Brain.EndPos);
        if (dist < 0.6f)
        {
            movespeed *= dist + 0.3f;
        }
        transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, movespeed * Time.deltaTime);
    }   
    public void SetRandomPosition()
    {
        Brain.SetEndPos(Brain.GetNewPosition());
    }
    private IEnumerator ChoseToBite()
    {
        yield return new WaitForSeconds(BiteWait);
        int RandomValue = Random.Range(1, Brain.fishData.biteRate + 1);
        int bitevalue = 1;
        if (RandomValue == bitevalue && Hook.instance.FishOnHook == null)
        {
            Hook.instance.FishOnHook = Brain;
            BiteState = true;
        }
        BiteC = null;
    }
    public void OnEnable()
    {
        if (Brain.EndPos == Vector3.zero) SetRandomPosition();
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
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, IntresstDistanceToHook);
    }
#endif
}
