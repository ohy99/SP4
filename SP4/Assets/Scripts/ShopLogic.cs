using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogic : MonoBehaviour
{
    [SerializeField]
    GameObject shopCanvas;

    public GameObject buttonPrefab;
    public GameObject panelToAttachButtonsTo;

    void Start ()
    {
        // Load items in
        shopCanvas.SetActive(true);

        for (int i = 0; i < ItemManager.Instance.itemNames.Count; ++i)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.transform.SetParent(panelToAttachButtonsTo.transform, false);//Setting button parent
            //button.transform.localScale = new Vector3(1, 1, 1);
            button.GetComponent<Button>().onClick.AddListener(OnClick);//Setting what button does when clicked
            string iName = ItemManager.Instance.itemNames[i];
            button.transform.GetChild(0).GetComponent<Text>().text = iName;
            button.transform.GetChild(1).GetComponent<Text>().text = ItemManager.Instance.items[iName].GetComponent<ItemBase>().itemDescription;
            button.transform.GetChild(2).GetComponent<Text>().text = ItemManager.Instance.items[iName].GetComponent<ItemBase>().cost.ToString();
        }

        //for (int i = 0; i < 20; ++i)
        //{
        //    GameObject button = Instantiate(buttonPrefab);
        //    button.transform.SetParent(panelToAttachButtonsTo.transform, false);//Setting button parent
        //    button.transform.localScale = new Vector3(1, 1, 1);
        //    button.GetComponent<Button>().onClick.AddListener(OnClick);//Setting what button does when clicked
        //    button.transform.GetChild(0).GetComponent<Text>().text = "ITEM";
        //    button.transform.GetChild(1).GetComponent<Text>().text = "DESCRIPTION";
        //}
    }

	void OnClick()
    {
        
    }

	// Update is called once per frame
	void Update ()
    {
       
	}

    public void SetShopActive(bool _isActive)
    {
        shopCanvas.SetActive(_isActive);
    }
}
