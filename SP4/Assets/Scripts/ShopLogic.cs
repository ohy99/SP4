using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject shopCanvas;

    private bool isClicked = false;
    private bool isPurchaseable = true;
    private string itemToBuy;
    private List<GameObject> buttonList;
    private Rect windowRect0;
    private Rect windowRect1;

    public GameObject confirmationPanel;
    public GameObject buttonPrefab;
    public GameObject panelToAttachButtonsTo;



    void Start ()
    {
        // Load items in
        confirmationPanel.SetActive(false);
        shopCanvas.SetActive(false);
        Time.timeScale = 1;
        isClicked = false;
        isPurchaseable = true;
        windowRect0 = new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 100, 200, 200);
        windowRect1 = new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 100, 200, 200);
        buttonList = new List<GameObject>();
       
        

        for (int i = 0; i < ItemManager.Instance.itemNames.Count; ++i)
        {
            //GUI pivot is at top left not in mid of the gui need offset by half of scaleX, scaleY 

            GameObject button = Instantiate(buttonPrefab);
            button.transform.SetParent(panelToAttachButtonsTo.transform, false);//Setting button parent                                                                                
            string iName = ItemManager.Instance.itemNames[i];
            button.GetComponent<Button>().onClick.AddListener(delegate { OnClick(iName); });//Setting what button does when clicked
            button.transform.GetChild(0).GetComponent<Text>().text = iName;
            button.transform.GetChild(1).GetComponent<Text>().text = ItemManager.Instance.items[iName].GetComponent<ItemBase>().itemDescription;
            button.transform.GetChild(2).GetComponent<Text>().text = ItemManager.Instance.items[iName].GetComponent<ItemBase>().cost.ToString();
            buttonList.Add(button);
        }

        //for (int i = 0; i < 20; ++i)
        //{
        //    GameObject button = Instantiate(buttonPrefab);
        //    button.transform.SetParent(panelToAttachButtonsTo.transform, false);//Setting button parent
        //    button.transform.localScale = new Vector3(1, 1, 1);
        //    button.GetComponent<Button>().onClick.AddListener(delegate { OnClick("Sword"); });//Setting what button does when clicked
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
            itemToBuy = itemName;
            isClicked = true;
            confirmationPanel.SetActive(true);
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
        if(!isPurchaseable)
        {
            GUI.color = Color.red;
            windowRect1 = GUI.Window(1, windowRect1, DoMyWindow, "Not enough money!");
        }
    }
    void DoMyWindow(int windowID)
    {
        switch(windowID)
        {
            case 0:
                {
                    if (GUI.Button(new Rect(50, 80, 100, 20), "Buy"))
                    {
                        int currency = InventoryManager.Instance.GetInventory("player").GetCurrency();
                        int itemCost = ItemManager.Instance.items[itemToBuy].GetComponent<ItemBase>().cost;
                        if (currency >= itemCost)
                        {
                            //minus player currency here
                            print("Bought Item");
                            InventoryManager.Instance.GetInventory("player").SetCurrency(currency - itemCost);
                            InventoryManager.Instance.GetInventory("player").AddItem(ItemManager.Instance.items[itemToBuy], itemToBuy);
                            confirmationPanel.SetActive(false);
                        }
                        else
                        {
                            //pop a screen to tell that they lack the monay
                            isPurchaseable = false;
                        }

                        isClicked = false;
                    }
                    if (GUI.Button(new Rect(50, 120, 100, 20), "Cancel"))
                    {
                        //go back
                        print("Canceled");
                        isClicked = false;
                        confirmationPanel.SetActive(false);
                    }
                }
                break;
            case 1:
                {
                    if (GUI.Button(new Rect(50, 120, 100, 20), "Back"))
                    {
                        //go back
                        print("backed");
                        isPurchaseable = true;
                        confirmationPanel.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
       

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void PurchaseItem()
    {

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
