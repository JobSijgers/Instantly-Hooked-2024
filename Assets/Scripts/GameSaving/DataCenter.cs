using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Enums;
using Fish;
using Player.Inventory;
using Events;
using Economy;
using Timer;

public class DataCenter : MonoBehaviour
{
    [SerializeField] private bool DebugLogs;
    private string Filename = "/GameSafe.json";
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

            string file = File.ReadAllText(Application.persistentDataPath + Filename);
            storageCenter = JsonUtility.FromJson<StorageCenter>(file);
            WriteLoad();
        }
        else if (DebugLogs) Debug.Log("No File Found.");
    }
    private void SafeGame()
    {
        WriteSave();
        string json = JsonUtility.ToJson(storageCenter,true);
        File.WriteAllText(Application.persistentDataPath + Filename, json);
        Debug.Log($"Json stored at {Application.persistentDataPath + Filename}");
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
        foreach (InventorySave fishSave in storageCenter.inventory)
        {
            EventManager.OnFishCaught(fishSave.FishData, fishSave.FishSize);
        }

        //load money
        EventManager.OnShopSell(storageCenter.Money);

        // load time
        TimeManager.instance.SetTime(storageCenter.Time.currentDay, storageCenter.Time.currentTime);
    } 
    private void WriteSave()
    {
        // write fish
        List<InventoryItem> invitems = Inventory.Instance.currentFish;
        foreach (InventoryItem fish in invitems)
        {
            InventorySave fishSave = new InventorySave();
            fishSave.FishSize = fish.GetFishSize();
            fishSave.StackSize = fish.GetStackSize();
            fishSave.FishData = fish.GetFishData();
            GameSave.Add(fishSave);
        }
        storageCenter.inventory = GameSave;

        // write money
        storageCenter.Money = EconomyManager.instance.GetCurrentMoneyAmount();

        // write time
        TimeManager.instance.GetTimeState(out int currentday, out float currenttime);
        storageCenter.Time.currentDay = currentday;
        storageCenter.Time.currentTime = currenttime;

        //upgrades
        //EventManager.On
    }
}
[Serializable]
public class StorageCenter
{
    public List<InventorySave> inventory = new List<InventorySave>();
    public TimeSave Time;
    public UpgradeSave Ubgrades;
    public int Money;
}

[Serializable]
public struct InventorySave
{
    public FishData FishData;
    public int StackSize;
    public FishSize FishSize;
}
[Serializable]
public struct UpgradeSave
{

}
[Serializable]
public struct TimeSave
{
    public int currentDay;
    public float currentTime;
}



