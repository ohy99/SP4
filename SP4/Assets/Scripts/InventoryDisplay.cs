using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour {

    public GameObject icon;
    public GameObject parentBackground;
    public GameObject panel;
    private List<GameObject> iconList;

	// Use this for initialization
	void Start ()
    {
        //for (int i = 0; i < ItemManager.Instance.itemNames.Count; ++i)
        //{
        //    //GUI pivot is at top left not in mid of the gui need offset by half of scaleX, scaleY 

        //    Image _icon = Instantiate(icon);
        //    _icon.transform.SetParent(parentBackground.transform, false);//Setting button parent 
        //    string itemName = ItemManager.Instance.itemNames[i];
        //    _icon.transform.GetChild(0).GetComponent<Image>().sprite = ItemManager.Instance.items[itemName].GetComponent<ItemBase>().sprite;
        //}

        panel.SetActive(InventoryManager.Instance.GetInventory("player").GetIsInventory());
        iconList = new List<GameObject>();
        for (int i = 0; i < 15; ++i)
        {
            GameObject _icon = Instantiate(icon);
            _icon.transform.SetParent(parentBackground.transform, false);//Setting button parent 
            iconList.Add(_icon);
        }


        Debug.Log("NumberOfItems: " + ItemManager.Instance.items.Count);

        string itemName = null;
        for(int j = 0; j < 15; ++j) //size of inventory(will be chanaged)
        {
            itemName = InventoryManager.Instance.GetInventory("player").slot[j];
            if(itemName != null)
                iconList[j].transform.GetChild(0).GetComponent<Image>().sprite 
                    = ItemManager.Instance.items[itemName].GetComponent<ItemBase>().sprite;
        }

    }
	
	void FixedUpdate ()
    {
        //temp
        if(InventoryManager.Instance.GetInventory("player").GetIsInventory())
        {
            panel.SetActive(true);
        }
        else
            panel.SetActive(false);
    }

    void AddToSlot(int index, Sprite icon)
    {
        //if (index >= iconList.Count || index < 0)
        //    return;

        //iconList[index].transform.GetChild(0).GetComponent<Image>().sprite = icon;
    }

    
}
