using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[InitializeOnLoad]
public class InventoryManager : Singleton<InventoryManager>
{
    private Dictionary<string, Inventory> inventories;

    // Use this for initialization
    public void Start ()
    {
        //inventories = new Dictionary<string, Inventory>();
        ////intialise player's inventory
        //inventories["player"] = new Inventory();
        //inventories["player"].Init();
        // other stuff enemies,another player inventory
	}

    public void Init()
    {
        inventories = new Dictionary<string, Inventory>();
    }

    public bool AddInventory(string key)
    {
        if(!inventories.ContainsKey(key))
        {
            inventories[key] = new Inventory();
            return true;
        }

        print("key is in use");
        return false;
    }

    public bool AddInventory(string key,Inventory _inventory)
    {
        if (!inventories.ContainsKey(key))
        {
            inventories[key] = _inventory;
            return true;
        }

        print("key is in use");
        return false;
    }

    public bool OverwriteExistingInvetory(string key, Inventory _inventory)
    {
        if(inventories.ContainsKey(key))
        {
            inventories[key] = _inventory;
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
}
