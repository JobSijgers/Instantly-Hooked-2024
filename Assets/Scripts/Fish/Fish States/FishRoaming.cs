using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class FishRoaming : MonoBehaviour, IFishState
{
    private FishBrain Brain;
    public Vector3 EndPos;
    [SerializeField] private float RotateSpeed;
    [Tooltip("1% == 1/10")]
    [Range(0,1f)]
    [SerializeField] private float BiteChance;
    private float FishBitePercentage = 0.1f;
    [SerializeField] private float IntresstDistanceToHook;
    [Tooltip("needs two values")]
    [SerializeField] private float[] BiteWait;
    private Coroutine BiteC;

    //state change
    private bool Bite = false;
    void Awake()
    {
        Brain = GetComponent<FishBrain>();  
    }
    public void OnStateActivate()
    {
        SetNewRandomNavPos();
    }
    public IFishState SwitchState()
    {
        if (Bite && Brain.NavAgent.remainingDistance < Brain.NavAgent.stoppingDistance)
        {
            ResetState();
            return Brain.states.Biting;
        }
        else return this;
    }
    public void UpdateState()
    {
        if (Brain.NavAgent.remainingDistance <= Brain.NavAgent.stoppingDistance && !Bite)
        {
             SetNewRandomNavPos();
        }

        Vector3 Dir = transform.position - EndPos;
        transform.LookAt(EndPos);
        Vector3 FixedRot = transform.eulerAngles;
        FixedRot.z = 0;
        transform.eulerAngles = FixedRot;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-Dir), RotateSpeed * Time.deltaTime);

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
        if (NavMesh.SamplePosition(Brain.GetNewPosition(), out hit, 2.0f, NavMesh.AllAreas))
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
        if (RandomValue < BiteChance * FishBitePercentage && Hook.instance.FishOnHook == null)
        {
            Bite = true;
        }
        yield return new WaitForSeconds(waittime);
        BiteC = null;
    }
    public void ResetState()
    {
        Bite = false;
    }
    public void OnDisable()
    {
        ResetState();
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
