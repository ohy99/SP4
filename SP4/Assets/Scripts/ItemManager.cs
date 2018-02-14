using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
    public List<string> itemNames = new List<string>();

    protected ItemManager()
    {
    }

    public void Start()
    {
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
       
}
