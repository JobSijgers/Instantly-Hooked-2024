using Player;
using System.Collections;
using System.Collections.Generic;
using Player.Inventory;
using UnityEngine;
using Enums;
using System;

public class BoatFish : MonoBehaviour
{
    [SerializeField] private Bobber bobber;

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
        inventory = Inventory.Instance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Bobber bobber))
        {
            FishBrain fish = bobber.GetComponentInChildren<FishBrain>();
            if (fish == null) return;
            inventory.AddFish(fish.data, GetRandomFishSize());
            bobber.transform.parent = this.transform;
            Destroy(fish.gameObject);
            OnFishCaught?.Invoke();
        }
    }
    private FishSize GetRandomFishSize()
    {
        var fish = Enum.GetValues(typeof(FishSize));
        return (FishSize)fish.GetValue(UnityEngine.Random.Range(0, fish.Length));
    }

}
