using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class FishRoaming : MonoBehaviour, IFishState
{
    private FishBrain Brain;
    private Vector3 EndPos;
    [SerializeField] private float RotateSpeed;
    [Range(0,1f)]
    [SerializeField] private float BiteChance;
    [SerializeField] private float IntresstDistanceToHook;
    [Tooltip("needs to values")]
    [SerializeField] private float[] BiteWait;
    private Coroutine BiteC;
    private bool Bite;
    void Awake()
    {
        Brain = GetComponent<FishBrain>();  
    }
    public IFishState SwitchState()
    {
        if (Bite) return Brain.states.Biting;
        else return this;
    }
    public void UpdateState()
    {
        if (Brain.NavAgent.remainingDistance <= Brain.NavAgent.stoppingDistance)
        {
            SetNewRandomNavPos();
        }

        Vector3 Dir = transform.position - EndPos;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-Dir), RotateSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, Brain.Hook.transform.position) < IntresstDistanceToHook)
        {
            if (BiteC == null) StartCoroutine(WaitTopBite());
        }
    }   
    public void SetNewRandomNavPos()
    {
        if (RandomPoint(out Vector2 point))
        {
            if (point != Vector2.zero)
            {
                Brain.NavAgent.SetDestination(point);
                EndPos = point;
            }
        }
    }
    private bool RandomPoint(out Vector2 point)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(Brain.GetNewPos(), out hit, 2.0f, NavMesh.AllAreas))
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

    private IEnumerator WaitTopBite()
    {
        float waittime = Random.Range(BiteWait[0], BiteWait[1]);
        float RandomValue = Random.value;
        Debug.Log($"randomvalue {RandomValue}   fish{this.name}   waittime {waittime}");
        if (RandomValue < BiteChance) Bite = true;
        yield return new WaitForSeconds(waittime);
        BiteC = null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, IntresstDistanceToHook);
    }
#endif
}
