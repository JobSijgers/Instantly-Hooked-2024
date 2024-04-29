using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Random = UnityEngine.Random;

[CustomEditor(typeof(FishSpawner))]
public class EditorFishSpawner : Editor
{
    private void OnSceneGUI()
    {
        //FishSpawner spawner = (FishSpawner)target;

        Handles.color = Color.green;
        //Handles.DrawWireCube(spawner.transform.position,spawner.SpawnArea);
    }
    private Color GenerateColor()
    {
        Color color = new Color()
        {
            r = Random.Range(1,255 + 1), 
            g = Random.Range(1, 255 + 1), 
            b = Random.Range(1, 255 + 1),
        };
        return color;
    }
}
