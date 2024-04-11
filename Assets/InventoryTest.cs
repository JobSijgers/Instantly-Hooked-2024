using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Fish;
using Player.Inventory;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    [SerializeField] private FishData[] testfish;
    [SerializeField] private int testfishamount;

    private void Start()
    {
        foreach (var a in testfish)
        {
            for (int i = 0; i < testfishamount; i++)
            {
                Inventory.Instance.AddFish(a, FishSize.Small);
            }

            for (int i = 0; i < testfishamount; i++)
            {
                Inventory.Instance.AddFish(a, FishSize.Medium);
            }

            for (int i = 0; i < testfishamount; i++)
            {
                Inventory.Instance.AddFish(a, FishSize.Large);
            }
        }
    }
}
