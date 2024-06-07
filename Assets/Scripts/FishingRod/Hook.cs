using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Hook : MonoBehaviour
{
    public static Hook instance;
    public GameObject hook;
    public BoxCollider bounds;
    [SerializeField] public GameObject HookOrigin;
    [SerializeField] public LineRenderer fishline;

    [Tooltip("fix de Rod MaxLenght value * Offset")]
    public float Offset;

    public FishingRod.FishingRod Rod;
    private FishBrain P_FishOnHook;

    public FishBrain FishOnHook
    {
        get { return P_FishOnHook; }
        set { P_FishOnHook = value; }
    }

    public bool touchingGround = false;

    private void Awake()
    {
        instance = this;
        hook = gameObject;
        bounds = gameObject.GetComponent<BoxCollider>();
    }

    public void Update()
    {
        if (FishOnHook == null)
        {
            Physics.gravity = new Vector3(0, -60f, 0);
        }
        else
        {
            
            Physics.gravity = FishOnHook.states.Biting.CurrentState == FishBitingState.struggeling
                ? new Vector3(0, -9.81f, 0) :
                new Vector3(0, -60f, 0);
        }
    }

    public void RemoveFish()
    {
        FishOnHook = null;
    }
    public void ResetRodColor()
    {
        Hook.instance.fishline.startColor = Color.white;
        Hook.instance.fishline.endColor = Color.white;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            touchingGround = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            touchingGround = false;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireArc(Rod.transform.position, Vector3.forward, Vector3.down, 360, Rod.GetLineLength() * Offset);
    }
#endif
}