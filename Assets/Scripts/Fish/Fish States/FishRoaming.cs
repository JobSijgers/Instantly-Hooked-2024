using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FishRoaming : MonoBehaviour, IFishState
{
    private FishBrain Brain;
    [SerializeField] private float RotateSpeed;
    [Tooltip("1 op ??")]
    [SerializeField] private int BiteChance;
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
        transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, Brain.moveSpeed * Time.deltaTime);
    }   
    public void SetRandomPosition()
    {
        Brain.SetEndPos(Brain.GetNewPosition());
        CancelInvoke();
    }
    private IEnumerator ChoseToBite()
    {
        //Debug.Log(BiteC);
        //Debug.Log("before wait");
        yield return new WaitForSeconds(BiteWait);
        //Debug.Log("after wait ");
        int RandomValue = Random.Range(1, BiteChance + 1);
        //Debug.Log($"random value : {RandomValue}");
        int bitevalue = 1;
        if (RandomValue == bitevalue && Hook.instance.FishOnHook == null)
        {
            Hook.instance.FishOnHook = Brain;
            BiteState = true;
        }
        BiteC = null;
        //Debug.Log(BiteC);
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
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.forward * 100);
    }
#endif
}
