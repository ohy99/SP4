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
    public Rect windowRect0 = new Rect(50, 50, 200, 200);
    bool isClicked = false;

    void Start ()
    {
        // Load items in
        shopCanvas.SetActive(true);
        Time.timeScale = 0;
        isClicked = false;
        windowRect0 = new Rect(50, 50, 200, 200);

        for (int i = 0; i < ItemManager.Instance.itemNames.Count; ++i)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.transform.SetParent(panelToAttachButtonsTo.transform, false);//Setting button parent                                                                                
            string iName = ItemManager.Instance.itemNames[i];
            button.GetComponent<Button>().onClick.AddListener(delegate { OnClick(iName); });//Setting what button does when clicked
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

	void OnClick(string itemName)
    {
        //player clicked on this item
        //display a pop_up to confirm purchase
        if (!isClicked)
        {
            Debug.Log("BUYING -> " + itemName);
            isClicked = true;
        }
    }

	// Update is called once per frame
	void Update ()
    {
       
	}

    void OnGUI()
    {
        if (isClicked)
        {
            GUI.color = Color.red;
            windowRect0 = GUI.Window(0, windowRect0, DoMyWindow, "Confirm Purchase");
        }
    }
    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(50, 80, 100, 20), "Buy"))
        {
            //minus player currency here
            print("Bought Item");
            isClicked = false;
        }
        if (GUI.Button(new Rect(50, 120, 100, 20), "Cancel"))
        {
            //go back
            print("Canceled");
            isClicked = false;
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public void ShopQuit()
    {
        Time.timeScale = 1;
        shopCanvas.SetActive(false);
    }

    public void SetShopActive(bool _isActive)
    {
        shopCanvas.SetActive(_isActive);
    }
}
