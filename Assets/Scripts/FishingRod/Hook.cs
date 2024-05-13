using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public static Hook instance;
    public GameObject hook;
    public BoxCollider bounds;
    [SerializeField] public GameObject HookOrigin;
    [SerializeField] public LineRenderer fishline;

    private FishBrain P_FishOnHook;
    public FishBrain FishOnHook { get { return P_FishOnHook; } set { P_FishOnHook = value; } }
    private void Awake()
    {
        instance = this;
        hook = gameObject;
        bounds = gameObject.GetComponent<BoxCollider>();
    }
    private void Update()
    {
       //if (FishOnHook != null) Debug.Log(FishOnHook.gameObject);
    }
    public void RemoveFish()
    {
        FishOnHook = null;
    }
}
