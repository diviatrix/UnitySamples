using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct Position 
    {
        public float x;
        public float y;
        public float z;
    }

[System.Serializable]
public struct SerializableObject
{
    public string name;
    public string prefabName;
    public Position position;
}

public class GameController : MonoBehaviour
{
    [Header("Object bindings")]
    public GameObject framePrefab;
    public GameObject buildFramePrefab;
    public List<GameObject> Buildings = new List<GameObject>(); // list of all building prefabs
    public GameObject userCreatedObjects;
    public GameObject selectedGO;

    [Header("Game Settings")]
    public int startGold = 0;
    public int startWood = 100;
    public int startStone = 10;
    public int startFood = 50;
    public int startCopper = 0;
    public int startCitizenMax = 5;
    public int startCitizen = 5;

    [Header("Game stats")]
    public int _gold;
    public int _wood;
    public int _stone;
    public int _food;
    public int _copper;
    public int _citizen;
    public int _maxCitizen;

    [Header("Object debug binding")]
    public GameObject chosenPrefabToBuild; // prefab for instantiation
    public GameObject buildingSelectionFrameObject;
    public GameObject buildFrameObject;

    // privates 
    private GameData gameData;
    private GameObject gameCamera;
    // grid stuff
    private SnapGrid grid;
    private GameObject buildPanel; // GUI panel with build buttons
    private GameObject actionPanel; // GUI panel with selected object actions
    private List<Transform> TopBarBtns = new List<Transform>(); // list with buttons, will fill in code
    private GameObject TopBar; // link for top ui bar  

    private void Start()
    {
        userCreatedObjects = userCreatedObjectsGO();
        CoreObjectsFindOnScene();
        FillBuildPanel();
        FillTopBar();
        UpdateTopBar();
        InstantiateFrame();
        InstantiateBuildFrame();        
    }
    private void CoreObjectsFindOnScene()
    {
        gameData = GameObject.Find("GameDataObject").GetComponent<GameData>();
        gameCamera = GameObject.Find("MainCamera");
        grid = FindObjectOfType<SnapGrid>();
        buildPanel = GameObject.Find("BuildPanel");
        actionPanel = GameObject.Find("ActionPanel");
        TopBar = GameObject.Find("TopBar");
    }

    private GameObject userCreatedObjectsGO()
    {
        GameObject go = new GameObject();
        go.name = "userCreatedObjects";

        return go;
    }

    public void SelectedDestroy()
    {
        if (!selectedGO) return;        
        gameData.buildingsOnScene.Remove(selectedGO.GetComponent<Building>().saveData);
        selectedGO.GetComponent<Building>().DestroyMe();
        ClearSelection();
    }
    public void SelectedRotate()
    {
        if (!selectedGO) return;
        selectedGO.GetComponent<Building>().RotateMe();
    }

    void EnterBuildMode()
    {
        buildFrameObject.SetActive(true);
    }

    public void ExitBuildMode()
    {
        chosenPrefabToBuild = null; // clear building prefab
        buildFrameObject.SetActive(false);
    }
    void EnableBuildingSelectionFrameTo(GameObject go)
    {
        //frameObject.transform.SetParent(go.transform);
        buildingSelectionFrameObject.transform.position = go.transform.position;
        buildingSelectionFrameObject.SetActive(true);
    }

    void DisableFrame()
    {
        buildingSelectionFrameObject.SetActive(false);
    }

    public void AddResource(Resource res, int amount)
    {
        // #todo: add res to data

    }

    void ShowActionPanel(bool show)
    {
        actionPanel.SetActive(show);
    }

    public void HandleObjectsInteraction(GameObject clickedGo, Vector3 point)
    {
        // build stuff, check if clicked snapgrid
        SnapGrid clickedGrid = clickedGo.GetComponent<SnapGrid>();
        if (clickedGrid)
        {
            ClearSelection();

            if (!chosenPrefabToBuild) // leave if there is no prefab
            {
                return; // #todo: need to add some ui action like choose                
            }

            // check if can build, and build
            if (CanBuildIt(chosenPrefabToBuild))
            {
                UpdateTopBar();
                PlaceObjectNearPoint(point, chosenPrefabToBuild);
            }
        }

        // for interaction with anything, like.. Building?
        ClickableObject clickableGo = clickedGo.GetComponent<ClickableObject>();
        if (clickableGo)
        {
            Building clickedBuilding = clickableGo.GetComponent<Building>();
            if (clickedBuilding)
            {
                selectedGO = clickableGo.gameObject; // set clicked go as selected
                EnableBuildingSelectionFrameTo(selectedGO);
                ExitBuildMode();
                ShowActionPanel(true);
            }
            clickableGo.OnClick();
        }
    }

    // gameplay build
    public bool CanBuildIt(GameObject go)
    {
        // #todo: add logic
        return true;
    }

    void Update()
    {
        // ray
        RaycastHit hit;
        Vector3 finalPosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // always casting
        if (Physics.Raycast(ray, out hit))
        {
            // LMB
            if (chosenPrefabToBuild != null)
            {
                finalPosition = grid.GetNearestPointOnGrid(hit.point);
                buildFrameObject.transform.position = finalPosition;
            }
        }

        // click on objects
        if (!EventSystem.current.IsPointerOverGameObject()) // if not over ui
        {
            Transform go = hit.transform;
            if (Input.GetMouseButtonDown(0))
            {
                HandleObjectsInteraction(go.gameObject, hit.point);
            }
        }

        if (Input.GetMouseButtonDown(1)) // exit build mode or rmb #todo: rework this, dunno how
        {
            ExitBuildMode();
        }
    }

    void ClearSelection() // clear unit selection
    {
        DisableFrame();
        selectedGO = null;
        ShowActionPanel(false);
    }

    void InstantiateFrame()
    {
        buildingSelectionFrameObject = GameObject.Instantiate(framePrefab, Vector3.down * 100, Quaternion.identity);
        buildingSelectionFrameObject.SetActive(false);
    }

    void InstantiateBuildFrame()
    {
        buildFrameObject = GameObject.Instantiate(buildFramePrefab, Vector3.zero, Quaternion.identity);
        buildFrameObject.SetActive(false);
    }

    public void ChangeActiveBuildingTo(int id)
    {
        EnterBuildMode();
        chosenPrefabToBuild = Buildings[id];
    }

    void UpdateTopBar()
    {
        TopBarBtns[0].GetComponentInChildren<Text>().text = "Gold\n" + _gold;
        TopBarBtns[1].GetComponentInChildren<Text>().text = "Wood\n" + _wood;
        TopBarBtns[2].GetComponentInChildren<Text>().text = "Stone\n" + _stone;
        TopBarBtns[3].GetComponentInChildren<Text>().text = "Food\n" + _food;
        TopBarBtns[4].GetComponentInChildren<Text>().text = "Copper\n" + _copper;
        TopBarBtns[5].GetComponentInChildren<Text>().text = "Citizen\n" + _citizen;
    }

    void FillTopBar()
    {
        foreach (Transform child in TopBar.transform)
        {
            TopBarBtns.Add(child);
        }
        UpdateTopBar();
    }

    // object placer
    private GameObject PlaceObjectNearPoint(Vector3 clickPoint, GameObject prefab)
    {
        var finalPlacingPosition = grid.GetNearestPointOnGrid(clickPoint);
        GameObject go = GameObject.Instantiate(prefab, userCreatedObjects.transform);
        go.transform.position = finalPlacingPosition;
        Building bld = go.GetComponent<Building>();
        go.name = bld.name;
        return go;

    }

    // fill ui build panel
    void FillBuildPanel()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("_btn");
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < Buildings.Count)
            {
                //buttons[i].GetComponent<Image>().sprite = Buildings[i].GetComponent<Building>().sprite;
                buttons[i].GetComponentInChildren<Text>().text = Buildings[i].GetComponent<Building>().buildingName;
            }
            else
            {
                buttons[i].SetActive(false);
            }
        }
    }

    public void SaveSceneObjects()
    {
        gameData.buildingsOnScene = new List<SerializableObject>();
        foreach (Transform userCreatedObject in userCreatedObjects.transform)
        {
            Building buildingComponent = userCreatedObject.GetComponent<Building>();
            SerializableObject saveData;
            saveData.name = buildingComponent.buildingName;
            saveData.position.x = buildingComponent.transform.position.x;
            saveData.position.y = buildingComponent.transform.position.y;
            saveData.position.z = buildingComponent.transform.position.z;
            saveData.prefabName = buildingComponent.buildingPrefab.name;
            gameData.buildingsOnScene.Add(saveData);
        }
        gameData.SaveData();
    }

    void WipeUserObjectsOnScene()
    {
        if(userCreatedObjects != null)
        {
            Destroy(userCreatedObjects);
        }
        userCreatedObjects = userCreatedObjectsGO();
    }

    public void LoadSavedObjects()
    {   
        List<SerializableObject> buildingsOnScene = gameData.buildingsOnScene;
        WipeUserObjectsOnScene();

        if (buildingsOnScene != null)
        {
            foreach (SerializableObject so in buildingsOnScene)
            {
                foreach (GameObject go in Buildings)
                {
                    if (so.name == go.GetComponent<Building>().buildingName)
                    {
                        Vector3 newPosition = new Vector3(so.position.x, so.position.y, so.position.z);
                        GameObject newGo = PlaceObjectNearPoint(newPosition, go);
                    }
                }
            }
        }
    }
}