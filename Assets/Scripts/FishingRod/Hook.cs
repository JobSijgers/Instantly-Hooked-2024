using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public static Hook instance;
    public static GameObject hook;
    public static Material hookMat;
    [SerializeField] public GameObject HookOrigin;
    [SerializeField] public LineRenderer fishline;
    [Header("laat leeg")]
    public FishBrain FishOnHook;
    private void Awake()
    {
        instance = this;
        hook = gameObject;
    }
    private void Update()
    {
       if (FishOnHook != null) Debug.Log(FishOnHook.gameObject);
    }
    public bool HasFish()
    {
        if (FishOnHook == null) return false;
        else return true;  
    }
    public void RemoveFish()
    {
        FishOnHook = null;
    }
}
