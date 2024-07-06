using System;
using System.Collections;
using Enums;
using Events;
using Fish;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [Serializable]
    public struct test
    {
        public FishData test1;
        public int amount;
    }

    [SerializeField] private test[] a;
    void Start()
    {
        StartCoroutine(A());
    }

    private IEnumerator A()
    {
        yield return new WaitForSeconds(2f);
        foreach (var COLLECTION in a)
        {
            for (int i = 0; i < COLLECTION.amount; i++)
            {
                EventManager.OnFishCaught(COLLECTION.test1, FishSize.Small);
            }
        }
    }
}