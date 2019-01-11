using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class PlayerData
{
    public string[] objectsData;
    public int gold;
    public int wood;
}

public class GameData : MonoBehaviour
{

    public static GameData gameData;
    
    // stats
    [Header("Game Settings")]
    public int startGold = 0;

    [Header("Game stats")]
    public int gold;
    public int wood;
    
    [Header("Game stats")]
    public List<GameObject> AvailableBsuildings = new List<GameObject>();

    public List<Building> bldOnScene;    

    // data save to file
    public void SaveData(PlayerData data)
    {
        // load file if exist
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        // save it
        bf.Serialize(file, data);
        file.Close();
    }

    void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            gold = data.gold;
            wood = data.wood;
            
            // trash with gamecontroller
            GameController gc = GameObject.Find("GameControllerObject").GetComponent<GameController>();

			gc.WipeScene();

            foreach (string s in data.objectsData)
            {
                SerializableObject so = JsonUtility.FromJson<SerializableObject>(s);
                gc.PlaceObjectfromSO(so);
            }
        }
    }

    public void LoadGame()
    {
        LoadData();        
    }

    public void SaveGame()
    {     
        // create new player data to serialize
        PlayerData data = new PlayerData();

        // create new array of objectsData with strings from buildings on scene list
        data.objectsData = new string[bldOnScene.Count];

        // itereate thru each one
        for (int i = 0; i < bldOnScene.Count; i++)
        {
            data.objectsData[i] = JsonUtility.ToJson(bldOnScene[i].ObjectSaveData(),true);
        }

        // set game data to savedata
        data.gold = gold;
        data.wood = wood;

        SaveData(data);
    }
}

