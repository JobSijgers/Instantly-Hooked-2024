using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRoamingBehaviour : MonoBehaviour, IFishState   
{
    [SerializeField] private float moveTime;
    private Coroutine moveCoroutine;
    public IFishState switchState()
    {
        return this;
    }

    public void UpdateState()
    {
        if (moveCoroutine == null) StartCoroutine(MoveFishAsync());
    }

    private IEnumerator MoveFishAsync()
    {
        moveCoroutine = null;
        yield return null;
    }
}
