using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour {

    public GameObject icon;
    public GameObject parentBackground;
    public GameObject panel;
    private List<GameObject> iconList;
    private int selectedIndex;
    public Color equipedColor;
    public Color selectionColor;
    public Color defaultcolor = new Color(0, 0, 0, 0);
    bool isPressedLR = false;
    bool isPressedUD = false;

    [SerializeField]
    GameObject settingsButton;

    // Use this for initialization
    void Start ()
    {
        //for (int i = 0; i < ItemManager.Instance.itemNames.Count; ++i)
        //{
        //    Image _icon = Instantiate(icon);
        //    _icon.transform.SetParent(parentBackground.transform, false);//Setting button parent 
        //    string itemName = ItemManager.Instance.itemNames[i];
        //    _icon.transform.GetChild(0).GetComponent<Image>().sprite = ItemManager.Instance.items[itemName].GetComponent<ItemBase>().sprite;
        //}
        selectedIndex = 0;
        panel.SetActive(InventoryManager.Instance.GetInventory("player").GetIsInventory());
        iconList = new List<GameObject>();
        //Setting the grid for the gridlayout
        for (int i = 0; i < 15; ++i)
        {
            GameObject _icon = Instantiate(icon);
            _icon.transform.SetParent(parentBackground.transform, false);//Setting button parent 
            IconPress iconPress = _icon.GetComponent<IconPress>();
            iconPress.id = i;
            iconList.Add(_icon);

            string itemName = null;
            itemName = InventoryManager.Instance.GetInventory("player").slot[i];
            if (itemName != null)
                iconList[i].transform.GetChild(1).GetComponent<Image>().sprite
                    = ItemManager.Instance.items[itemName].GetComponent<ItemBase>().sprite;
        }

        Debug.Log("NumberOfItems: " + ItemManager.Instance.items.Count);
        
    }
	

    void Update()
    {
        if (settingsButton)
        {
            if (Input.GetButtonDown("StartButton"))
            {
                settingsButton.GetComponent<Button>().onClick.Invoke();
            }
        }

        if (!InventoryManager.Instance.GetInventory("player").GetIsInventory())
            return;


        //Get client player component playershoot
        iconList[selectedIndex].transform.GetChild(0).GetComponent<Image>().color = selectionColor;
        iconList[Global.Instance.player.GetComponent<PlayerShoot>().itemIndex].transform.GetChild(0).GetComponent<Image>().color = equipedColor;

        //Below are ways to change the selected index and item index

        //pressed right
        if (Input.GetAxis("DPadXAxis") > 0.5 && !isPressedLR)
        {
            iconList[selectedIndex].transform.GetChild(0).GetComponent<Image>().color = defaultcolor;
            selectedIndex = Mathf.Clamp(selectedIndex + 1, 0, parentBackground.transform.childCount - 1);
            isPressedLR = true;
        }
        else if (Input.GetAxis("DPadXAxis") < -0.5 && !isPressedLR)
        {
            iconList[selectedIndex].transform.GetChild(0).GetComponent<Image>().color = defaultcolor;
            selectedIndex = Mathf.Clamp(selectedIndex - 1, 0, parentBackground.transform.childCount - 1);
            isPressedLR = true;
        }
        else if (isPressedLR && Mathf.Approximately(Input.GetAxis("DPadXAxis"), 0.0f))
        {
            isPressedLR = false;
        }

        //up
        GridLayoutGroup glg = parentBackground.GetComponent<GridLayoutGroup>();
        int constrantCount = glg.constraintCount;
        if (Input.GetAxis("DPadYAxis") > 0.5 && !isPressedUD)
        {
            iconList[selectedIndex].transform.GetChild(0).GetComponent<Image>().color = defaultcolor;
            selectedIndex = Mathf.Clamp(selectedIndex - constrantCount, 0, parentBackground.transform.childCount - 1);
            isPressedUD = true;
        }
        else if (Input.GetAxis("DPadYAxis") < -0.5 && !isPressedUD)
        {
            //down
            iconList[selectedIndex].transform.GetChild(0).GetComponent<Image>().color = defaultcolor;
            selectedIndex = Mathf.Clamp(selectedIndex + constrantCount, 0, parentBackground.transform.childCount - 1);
            isPressedUD = true;
        }
        else if (isPressedUD && Mathf.Approximately(Input.GetAxis("DPadYAxis"), 0.0f))
            isPressedUD = false;

        if (Input.GetButtonDown("A"))
        {
            //reset previous equiped color
            iconList[Global.Instance.player.GetComponent<PlayerShoot>().itemIndex].transform.GetChild(0).GetComponent<Image>().color = defaultcolor;
            Global.Instance.player.GetComponent<PlayerShoot>().itemIndex = selectedIndex;
        }

        //FOR MOBILE / MOUSE
        List<IconPress> clickedIcons = new List<IconPress>();
        foreach (GameObject icon in iconList)
        {
            IconPress iconPress = icon.GetComponent<IconPress>();
            if (iconPress.isClicked)
                clickedIcons.Add(iconPress);
        }
        float iconElapsed = float.MaxValue;
        IconPress mostRecent = null;
        foreach (IconPress icon in clickedIcons)
        {
            //find the shortest time, means most recent so that is the selected one
            if (icon.clickElapsed < iconElapsed)
            {
                if (mostRecent)
                    mostRecent.Reset(); //reset the previous icon
                mostRecent = icon;
            }
        }
        if (mostRecent)
        {
            selectedIndex = mostRecent.id;
            if (mostRecent.doubleClicked)
            {
                iconList[Global.Instance.player.GetComponent<PlayerShoot>().itemIndex].transform.GetChild(0).GetComponent<Image>().color = defaultcolor;
                Global.Instance.player.GetComponent<PlayerShoot>().itemIndex = selectedIndex;
                mostRecent.Reset();
            }
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

    public void ToggleInventory()
    {
        Inventory inventory = InventoryManager.Instance.GetInventory("player");
        if (!inventory.GetIsInventory())
            inventory.SetIsInventory(true);
        else
            inventory.SetIsInventory(false);
    }
    
}
