using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public enum ITEM_TYPE
    {
        WEAPON = 0,
        MISC,
    }

    [SerializeField]
    protected ITEM_TYPE itemType;

    public int cost;
    public string itemName;
    public string itemDescription;

	// Use this for initialization
	virtual public void Start ()
    {	
	}

    // Update is called once per frame
    virtual public void Update ()
    {
	}

    //get item type
    virtual public ITEM_TYPE ItemType()
    {
        return itemType;
    }

    //set itemtype
    virtual public void SetItemType(ITEM_TYPE _itemType)
    {
        itemType = _itemType;
    }

    virtual public void OnClick()
    {
    }
}
