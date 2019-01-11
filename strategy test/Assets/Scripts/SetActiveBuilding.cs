using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveBuilding : MonoBehaviour
{
    public GameController gc;
    public int id;

    public void Set()
    {
        if (gc != null)
        {
            gc.chosenPrefabToBuild = gc.Buildings[id];
            gc.EnterBuildMode();
        }
    }
}
