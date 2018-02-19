using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, GameObject> myItems = new Dictionary<string, GameObject>();
    private List<string> itemNameList = new List<string>();
    int currency;

    // Use this for initialization
    public void Init ()
    {
        Debug.Log("INVETORY START");

        AddItem(ItemManager.Instance.items["Crossbow"], "Crossbow");
        //itemNameList.Add("Crossbow");
        //RemoveItem(ItemManager.Instance.items["Sword"], "Sword");
        //itemNameList.Add("Sword");
        currency = 1000;
    }

    public void AddItem(GameObject _item, string itemName)
    {
        Debug.Log("added");
        if (!myItems.ContainsKey(itemName))
        {
            myItems[itemName] = _item;
            itemNameList.Add(itemName);
        }
    }

    public void RemoveItem(GameObject _item, string itemName)
    {
        myItems[itemName] = null;
        itemNameList.Remove(itemName);
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

    public int GetCurrency()
    {
        return currency;
    }

    public void SetCurrency(int _currency)
    {
        currency = _currency;
    }
}
