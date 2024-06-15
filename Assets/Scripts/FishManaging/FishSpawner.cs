using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishSpawner : MonoBehaviour
{
    [Serializable]
    private struct FishToSpawn
    {
        public FishData fishData;
        public int amount;
    }

    [SerializeField] private Vector2 SpawnArea;
    [SerializeField] private float ActiveToBoatDistance;
    [SerializeField] private FishToSpawn[] FishTypesToSpawn;
    [SerializeField] private GameObject hook;

    private FishPooler fishPooler;
    private List<FishBrain> ActiveFish = new List<FishBrain>();
    private bool IsThisSpawnerActive = false;

    void Start()
    {
        fishPooler = FishPooler.instance;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, hook.transform.position) < ActiveToBoatDistance &&
            !IsThisSpawnerActive && Hook.instance.hook.gameObject.activeInHierarchy)
        {
            ActivateThisSpwner();
        }
        else if (Vector2.Distance(transform.position, hook.transform.position) > ActiveToBoatDistance &&
                 IsThisSpawnerActive)
        {
            DeactivateThisSpwaner();
        }
        else if (!Hook.instance.hook.activeInHierarchy) DeactivateThisSpwaner();
    }

    private void ActivateThisSpwner()
    {
        IsThisSpawnerActive = true;
        SpawnFish();
    }

    private void DeactivateThisSpwaner()
    {
        IsThisSpawnerActive = false;
        RemoveFish();
    }

    private void RemoveFish()
    {
        foreach (FishBrain fish in ActiveFish)
        {
            if (fish == null)
            {
                continue;
            }

            if (Hook.instance.FishOnHook == null)
            {
                fishPooler.ReturnFish(fish);
            }
            else if (fish.gameObject != Hook.instance.FishOnHook.gameObject)
            {
                fishPooler.ReturnFish(fish);
            }
        }

        ActiveFish.Clear();
    }

    private void SpawnFish()
    {
        foreach (FishToSpawn fishType in FishTypesToSpawn)
        {
            for (int i = 0; i < fishType.amount; i++)
            {
                FishBrain fish = fishPooler.GetFish();
                fish.SetOriginSpawner(this);
                fish.Initialize(fishType.fishData, GetRandomFishSize());
                fish.transform.position = GetRandomPos();
                fish.transform.SetParent(transform);
                ActiveFish.Add(fish);
                fish.gameObject.SetActive(true);
            }
        }
    }

    private FishSize GetRandomFishSize()
    {
        Array fish = Enum.GetValues(typeof(FishSize));
        return (FishSize)fish.GetValue(Random.Range(0, fish.Length));
    }

    public Vector3 GetRandomPos()
    {
        Vector3 pos = new Vector3()
        {
            x = Random.Range(transform.position.x - SpawnArea.x / 2, transform.position.x + SpawnArea.x / 2),
            y = Random.Range(transform.position.y - SpawnArea.y / 2, transform.position.y + SpawnArea.y / 2),
            z = transform.position.z,
        };
        return pos;
    }
#if UNITY_EDITOR
    [Header("Editor Gizmos")] [SerializeField]
    private Color color = Color.green;
    [SerializeField] private bool ShowActivateRange = true;
    [SerializeField] private bool ShowBounds = true;
    [SerializeField] private bool ShowFishNames = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (ShowBounds) Gizmos.DrawWireCube(transform.position, SpawnArea);
        Handles.color = color;
        if (ShowActivateRange) Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, ActiveToBoatDistance);
        int i = 0;

        if (ShowFishNames)
        {
            foreach (FishToSpawn type in FishTypesToSpawn)
            {
                if (type.fishData == null)
                    return;
                i++;
                Vector3 pos = transform.position;
                pos.y += i * 2;
                Handles.Label(pos, type.fishData.fishName);
            }
        }
    }
#endif
}