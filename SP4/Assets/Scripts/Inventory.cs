﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, GameObject> myItems = new Dictionary<string, GameObject>();
    private List<string> itemNameList = new List<string>();

    [HideInInspector]
    public string[] slot = new string[15];

    private int currency;
    private bool isInvetory;

    //private const int MAX_INVENTORY_SIZE = 15;

    // Use this for initialization
    public void Init ()
    {
        Debug.Log("INVETORY START");

        slot = new string[15];
        for (int i = 0; i < 15; ++i)
            slot[i] = null;

        if(itemNameList.Count > 0)
        {
            for(int i = 0; i < itemNameList.Count; ++i)
                slot[i] = itemNameList[i];
        }


        AddItem(ItemManager.Instance.items["Crossbow"], "Crossbow");

        isInvetory = false;

        currency = 1000;
    }

    public void AddItem(GameObject _item, string itemName)
    {
        Debug.Log("added " + itemName);
        if (!myItems.ContainsKey(itemName))
        {
            _item = GameObject.Instantiate(_item) as GameObject;
            myItems[itemName] = _item;
            itemNameList.Add(itemName);
            AddToSlot(itemName);
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

    public void AddCurrency(int currencyToAdd)
    {
        Debug.Log(currencyToAdd);
        currency += currencyToAdd;
    }

    public void AddToSlot(string iName)
    {
        for(int i = 0; i < 15; ++i)
        {
            if (slot[i] == null)
            {
                slot[i] = iName;
                break;
            }
        }
    }

    public void SwapSlots(int first,int second)
    {
        string temp = slot[first];
        slot[first] = slot[second];
        slot[second] = temp;
    }

    public bool GetIsInventory()
    {
        return isInvetory;
    }

    public void SetIsInventory(bool _isInventory)
    {
        isInvetory = _isInventory;
    }

    public void LoadItems()
    {
        foreach(string itemName in ItemManager.Instance.itemNames)
        {
            if (System.Convert.ToBoolean(PlayerPrefs.GetInt(itemName)))
            {
                AddItem(ItemManager.Instance.items[itemName], itemName);
            }
        }


        currency = PlayerPrefs.GetInt("Currency", 1000);
    }

    public void SaveItems()
    {
        foreach(string itemName in ItemManager.Instance.itemNames)
        {
            PlayerPrefs.SetInt(itemName, System.Convert.ToInt32(myItems.ContainsKey(itemName)));
        }

        PlayerPrefs.SetInt("Currency", currency);
        PlayerPrefs.Save();
    }
}
