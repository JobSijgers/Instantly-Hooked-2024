using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public static Hook instance;
    public static FishBrain FishOnHook;
    public static GameObject hook;
    public static Material hookMat;
    [SerializeField] public LineRenderer fishline;
    [SerializeField] private Material P_BreakingHookMat;
    [SerializeField] private Material P_NormalHookMat;
    public  Material NormalHookMat { get { return P_NormalHookMat; }}
    public  Material BrrakingHookMat { get { return P_BreakingHookMat; } }
    private void Awake()
    {
        instance = this;
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
