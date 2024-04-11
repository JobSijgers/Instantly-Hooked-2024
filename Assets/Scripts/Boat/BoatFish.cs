using Player.Inventory;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEditor.PackageManager;
using UnityEngine;

public class BoatFish : MonoBehaviour
{
    public static BoatFish Instance;
    private Inventory inventory;

    public delegate void BoatFishDelegate();
    public event BoatFishDelegate OnFishCaught;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        inventory = Inventory.instance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Bobber bobber))
        {
            FishBrain fish = bobber.GetComponentInChildren<FishBrain>();
            if (fish == null) return;
            inventory.AddFish(fish.data, FishSize.Small);
            Destroy(fish.gameObject);
            OnFishCaught?.Invoke();
        }
    }
}
