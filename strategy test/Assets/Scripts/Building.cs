using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : ClickableObject
{
    [Header("Prefab settings")]
    public GameObject buildingPrefab;
    public GameObject popupPrefab;
    public GameObject bgPrefab;
    public Sprite sprite;
    public string buildingName;
 
    // privates
    private GameObject building,popup;
    private GameData GameDataObject;
    private Material normalMat;

    // handle when click on this object
    public override void OnClick()
    {
        if (popup.activeSelf == true)
        {
            return;
        }
        //popup.SetActive(true);
    }
    // Start is called before the first frame update
    public void InitializeObject()
    {
        AddCollider();            
        PopupCreate();
        AddBg();
        building = InstantiateObject(buildingPrefab);
        building.name = buildingName;    
        normalMat = building.GetComponentInChildren<Renderer>().material;
        GameDataObject = GameObject.Find("GameDataObject").GetComponent<GameData>();
        GameDataObject.bldOnScene.Add(this);
    }

    public void InitializeObjectFromSO(SerializableObject so)
    {
 
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
        popup_rt.localPosition = new Vector3(0,1,.5f);
        popup_rt.Rotate(transform.right,45);
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
        saveData.rotation = JsonUtility.ToJson(transform.rotation, true);
        saveData.prefabName = buildingPrefab.name;

        return saveData;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
