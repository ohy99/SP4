using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
    private static SomeClass _instance;

    public static SomeClass Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
 */


[InitializeOnLoad]
public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
    public List<string> itemNames = new List<string>();

    protected ItemManager()
    {
    }

    public void Start()
    {
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
            items[go.name] = go;
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

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        ItemManager.Instance.OnDestroy();
    }

}
