using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tutorial
{
    [CreateAssetMenu(fileName = "TutorialData", menuName = "Tutorial/Data", order = 0)]
    public class TutorialData : ScriptableObject
    {
        [Serializable]
        public struct Page
        {
            public string description;
            public Sprite image;
            public Vector2 imageSize;
            public string sound;
        }
        public Page[] pages;
    }
}