using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class PlayerData
{
    public string[] userCreatedObjects;
    public string[] generatedObjects;
    public int gold;
    public int wood;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    
    // stats
    [Header("Game Settings")]
    public Resources startingResources;

    [Header("Game stats")]
    public Resources resources = new Resources();
    
    [Header("Game setup")]
    public List<GameObject> availableBuildings = new List<GameObject>();
    public List<GameObject> availableObjects = new List<GameObject>();
    public List<PlaceableObject> userCreatedObjects;    
    public List<PlaceableObject> generatedObjects;

    public GameObject userCreatedObjectsGO;
    public GameObject generatedObjectsGO;

    private void Awake() 
    {
        resources = startingResources;
    }
    private void Start()
    {
        
        userCreatedObjectsGO = UserCreatedObjectsGO();
        generatedObjectsGO = GeneratedObjectsGO();
    }

    private GameObject UserCreatedObjectsGO()
    {
        GameObject go = new GameObject();
        go.name = "userCreatedObjects";

        return go;
    }
    private GameObject GeneratedObjectsGO()
    {
        GameObject go = new GameObject();
        go.name = "generatedObjects";

        return go;
    }

    // data save to file
    public void SaveData(PlayerData data)
    {
        // load file if exist
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        // save it
        bf.Serialize(file, data);
        file.Close();

        GetComponent<Notification>().SetNotification("Game Saved");
    }

    void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            resources.gold = data.gold;
            resources.wood = data.wood;
            
            // trash with gamecontroller
            GameController gc = GetComponent<GameController>();

			gc.WipeScene();

            foreach (string s in data.userCreatedObjects)
            {
                SerializableObject so = JsonUtility.FromJson<SerializableObject>(s);
                gc.PlaceBuildingfromSO(so);
            }
            foreach (string s in data.generatedObjects)
            {
                SerializableObject so = JsonUtility.FromJson<SerializableObject>(s);
                gc.PlaceObjectfromSO(so);
            }
            GetComponent<Notification>().SetNotification("Game Loaded");
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
        data.userCreatedObjects = new string[userCreatedObjects.Count];
        data.generatedObjects = new string[generatedObjects.Count];

        // itereate thru each one
        for (int i = 0; i < userCreatedObjects.Count; i++)
        {
            data.userCreatedObjects[i] = JsonUtility.ToJson(userCreatedObjects[i].ObjectSaveData(),true);
        }
        // itereate thru each one
        for (int i = 0; i < generatedObjects.Count; i++)
        {
            data.generatedObjects[i] = JsonUtility.ToJson(generatedObjects[i].ObjectSaveData(),true);
        }

        foreach (string s in data.generatedObjects)
        {
            print(s);
        }

        // set game data to savedata
        data.gold = resources.gold;
        data.wood = resources.wood;

        SaveData(data);
    }

    public void AddResources(Resources res)
    {
        resources = resources + res;
    }
}

