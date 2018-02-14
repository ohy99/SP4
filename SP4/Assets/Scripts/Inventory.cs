using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, GameObject> myItems = new Dictionary<string, GameObject>();
   
    //public List<GameObject> myItems;

    // Use this for initialization
    public void Init ()
    {
        Debug.Log("INVETORY START");
        AddWeapon(ItemManager.Instance.items["Crossbow"], "Crossbow");
        AddWeapon(ItemManager.Instance.items["Sword"], "Sword");
    }

    public void AddWeapon(GameObject _item,string itemName)
    {
        Debug.Log("added");
        if (!myItems.ContainsKey(itemName))
        {
            myItems[itemName] = _item;
        }
    }

    public void RemoveWeapon(GameObject _item, string itemName)
    {
        myItems[itemName] = null;
    }

    public GameObject GetItem(string itemName)
    {
        if (!myItems.ContainsKey(itemName))
        {
            Debug.Log("Key is not assigned.");
            return null;
        }

        return myItems[itemName];
    }
}
