using System;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Fish
{
    [CreateAssetMenu(menuName = "Fish/Create Fish Data)", fileName = "FishData")]
    [Serializable]
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
        public float[] fishSize;

        [Header("UI")]
        public Sprite fishVisual;

        [Header("Stats")] 
        public float moveSpeed;

        [Header("Inventory")] 
        public int maxStackAmount;

        [Header("Bite")] [Range(1,10)]
        public int biteRate;


        public Vector3 GetScale(FishSize size)
        {
            if (fishSize.Length == Enum.GetNames(typeof(FishSize)).Length) 
                return fishSize[(int)size] * Vector3.one;
            
            Debug.LogWarning("<color=#f00>Fish siz e array is not the correct size in: " + fishName + " Defaulting to 1</color>");
            return Vector3.one;
        }
    }
}