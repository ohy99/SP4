using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour {

    public GameObject icon;
    public GameObject parentBackground;

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
        iconList = new List<GameObject>();
        for (int i = 0; i < 40; ++i)
        {
            //GUI pivot is at top left not in mid of the gui need offset by half of scaleX, scaleY 
            if(i != 0)
            {
                GameObject _icon = Instantiate(icon);
                _icon.transform.SetParent(parentBackground.transform, false);//Setting button parent 
                iconList.Add(_icon);
            }
            else
            {
                GameObject _icon = Instantiate(icon);
                _icon.transform.SetParent(parentBackground.transform, false);//Setting button parent 
                string itemName = "Sword";
                _icon.transform.GetChild(0).GetComponent<Image>().sprite = ItemManager.Instance.items[itemName].GetComponent<ItemBase>().sprite;
                iconList.Add(_icon);
            }
           // string itemName = "Sword";
           // _icon.transform.GetChild(0).GetComponent<Image>().sprite = ItemManager.Instance.items[itemName].GetComponent<ItemBase>().sprite;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    void AddToSlot(int index)
    {
        if (index >= iconList.Count || index < 0)
            return;

        //iconList[index].GetComponent<Image>().sprite = 
    }

    
}
