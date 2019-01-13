using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveBuilding : MonoBehaviour
{
    public GameController gc;
    public GameData gameData;
    public int id;

    public void Set()
    {
        if (gc != null)
        {
            gc.chosenPrefabToBuild = gameData.availableBuildings[id];
            gc.EnterBuildMode();
        }
    }
}
