using Fish;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
public class FishSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 SpawnArea;
    [SerializeField] private float ActiveToBoatDistance;
    [SerializeField] private FishData[] FishTypesToSpawn;
    [SerializeField] private int FishInArea;
    [SerializeField] private GameObject hook;

    private FishPooler fishPooler;
    private List<FishBrain> ActiveFish = new List<FishBrain>();
    private bool IsThisSpawnerActive = false;

    public Vector2 GetSpawnBunds() => SpawnArea;

    void Start()
    {
        fishPooler = FishPooler.instance;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, hook.transform.position) < ActiveToBoatDistance && !IsThisSpawnerActive && Hook.instance.hook.gameObject.activeInHierarchy)
        {
            ActivateThisSpwner();
        }
        else if (Vector2.Distance(transform.position, hook.transform.position) > ActiveToBoatDistance && IsThisSpawnerActive)
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
        for (int i = 0; i < FishInArea; i++)
        {
            FishBrain fish = fishPooler.GetFish();
            fish.SetOriginSpawner(this);
            fish.fishData = FishTypesToSpawn[Random.Range(0, FishTypesToSpawn.Length)];
            fish.transform.position = GetRandomPos();
            fish.transform.SetParent(transform);
            ActiveFish.Add(fish);
            fish.gameObject.SetActive(true);
        }
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
    [Header("Editor Gizmos")]
    [SerializeField] private Color color = Color.green;
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, SpawnArea);
        Handles.color = color;
        Handles.DrawWireArc(transform.position,Vector3.forward,Vector3.up,360,ActiveToBoatDistance);
        int i = 0;
        foreach (FishData fish in FishTypesToSpawn)
        {
            i++;
            Vector3 pos = transform.position;
            pos.y += i * 2;
            Handles.Label(pos, fish.fishName);
        }
    }
#endif
}

