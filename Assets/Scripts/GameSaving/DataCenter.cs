using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Enums;
using Fish;
using Player.Inventory;

public class DataCenter : MonoBehaviour
{
    [SerializeField] private bool DebugLogs;
    private string Filename = "/GameSafe.dat";
    private BinaryFormatter bf = new BinaryFormatter();
    private StorageCenter storageCenter = new StorageCenter();
    private List<InventorySave> GameSave = new List<InventorySave>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SafeGame();
        if (Input.GetKeyDown(KeyCode.Alpha2)) LoadGame();
        if (Input.GetKeyDown(KeyCode.Alpha3)) DeleteFile();
    }
    private void LoadGame()
    {
        if (DebugLogs) Debug.Log("Load Game");
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            if (DebugLogs) Debug.Log($"Debug Found at {Application.persistentDataPath + Filename}");

            FileStream file = File.Open(Application.persistentDataPath + Filename, FileMode.Open);
            storageCenter = (StorageCenter)bf.Deserialize(file);
            file.Close();
            WriteLoad();
        }
        else if (DebugLogs) Debug.Log("No File Found.");
    }
    private void SafeGame()
    {
        WriteSave();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            if (DebugLogs) Debug.Log($"file exist at {Application.persistentDataPath + Filename}");
            file = File.Open(Application.persistentDataPath + Filename,FileMode.Open);
        }
        else
        {
            if (DebugLogs) Debug.Log($"file created at {Application.persistentDataPath + Filename}");
            file = File.Create(Application.persistentDataPath + Filename);
        }
        bf.Serialize(file,storageCenter);
        file.Close();
    }
    private void DeleteFile()
    {
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            if (DebugLogs) Debug.Log("file has been deleted");
            File.Delete(Application.persistentDataPath + Filename);
        }
        else if (DebugLogs) Debug.Log("no file exist to delete");
    }
    private void WriteLoad()
    {
        List<InventoryItem> inv = new List<InventoryItem>();
        foreach (InventorySave fishSave in storageCenter.GameSave)
        {
            InventoryItem fish = new InventoryItem();
            Color bColor = new Color()
            {
                r = fishSave.Color[0],
                g = fishSave.Color[1],
                b = fishSave.Color[2]
            };
            fish.Initialize(fishSave.FishData, fishSave.FishSize, bColor);
            fish.SetStackSize(fishSave.StackSize);
            inv.Add(fish);
        }
        Inventory.Instance.currentFish = inv;   
    } 
    private void WriteSave()
    {
        List<InventoryItem> invitems = Inventory.Instance.currentFish;
        foreach (InventoryItem fish in invitems)
        {
            string json = JsonUtility.ToJson(fish.GetFishData());
            Debug.Log(json);
            InventorySave fishSave = new InventorySave();
            fishSave.Color = new float[2];
            fishSave.FishSize = fish.GetFishSize();
            fishSave.StackSize = fish.GetStackSize();
            fishSave.FishData = fish.GetFishData();

            Color color = fish.GetColor();
            for (int i = 0; i < 3; i++)
            {
                Debug.Log("loop" + i);
                fishSave.Color[i] = color[i];   
            }
            GameSave.Add(fishSave);
        }

        storageCenter.GameSave = GameSave;
    }
}
[Serializable]
public class StorageCenter
{
    public List<InventorySave> GameSave = new List<InventorySave>();
}

[Serializable]
public struct InventorySave
{
    public FishData FishData;
    public int StackSize;
    public FishSize FishSize;
    public float[] Color;
}


