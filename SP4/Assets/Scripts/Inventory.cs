using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, GameObject> myItems = new Dictionary<string, GameObject>();
    private List<string> itemNameList = new List<string>();

    // Use this for initialization
    public void Init ()
    {
        Debug.Log("INVETORY START");

        AddWeapon(ItemManager.Instance.items["Crossbow"], "Crossbow");
        itemNameList.Add("Crossbow");
        AddWeapon(ItemManager.Instance.items["Sword"], "Sword");
        itemNameList.Add("Sword");
    }

    public void AddWeapon(GameObject _item, string itemName)
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

    public string GetItemName(int index)
    {
        if (index >= itemNameList.Count || index < 0)
            return "";

        return itemNameList[index];
    }

    public List<string> GetItemNameList()
    {
        return itemNameList;
    }
}
