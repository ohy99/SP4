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
    [SerializeField]
    int cost;
    [SerializeField]
    string itemName;
    [SerializeField]
    string itemDescription;

	// Use this for initialization
	virtual public void Start ()
    {	
	}

    // Update is called once per frame
    virtual public void Update ()
    {
	}

    virtual public ITEM_TYPE ItemType()
    {
        return itemType;
    }

    virtual public void SetItemType(ITEM_TYPE _itemType)
    {
        itemType = _itemType;
    }
}
