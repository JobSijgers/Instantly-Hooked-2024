using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager instance;
    public MeshRenderer waterMesh;
    public Bobber bobber;
    private void Awake()
    {
        instance = this;
    }
    private void SpawnFish()
    {

    }
}
