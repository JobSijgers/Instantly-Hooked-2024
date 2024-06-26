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
    [Header("Settings")]
    [SerializeField] private bool EnableGameSaving;
    [SerializeField] private bool AutoLoadGame;
    [SerializeField] private bool AutoSaving;
    [Tooltip("In secondes")]
    [SerializeField] private int saveAfterTime;
    private string Filename = "/GameSafe.json";
    private StorageCenter storageCenter = new StorageCenter();
    private List<InventorySave> GameSave = new List<InventorySave>();

    private Coroutine SavingC;
    private void Start()
    {
        if (AutoLoadGame && File.Exists(Application.persistentDataPath + Filename))
        {
            LoadGame();
            WriteLoad(LoadMode.start);
            StartCoroutine(LateStart());
        }
        if (AutoSaving) SavingC = StartCoroutine(AutoSaver());
    }
    void Update()
    {
        if (AutoSaving && SavingC == null) SavingC = StartCoroutine(AutoSaver());
    }
    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            string file = File.ReadAllText(Application.persistentDataPath + Filename);
            storageCenter = JsonUtility.FromJson<StorageCenter>(file);
        }
    }
    private void SafeGame()
    {
        WriteSave();
        string json = JsonUtility.ToJson(storageCenter,true);
        File.WriteAllText(Application.persistentDataPath + Filename, json);
    }
    private void DeleteFile()
    {
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            File.Delete(Application.persistentDataPath + Filename);
        }
    }
    private void WriteLoad(LoadMode load)
    {

        switch (load)
        {
            case LoadMode.start:
                // load upgrades 
                UpgradeManager.Instance.SetUpgrades(storageCenter.upgradeIndex);

                // load time
                TimeManager.instance.SetDay(storageCenter.currentDay -1);
                break;
            case LoadMode.late:
                //load fish
                foreach (InventorySave fishSave in storageCenter.inventory)
                {
                    Inventory.instance.AddFish(fishSave.FishData, fishSave.FishSize);
                }

                //load money
                EventManager.OnShopSell(storageCenter.Money);

                // load catalogue
                CatalogueTracker.instance.SetCatalogueNotes(storageCenter.Catalogue.totalCollectedFish, storageCenter.Catalogue.amountCaught);

                // load quests
                QuestTracker.instance.LoadQuests(storageCenter.Quests);
                break;

        }
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
        CatalogueTracker.instance.GetCurrentCatalogueNotes(out int totalFish, out int[] amountcollectedPF);
        storageCenter.Catalogue.totalCollectedFish = totalFish;
        storageCenter.Catalogue.amountCaught = amountcollectedPF;

        // quest 
        storageCenter.Quests = QuestTracker.instance.GetQuests();
    }
    private void OnApplicationQuit()
    {
        if (EnableGameSaving) SafeGame();
    }
    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        WriteLoad(LoadMode.late);
    }
    private IEnumerator AutoSaver()
    {
        yield return new WaitForSeconds(saveAfterTime);
        SafeGame();
        SavingC = null;
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

public enum LoadMode
{
    start,
    late
}


