using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct SerializableObject
{
    public string name;
    public string prefabName;
    public string position;
    public string rotation;
}

[System.Serializable]
public struct BuildingActionPanel
{
    public GameObject panel;
    public Image image;
    public Text nameText;
    public Text descriptionText;
    public Text costText;
    public Button button;
}

public class GameController : MonoBehaviour
{
    [Header("Object bindings")]
    public GameData gameData;
    public GameObject framePrefab;
    public GameObject buildFramePrefab;
    
    [Header("UI Object bindings")]
    public BuildingActionPanel buildingActionPanel = new BuildingActionPanel();
    public GameObject actionPanel; // GUI panel with selected object actions
    public GameObject buildPanel;
    public GameObject button;
     
    public Text notificationText; 

    [Header("Object debug binding")]
    public GameObject chosenPrefabToBuild; // prefab for instantiation
    
    public GameObject selectedGO;


    // privates 
    
    private GameObject gameCamera;
    // frames
    private GameObject buildingSelectionFrameObject;
    private GameObject buildFrameObject;
    // grid stuff
    private SnapGrid grid;
    
    private bool isMobile; // check if mobile or pc
    float touchDuration;
    Touch touch;
    private Vector3 finalPosition = new Vector3();

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
        CoreObjectsFindOnScene();
        FillBuildPanel();
        
        InstantiateFrame();
        InstantiateBuildFrame();
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void CoreObjectsFindOnScene()
    {;
        gameCamera = GameObject.Find("MainCamera");
        grid = FindObjectOfType<SnapGrid>();
    }

    public void SelectedDestroy()
    {
        if (!selectedGO) return;
        Building bld = selectedGO.GetComponent<Building>();
        //gameData.userCreatedBuildings.Remove(bld);
        gameData.resources = gameData.resources + bld.cost;
        bld.DestroyMe();
        ClearSelection();
        
    }
    public void SelectedRotate()
    {
        if (!selectedGO) return;
        selectedGO.GetComponent<Building>().RotateMe(90);
    }

    public void EnterBuildMode()
    {
        //buildFrameObject.SetActive(true);
        ClearSelection();
        
        Building buildingComponent =  chosenPrefabToBuild.GetComponent<Building>();
        buildingActionPanel.panel.SetActive(true);
        buildingActionPanel.image.sprite = buildingComponent.sprite;
        buildingActionPanel.nameText.text = buildingComponent.buildingName;
        buildingActionPanel.descriptionText.text =  buildingComponent.description;
        
        string costText = "Cost: \n";        
        if (buildingComponent.cost.copper != 0) 
        {
            string t = "<color=#ffa500ff>Copper:</color>";
            if (buildingComponent.cost.copper > gameData.resources.copper)
            {t += "<color=#ff0000ff>"+ buildingComponent.cost.copper.ToString() +"</color>" + "\n";}
            else {t+=buildingComponent.cost.copper.ToString()+ "\n";}
            costText +=  t;
        }
        if (buildingComponent.cost.food != 0) 
        {
            string t = "<color=#008000ff>Food:</color>";
            if (buildingComponent.cost.food > gameData.resources.food)
            {t += "<color=#ff0000ff>"+ buildingComponent.cost.food.ToString() +"</color>" + "\n";}
            else {t+=buildingComponent.cost.food.ToString()+ "\n";}
            costText +=  t;
        }
        if (buildingComponent.cost.gold != 0) 
        {
            string t = "<color=#ffff00ff>Gold:</color>";
            if (buildingComponent.cost.gold > gameData.resources.gold)
            {t += "<color=#ff0000ff>"+ buildingComponent.cost.gold.ToString() +"</color>" + "\n";}
            else {t+=buildingComponent.cost.gold.ToString()+ "\n";}
            costText +=  t;
            
        }
        if (buildingComponent.cost.stone != 0) 
        {
            string t = "<color=#808080ff>Stone:</color>";
            if (buildingComponent.cost.stone > gameData.resources.stone)
            {t += "<color=#ff0000ff>"+ buildingComponent.cost.stone.ToString() +"</color>" + "\n";}
            else {t+=buildingComponent.cost.stone.ToString()+ "\n";}
            costText +=  t;
        }
        if (buildingComponent.cost.wood != 0) 
        {
            string t = "<color=#a52a2aff>Wood:</color>";
            if (buildingComponent.cost.wood > gameData.resources.wood)
            {t += "<color=#ff0000ff>"+ buildingComponent.cost.wood.ToString() +"</color>" + "\n";}
            else {t+=buildingComponent.cost.wood.ToString()+ "\n";}
            costText +=  t;       
        }
        buildingActionPanel.costText.text = costText;
        
    }

    public void ExitBuildMode()
    {
        buildingActionPanel.panel.SetActive(false);
        chosenPrefabToBuild = null; // clear building prefab
        buildFrameObject.SetActive(false);
    }
    void EnableBuildingSelectionFrameTo(GameObject go)
    {
        //frameObject.transform.SetParent(go.transform);
        buildingSelectionFrameObject.transform.position = go.transform.position;
        buildingSelectionFrameObject.SetActive(true);
    }

    void DisableSelectionFrame()
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
            if (CanBuildIt(chosenPrefabToBuild.GetComponent<Building>()))
            {
                gameData.resources = gameData.resources - chosenPrefabToBuild.GetComponent<Building>().cost;
                PlaceObjectWithParams(chosenPrefabToBuild, point, Quaternion.identity,ObjectType.Building);
            }
            EnterBuildMode();
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
    public bool CanBuildIt(Building bld)
    {
        bool can = false;
        if 
        (
            bld.cost.citizen <= gameData.resources.citizen &&
            bld.cost.copper <= gameData.resources.copper &&
            bld.cost.food <= gameData.resources.food &&
            bld.cost.gold <= gameData.resources.gold &&
            bld.cost.stone <= gameData.resources.stone &&
            bld.cost.wood <= gameData.resources.wood
        )
        //Debug.Log("weoweo");

        if (bld.cost <= gameData.resources)
        {
            can = true;
        }
        return can;
    }


    void Update()
    {
        // ray
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // always casting
        if (Physics.Raycast(ray, out hit))
        {
            finalPosition = grid.GetNearestPointOnGrid(hit.point);
        }

        if (isMobile)
        {
            if(Input.touchCount > 0)
            { //if there is any touch
                touchDuration += Time.deltaTime;
                touch = Input.GetTouch(0);
    
                if(touch.phase == TouchPhase.Ended && touchDuration < 0.2f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
                
                {
                    StartCoroutine("singleOrDouble");
                }
            }
            else
            {
                touchDuration = 0.0f;
            }                
        }
        if (Application.isEditor)
        {
            // click on objects
            if (!IsPointerOverUIObject()) // if not over ui
            {

                Transform go = hit.transform;
                if (Input.GetMouseButtonDown(0))
                {
                    if (go != null) HandleObjectsInteraction(go.gameObject, hit.point);
                }
            }

            if (chosenPrefabToBuild != null)
            {                
                buildFrameObject.transform.position = finalPosition;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                GenerateForest();
            }

            if (Input.GetMouseButtonDown(1)) // exit build mode or rmb #todo: rework this, dunno how
            {
                ExitBuildMode();
            }
        }
    }
    IEnumerator singleOrDouble()
    {
        // ray
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // always casting
        if (Physics.Raycast(ray, out hit))
        {
            finalPosition = grid.GetNearestPointOnGrid(hit.point);
        }

        yield return new WaitForSeconds(0.15f);
        if(touch.tapCount == 1)
        {
            if (!IsPointerOverUIObject()) // if not over ui
                {
                    Transform go = hit.transform;
                    if (go != null) HandleObjectsInteraction(go.gameObject, hit.point);
                }
        }            

        else if(touch.tapCount == 2){
            //this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            StopCoroutine("singleOrDouble");
            Debug.Log ("Double");
        }
    }
    public void GenerateForest()
    {
        WipeScene();
        
        GameObject tree = gameData.availableObjects[0];
        GameObject ore = gameData.availableObjects[1];
        GameObject ore2 = gameData.availableObjects[2];
        GameObject stone = gameData.availableObjects[3];


        foreach (Vector3 v in grid.allPointsOnMap)
        {
            int myRnd = Random.Range(0, 100);
            if (myRnd >= 70) PlaceObjectWithParams(tree, v, Quaternion.identity,ObjectType.Object);
            else if (myRnd == 1) PlaceObjectWithParams(ore, v, Quaternion.identity,ObjectType.Object);
            else if (myRnd == 2) PlaceObjectWithParams(ore2, v, Quaternion.identity,ObjectType.Object);
            else if (myRnd == 4) PlaceObjectWithParams(stone, v, Quaternion.identity,ObjectType.Object);
        }

        GetComponent<Notification>().SetNotification("Location Generated");
    }

    void ClearSelection() // clear unit selection
    {
        DisableSelectionFrame();
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
        chosenPrefabToBuild = gameData.availableBuildings[id];
        EnterBuildMode();        
    }

    public void WipeScene()
    {
        gameData.userCreatedObjects = new List<Building>();
        foreach (Transform child in gameData.userCreatedObjectsGO.transform)
        {
            Destroy(child.gameObject);
        }
        gameData.generatedObjects = new List<Building>();
        foreach (Transform child in gameData.generatedObjectsGO.transform)
        {
            Destroy(child.gameObject);
        }
    }
 
    // object placer
    public void PlaceObjectWithParams(GameObject prefab, Vector3 clickPoint, Quaternion rot, ObjectType type)
    {
        Vector3 position = grid.GetNearestPointOnGrid(clickPoint);
        GameObject go = GameObject.Instantiate(prefab);
        Building bld = go.GetComponent<Building>();
        bld.gameData = gameData;
        bld.type = type;
        bld.Initialize();

        go.name = bld.name;
        go.transform.position = position;
    }

    public int SearchIdByNameIn(string str, List<GameObject> list)
    {
        int id = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (str == list[i].GetComponent<Building>().buildingName)
            {
                id = i;
            }
        }
        return id;
    }
    public void PlaceBuildingfromSO(SerializableObject so)
    {
        GameObject go = GameObject.Instantiate(gameData.availableBuildings[SearchIdByNameIn(so.name, gameData.availableBuildings)]);
        Building building = go.GetComponent<Building>();
        building.gameData = gameData;
        building.type = ObjectType.Building;
        building.InitializeWithSO(so);
    }
    public void PlaceObjectfromSO(SerializableObject so)
    {
        GameObject go = GameObject.Instantiate(gameData.availableObjects[SearchIdByNameIn(so.name, gameData.availableObjects)]);
        Building building = go.GetComponent<Building>();
        building.gameData = gameData;
        building.type = ObjectType.Object;
        building.InitializeWithSO(so);
    }

    // fill ui build panel
    void FillBuildPanel()
    {
        for (int i = 0; i < gameData.availableBuildings.Count; i++)
        {
            GameObject newbtn = GameObject.Instantiate(button,buildPanel.transform);
            newbtn.GetComponentInChildren<Text>().text = gameData.availableBuildings[i].GetComponent<Building>().buildingName;
            newbtn.GetComponent<SetActiveBuilding>().id = i;
            newbtn.GetComponent<SetActiveBuilding>().gc = this;
            newbtn.GetComponent<SetActiveBuilding>().gameData = gameData;
            if (gameData.availableBuildings[i].GetComponent<Building>().sprite != null)
            {
                newbtn.GetComponent<Image>().sprite = gameData.availableBuildings[i].GetComponent<Building>().sprite;
            }
        }
        button.SetActive(false);
    }
}