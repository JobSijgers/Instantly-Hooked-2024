using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public static FishBrain FishOnHook;
    public static GameObject hook;
    private void Awake()
    {
        hook = gameObject;
    }
    private void Update()
    {
       if (FishOnHook != null) Debug.Log(FishOnHook.gameObject);
    }
    public static bool HasFish()
    {
        if (FishOnHook == null) return false;
        else return true;  
    }
    public static void RemoveFish()
    {
        FishOnHook = null;
    }
}
