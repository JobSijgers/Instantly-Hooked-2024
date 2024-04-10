using Enums;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Fish/Create Fish Data)", fileName = "FishData")]
public class FishData : ScriptableObject
{
    public string fishName;
    
    [Header("Economy")]
    public FishRarity fishRarity;
    public int[] fishSellAmount;

    [Header("Visual")] 
    public GameObject fishObject;

    [Header("UI")] 
    public Sprite fishVisual;
     
    [Header("Stats")]
    public float moveSpeed;

    [Header("Inventory")] 
    public int maxStackAmount;
}