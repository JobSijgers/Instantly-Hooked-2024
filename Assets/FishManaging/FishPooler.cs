using Fish;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPooler : MonoBehaviour
{
    public static FishPooler instance;
    [SerializeField] private List<FishBrain> FishList = new List<FishBrain>();
    [SerializeField] private FishBrain fishPrefab;
    [SerializeField] private int MaxFishInList;
    private void Awake()
    {
        instance = this;
    }
    public FishBrain GetFish()
    {
        for (int i = 0; i < FishList.Count; i++)
        {
            if (!FishList[i].gameObject.activeInHierarchy)
            {
                return FishList[i];
            }
            else if (i == FishList.Count - 1)
            {
                FishBrain newfish = SpawnNewFish();
                FishList.Add(newfish);
                return newfish;
            }
        }
        // code zou niet null moeten berijken
        return null;
    }
    public void ReturnFish(FishBrain fish)
    {
        if (ToManyFishActive())
        {
            FishList.Remove(fish);
            Destroy(fish.gameObject);
        }
        fish.transform.SetParent(transform);
        fish.gameObject.transform.position = transform.position;
        fish.DestroyVisual();
        fish.gameObject.SetActive(false);
    }
    private bool ToManyFishActive()
    {
        int i = 0;
        foreach (FishBrain fish in FishList)
        {
            if (!fish.gameObject.activeInHierarchy) i++;
        }
        if (i > MaxFishInList) return true;
        else return false;
    }
    public FishBrain SpawnNewFish()
    {
        FishBrain newfish = Instantiate(fishPrefab, transform.position, Quaternion.identity);
        return newfish;
    }
}
