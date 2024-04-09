using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish/Create Fish Data)", fileName = "FishData")]
public class FishData : ScriptableObject
{
    [Header("Economy")]
    public FishRarity fishRarity;
    public int minimumSellValue;
    public int maximumSellValue;

    [Header("Visual")] 
    public GameObject fishObject;

    [Header("UI")] 
    public Sprite fishVisual;
     
    [Header("Stats")]
    public float moveSpeed;
}