using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SerializableObject
{
    public ObjectType type;
    public string name;
    public string prefabName;
    public string position;
    public string rotation;
}

public enum ObjectType
{
    Building,
    Object
}
public class PlaceableObject : ClickableObject
{
    [Header("Prefab settings")]
    public ObjectType type;
    public GameObject prefab;
    public GameObject popupPrefab;
    public GameObject bgPrefab;
    public Sprite sprite;
    public string buildingName;
    public string description;
    public Resources cost;
    
    [Header("Debug publics")]
    public GameData gameData; 
 
    // privates
    private GameObject building,popup;
    
    private Material normalMat;

    // handle when click on this object
    public override void OnClick()
    {
        if (popup.activeSelf == true)
        {
            return;
        }
        popup.SetActive(true);
    }
    // Start is called before the first frame update
    public void Initialize()
    {
        AddCollider();            
        PopupCreate();
        AddBg();

        // Create GameObject
        building = InstantiateObject(prefab);
        building.name = buildingName;    

        if (type == ObjectType.Building)
        {
            transform.SetParent(gameData.userCreatedObjectsGO.transform);
            gameData.userCreatedObjects.Add(this);
        } 
        if (type == ObjectType.Object)
        {
            transform.SetParent(gameData.generatedObjectsGO.transform);
            gameData.generatedObjects.Add(this);
         }
        // push this building to GameData
        gameData.GetComponent<Notification>().text.text = "Placed "+ buildingName;
        
    }
    
    public void InitializeWithSO(SerializableObject so)
    {
        Initialize();
        transform.position = JsonUtility.FromJson<Vector3>(so.position);
        building.transform.rotation = JsonUtility.FromJson<Quaternion>(so.rotation);
    }

    void AddCollider()
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(1,0.1f,1);
        col.center = new Vector3(0,0.05f,0);
    }
    void AddBg()
    {
        if(!bgPrefab)
        {
            return;
        }
        GameObject bg = GameObject.Instantiate(bgPrefab,transform.position,Quaternion.identity);
        bg.transform.SetParent(transform);        
    }

    void PopupCreate()
    {
        if (!popupPrefab)
        {
            return;
        }
        popup = InstantiateObject(popupPrefab);
        RectTransform popup_rt = popup.GetComponent<RectTransform>();
        popup_rt.localPosition = new Vector3(0,2,0);
        popup_rt.Rotate(new Vector3(45,-45,0));
        //popup_rt.Rotate(transform.up,-45);
        popup.SetActive(false);
        List<Transform> popup_btns = new List<Transform>();

        // find buttons in panel (this is really shitty)
        Transform btn_panel = popup.transform.Find("btn_panel");
        foreach (Transform child in btn_panel){
            popup_btns.Add(child);
        }

        // add funcs to them
        // Destroy btn
        popup_btns[0].GetComponent<Button>().onClick.AddListener(DestroyMe);
        popup_btns[0].GetChild(0).GetComponent<Text>().text = "Destroy";

        // Rotate Btn
        popup_btns[1].GetComponent<Button>().onClick.AddListener(delegate{RotateMe(90);});
        popup_btns[1].GetChild(0).GetComponent<Text>().text = "Rotate";
    }

    public void RotateMe(float degree)
    {
        building.transform.Rotate(Vector3.up,degree);
    }
    public void DestroyMe()
    {        
        if (type == ObjectType.Building)
        {
            gameData.userCreatedObjects.Remove(this);
        }
        if (type == ObjectType.Object)
        {
            gameData.generatedObjects.Remove(this);
        }
        gameData.resources = gameData.resources + cost; 
        gameData.GetComponent<Notification>().text.text = "Destroyed "+ buildingName;
        Destroy(gameObject);
    }

    GameObject InstantiateObject(GameObject go)
    {
        GameObject newgo = Instantiate(go,transform.position,Quaternion.identity);
        newgo.transform.SetParent(transform);
        return newgo;
    }
    public SerializableObject ObjectSaveData()
    {
        SerializableObject saveData; 
        saveData.name = buildingName;
        saveData.position = JsonUtility.ToJson(transform.position, true);
        saveData.rotation = JsonUtility.ToJson(building.transform.rotation, true);
        saveData.prefabName = prefab.name;
        saveData.type = type;
        
        print (saveData);
        return saveData;
        
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
