using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This line below doesnt work with android :thinking:
//[UnityEditor.InitializeOnLoad]
public class ItemManager : Singleton<ItemManager>
{ 
    public Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
    public List<string> itemNames = new List<string>();
    //[SerializeField]
    //private List<GameObject> itemList = new List<GameObject>();
    //public static ItemManager Instance { get; set; }

    protected ItemManager() 
    {
     
    }

    public void Awake()
    {
        //if (Instance != null && Instance != this)
        //    Destroy(gameObject);
        //else
        //    Instance = this;
        //DontDestroyOnLoad(gameObject);
        Debug.Log("ItemManagerStart");
        LoadAllItems();

        //for(int i = 0; i < itemList.Count; ++i)
        //{
        //  itemNames.Add(itemList[i].GetComponent<ItemBase>().itemName);
        //  items.Add(itemList[i].GetComponent<ItemBase>().itemName, itemList[i]);
        //}

        Inventory temp = new Inventory();
        temp.Init();
        InventoryManager.Instance.Init(); // init the manager
        InventoryManager.Instance.AddInventory("player", temp); //Add inventory with key of player
    }

    public void LoadAllItems()
    {
        GameObject go;

        var loadedObj = Resources.LoadAll("Weapons");
        foreach (var obj in loadedObj)
        {
            go = (obj as GameObject);
            items[go.name] = Instantiate(go);
            itemNames.Add(go.name);
            //Debug.Log("itemName: " + go.name);
        }

        //itemList = Resources.FindObjectsOfTypeAll(typeof(GameObject));


        //foreach(GameObject go in items["items"])
        //{
        //    Debug.Log(go.name);
        //}
        //foreach(var go in Resources.LoadAll("Weapons", typeof(GameObject)))
        //{
        //    Debug.Log("itemName: " + go.name);
        //    items["items"].Add(go);
        //}
    }

}
