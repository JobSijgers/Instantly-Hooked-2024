using System.Collections;
using System.Collections.Generic;
using Economy.ShopScripts;
using Enums;
using Events;
using Fish;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private FishData test;
    public int amount;
    void Start()
    {
        StartCoroutine(A());
    }

    private IEnumerator A()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < amount; i++)
        {
            EventManager.OnFishCaught(test, FishSize.Small);
        }
    }
}
