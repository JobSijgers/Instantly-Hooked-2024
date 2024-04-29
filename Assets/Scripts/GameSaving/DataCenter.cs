using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class DataCenter : MonoBehaviour
{
    private string Filename = "/GameSafe.dat";
    private BinaryFormatter bf = new BinaryFormatter();
    private StorageCenter storageCenter = new StorageCenter();
    public int KurwaBaut;
    public int KurwaCaut;
    public Kurwatje kurwatje;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) SafeGame();
        if (Input.GetKeyDown(KeyCode.D)) DeleteFile();
        if (Input.GetKeyDown(KeyCode.L)) LoadGame();
        if (Input.GetKeyDown(KeyCode.O)) KurwaBaut++;
        if (Input.GetKeyDown(KeyCode.P)) KurwaCaut++;
        if (Input.GetKeyDown(KeyCode.I)) {KurwaBaut = 0; KurwaCaut = 0;}
    }
    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            Debug.Log("Load Game");
            FileStream file = File.Open(Application.persistentDataPath + Filename, FileMode.Open);
            storageCenter =  (StorageCenter)bf.Deserialize(file);
            KurwaBaut = storageCenter.StoreInt;
            KurwaCaut = storageCenter.Kurwatje.kurwaFish;
            file.Close();
        }
        Debug.Log($"KurwaBaut{KurwaBaut} : Kurwacaut{KurwaCaut}");
    }
    private void SafeGame()
    {
        Debug.Log("zaad");
        FileStream file;
        storageCenter.StoreInt = KurwaBaut;
        storageCenter.Kurwatje.kurwaFish = KurwaCaut;
        Debug.Log(kurwatje.kurwaFish);
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            file = File.Open(Application.persistentDataPath + Filename,FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + Filename);
        }
        Debug.Log(file.Name);
        bf.Serialize(file,storageCenter);
        file.Close();
    }
    private void DeleteFile()
    {
        if (File.Exists(Application.persistentDataPath + Filename))
        {
            File.Delete(Application.persistentDataPath + Filename);
        }
    }
}
[Serializable]
public struct Kurwatje
{
    public int kurwaFish;
}

[Serializable]
public class StorageCenter
{
    public int StoreInt;
    public Kurwatje Kurwatje;
}
