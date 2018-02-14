using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Dictionary<string, ItemBase> myWeapon;

	// Use this for initialization
	void Start ()
    {
	}

    public void AddWeapon(ItemBase _item,string itemName)
    {
        if(!myWeapon.TryGetValue(itemName, out _item))
        {
            myWeapon[itemName] = _item;
        }
    }

    public void RemoveWeapon(ItemBase _item, string itemName)
    {
        if (!myWeapon.TryGetValue(itemName, out _item))
        {
            myWeapon[itemName] = null;
        }
    }
}
