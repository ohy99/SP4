﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

//[InitializeOnLoad]
public class InventoryManager : Singleton<InventoryManager>
{
    private Dictionary<string, Inventory> inventories;

    // Use this for initialization
    public void Start ()
    {
        //DontDestroyOnLoad(gameObject);
        Debug.Log("InventoryManagerStart");
        //inventories = new Dictionary<string, Inventory>();
        ////intialise player's inventory
        //inventories["player"] = new Inventory();
        //inventories["player"].Init();
        // other stuff enemies,another player inventory
    }

    public void Init()
    {
        Debug.Log("InventoryManagerInit");
        inventories = new Dictionary<string, Inventory>();
    }

    public bool AddInventory(string key)
    {
        if(!inventories.ContainsKey(key))
        {
            //inventories[key] = new Inventory();
            inventories.Add(key, new Inventory());
            return true;
        }

        print("key is in use");
        return false;
    }

    public bool AddInventory(string key,Inventory _inventory)
    {
        if (!inventories.ContainsKey(key))
        {
            //inventories[key] = _inventory;
            inventories.Add(key, _inventory);
            return true;
        }

        print("key is in use");
        return false;
    }

    public bool OverwriteExistingInvetory(string key, Inventory _inventory)
    {
        if(inventories.ContainsKey(key))
        {
            //inventories[key] = _inventory;
            inventories.Add(key, _inventory);
            return true;
        }

        print("key does not exist (overwrite Func)");
        return false;
    }

    public bool RemoveInventory(string key)
    {
        if (inventories.ContainsKey(key))
        {
            inventories[key] = null;
            return true;
        }

        print("key does not exist (remove Func)");
        return false;
    }

    public Inventory GetInventory(string key)
    {
        if(inventories.ContainsKey(key))
        {
            return inventories[key];
        }

        print("key does not exist, creating new invetory with " + key + "as key (GetInventory Func)");
        AddInventory(key);
        return inventories[key];
    }

    public void AddAllItems(string key)
    {
        if (inventories.ContainsKey(key))
        {
            for(int i = 0; i < ItemManager.Instance.itemNames.Count; ++i)
            {
                string itemName = ItemManager.Instance.itemNames[i];
                inventories[key].AddItem(ItemManager.Instance.items[itemName], itemName);
            }

        }

        Debug.Log("key does not exist");
    }
}
