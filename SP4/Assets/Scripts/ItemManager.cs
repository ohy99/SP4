using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/*
 *     private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
 * 
 */


[InitializeOnLoad]
public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
    public List<string> itemNames = new List<string>();

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

        Debug.Log("ItemManagerStart");
        LoadAllItems();
    }

    public void LoadAllItems()
    {
        GameObject go;
        var loadedObj = Resources.LoadAll("Weapons");
        foreach(var obj in loadedObj)
        {
            go = (obj as GameObject);
            items[go.name] = Instantiate(go);
            itemNames.Add(go.name);
            Debug.Log("itemName: " + go.name);
        }

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
