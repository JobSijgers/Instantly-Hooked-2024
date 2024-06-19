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
using GameTime;
using Catalogue;
using Upgrades;
using Quests;
using Quests.ScriptableObjects;

public class DataCenter : MonoBehaviour
{
    [Header("Commands")]
    [Header("/+1 save file")]
    [Header("/+2  load file")]
    [Header("/+3  delete file")]
    [Space(20)]
    [Header("Settings")]
    [SerializeField] private bool EnableGameSaving;
    [SerializeField] private bool AutoLoadGame; 
    [SerializeField] private bool DebugLogs;
    private string Filename = "/GameSafe.json";
    private StorageCenter storageCenter = new StorageCenter();
    private List<InventorySave> GameSave = new List<InventorySave>();
    private void Start()
    {
        if (AutoLoadGame) Invoke("LoadGame", 0.1f);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Slash))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SafeGame();
            if (Input.GetKeyDown(KeyCode.Alpha2)) LoadGame();
            if (Input.GetKeyDown(KeyCode.Alpha3)) DeleteFile();
        }
    }
    private void LoadGame()
    {
        if (DebugLogs) Debug.Log("Load Game");
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            if (DebugLogs) Debug.Log($"Found at {Application.persistentDataPath + Filename}");

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
        TimeManager.instance.SetDay(storageCenter.currentDay);

        // load upgrades 
        UpgradeManager.Instance.SetUpgrades(storageCenter.upgradeIndex);

        // load catalogue
        CatalogueTracker.Instance.SetCatalogueNotes(storageCenter.Catalogue.totalCollectedFish,storageCenter.Catalogue.amountCaught);

        // load quests
        QuestTracker.instance.LoadQuests(storageCenter.Quests);
    } 
    private void WriteSave()
    {
        // write fish
        List<InventoryItem> invitems = Inventory.instance.currentFish;
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
        TimeManager.instance.GetTimeState(out int currentday);
        storageCenter.currentDay = currentday;

        //upgrades
        storageCenter.upgradeIndex = UpgradeManager.Instance.GetUpgrades();

        // catalog
        CatalogueTracker.Instance.GetCurrentCatalogueNotes(out int totalFish, out int[] amountcollectedPF);
        storageCenter.Catalogue.totalCollectedFish = totalFish;
        storageCenter.Catalogue.amountCaught = amountcollectedPF;

        // quest 
        storageCenter.Quests = QuestTracker.instance.GetQuests();
    }
    private void OnApplicationQuit()
    {
        if (EnableGameSaving) SafeGame();
    }
}
[Serializable]
public class StorageCenter
{
    public List<InventorySave> inventory = new List<InventorySave>();
    public CatalogueSave Catalogue;
    public int[] upgradeIndex;
    public int currentDay;
    public int Money;
    public QuestProgress[] Quests;
}

[Serializable]
public struct InventorySave
{
    public FishData FishData;
    public int StackSize;
    public FishSize FishSize;
}
[Serializable]
public struct CatalogueSave
{
    public int totalCollectedFish;
    public int[] amountCaught; 
}


