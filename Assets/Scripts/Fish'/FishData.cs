using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Fish/Create Fish Data)", fileName = "FishData")]
public class FishData : ScriptableObject
{
    [Header("Economy")]
    public FishRarity fishRarity;
    public int minimumSellValue;
    public int maximumSellValue;

    [Header("Visual")] 
    public GameObject fishObject;
    
    [Header("Stats")]
    public float moveSpeed;
}