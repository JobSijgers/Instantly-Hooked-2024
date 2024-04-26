using System;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Fish
{
    [CreateAssetMenu(menuName = "Fish/Create Fish Data)", fileName = "FishData")]
    public class FishData : ScriptableObject
    {
        
        public string fishName;
        public string fishDescription;
        public string habitat;
        
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
    
    
}