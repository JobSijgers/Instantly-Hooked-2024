using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    public FishBrain FishOnHook { get { return P_FishOnHook; } set { P_FishOnHook = value; } }
    private void Awake()
    {
        instance = this;
        hook = gameObject;
        bounds = gameObject.GetComponent<BoxCollider>();
    }
    public void RemoveFish()
    {
        FishOnHook = null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawWireArc(Rod.transform.position, Vector3.forward, Vector3.down, 360, Rod.GetLineLength() * Offset);

    }
#endif
}
