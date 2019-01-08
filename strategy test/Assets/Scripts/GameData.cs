using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameData : MonoBehaviour
{

    public static GameData gameData;
    
    // stats
    [Header("Game Settings")]
    public int startGold = 0;

    [Header("Game stats")]
    public int gold;
    public int wood;
    
    public List<SerializableObject> buildingsOnScene;
    public SerializableObject[] buildingsOnSceneArray;

    // data save to file
    public void SaveData()
    {
        // load file if exist
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        // create new savedata
        PlayerData data = new PlayerData();

        // set game data to savedata
        data.gold = gold;
        data.wood = wood;

        // convert list to array
        buildingsOnSceneArray = new SerializableObject[buildingsOnScene.Count];
        for (int i = 0; i < buildingsOnScene.Count; i++)
        {
            buildingsOnSceneArray[i] = buildingsOnScene[i];
        }
        data.buildingsOnSceneArray = buildingsOnSceneArray;

        // save it
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            gold = data.gold;
            wood = data.wood;

            // convert array to list
            buildingsOnScene = new List<SerializableObject>();
            for (int i = 0; i < buildingsOnSceneArray.Length; i++)
            {
                buildingsOnScene.Add(buildingsOnSceneArray[i]);
            }
            buildingsOnSceneArray = data.buildingsOnSceneArray;
        }
    }

    // Start is called before the first frame update
    
    private void Awake() 
    {
       if (!gameData)
       {
           DontDestroyOnLoad(gameObject);
           gameData = this;
       } 
       else if (gameData == this)
       {
           Destroy(gameObject);
       }
    }

    private void OnGUI() {
        GUI.Label(new Rect(10,10,100,30), "Gold: " + gold);
    }
}

[Serializable]
class PlayerData
{
    public SerializableObject[] buildingsOnSceneArray;
    public int gold;
    public int wood;
}