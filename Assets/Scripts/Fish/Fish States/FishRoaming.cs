using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Events;
public class FishRoaming : MonoBehaviour, IFishState
{
    private FishBrain Brain;

    [Header("fish intresst")]
    [SerializeField] private float IntresstDistanceToHook;
    [SerializeField] private float BiteWait;
    private float biteMultiply;

    private Coroutine BiteC;

    [Header("Editor")]
    [SerializeField] private bool ShowGizmos;

    //state change
    private bool BiteState = false;

    //propeties
    public float BiteMultiply { set { biteMultiply = value; } }

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
        // zet nieuwe End positie als vis positie gelijk staat aan end positie
        if (transform.position == Brain.EndPos)
        {
             SetRandomPosition();
        }
        // zet nieuwe position als End position zero is
        if (Brain.EndPos == Vector3.zero) { SetRandomPosition(); }
        if (Vector2.Distance(transform.position, Hook.instance.hook.transform.position) < IntresstDistanceToHook 
            && 
            FishPooler.instance.WaterBlock.bounds.Intersects(Hook.instance.bounds.bounds))
        {
            if (BiteC == null) BiteC = StartCoroutine(ChoseToBite());
        }

        // decelaration
        float movespeed = Brain.moveSpeed;
        float dist = Vector2.Distance(transform.position, Brain.EndPos);
        if (dist < 0.6f)
        {
            movespeed *= dist + 0.3f;
        }

        // move fish
        transform.position = Vector3.MoveTowards(transform.position, Brain.EndPos, movespeed * Time.deltaTime);
    }   
    public void SetRandomPosition()
    {
        if (Brain.GetOriginSpawner() == null) Brain.SetEndPos(Vector3.zero);
        else Brain.SetEndPos(Brain.GetNewPosition());
    }
    private IEnumerator ChoseToBite()
    {
        yield return new WaitForSeconds(BiteWait);
        float br = Brain.fishData.biteRate + biteMultiply;
        br /= 10f;
        float randomvalue = Random.value;
        if (randomvalue < br && Hook.instance.FishOnHook == null)
        {
            //Hook.instance.FishOnHook = Brain;
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
        if (ShowGizmos) Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, IntresstDistanceToHook);
    }
#endif
}
