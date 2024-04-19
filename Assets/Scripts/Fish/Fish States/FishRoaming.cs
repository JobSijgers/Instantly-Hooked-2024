using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishRoaming : MonoBehaviour, IFishState
{
    private FishBrain Brain;
    private Vector3 EndPos;
    public IFishState SwitchState()
    {
        return this;
    }

    public void UpdateState()
    {
        if (Brain.NavAgent.remainingDistance <= Brain.NavAgent.stoppingDistance)
        {
            SetNewRandomNavPos();
        }
    }

    void Start()
    {
        Brain = GetComponent<FishBrain>();  
    }
    public void SetNewRandomNavPos()
    {
        if (RandomPoint(out Vector2 point))
        {
            if (point != Vector2.zero) Brain.NavAgent.SetDestination(point);
        }
    }
    private bool RandomPoint(out Vector2 point)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(Brain.GetNewPos(), out hit, 1.0f, NavMesh.AllAreas))
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
}
