using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    public GameObject topBar; // link for top ui bar 

    private GameData gameData;
    private List<Transform> TopBarBtns = new List<Transform>(); // list with buttons, will fill in code

    private void Awake() 
    {
        gameData = GetComponent<GameData>();
    }
    void Start()
    {
        FillTopBar();
    }
    void FillTopBar()
    {
        foreach (Transform child in topBar.transform)
        {
            TopBarBtns.Add(child);
        }
        UpdateTopBar();
    }
    
    void UpdateTopBar()
    {
        TopBarBtns[0].GetComponentInChildren<Text>().text = "Gold\n" + gameData.resources.gold;
        TopBarBtns[1].GetComponentInChildren<Text>().text = "Wood\n" + gameData.resources.wood;
        TopBarBtns[2].GetComponentInChildren<Text>().text = "Stone\n" + gameData.resources.stone;
        TopBarBtns[3].GetComponentInChildren<Text>().text = "Food\n" + gameData.resources.food;
        TopBarBtns[4].GetComponentInChildren<Text>().text = "Copper\n" + gameData.resources.copper;
        TopBarBtns[5].GetComponentInChildren<Text>().text = "Citizen\n" + gameData.resources.citizen;
    }

    private void FixedUpdate() 
    {
        UpdateTopBar();
    }
    
}
