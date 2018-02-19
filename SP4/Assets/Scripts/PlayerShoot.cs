using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //[SerializeField]
    //GameObject playerObj;

    private GameObject go;
    private Inventory playerGear;

    private int itemIndex = 0;

    enum CONTROLTYPE
    {
        KEYBOARD,
        GAMEPAD,
    }
    CONTROLTYPE controlType = CONTROLTYPE.KEYBOARD;

    // Use this for initialization
    void Start ()
    {
        //playerGear = new Inventory();
        //playerGear.Init();

        playerGear = InventoryManager.Instance.GetInventory("player");
        go = playerGear.GetItem("Crossbow");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(controlType);
        switch (controlType)
        {
            case CONTROLTYPE.KEYBOARD:
                KeyboardUpdate();
                break;
            case CONTROLTYPE.GAMEPAD:
                GamePadUpdate();
                break;
        }
    }

    void KeyboardUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ++itemIndex;
            if (itemIndex >= playerGear.GetItemNameList().Count)
                itemIndex = 0;

            string itemName = playerGear.GetItemName(itemIndex);
            Debug.Log("ItemChange" + itemIndex + " - " + itemName);
            go = playerGear.GetItem(itemName);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("TotalRounds:" + go)

            if (go.GetComponent<RangeWeaponBase>())
                go.GetComponent<RangeWeaponBase>().Discharge(transform.position, transform.rotation);
            else if (go.GetComponent<MeleeWeaponBase>())
                go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up, transform.rotation);
        }

        if (Input.GetKey(KeyCode.R))
        {
            go.GetComponent<RangeWeaponBase>().Reload();
        }

        //if (Input.GetKey(KeyCode.T))
        //{
        //    //GameObject temp = Instantiate(ItemManager.Instance.items["Crossbow"]);
        //    InventoryManager.Instance.GetInventory("player").AddItem(ItemManager.Instance.items["Crossbow"]
        //        , "Crossbow02");
        //    playerGear = InventoryManager.Instance.GetInventory("player");
        //}
    }

    void GamePadUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ++itemIndex;
            if (itemIndex >= playerGear.GetItemNameList().Count)
                itemIndex = 0;

            string itemName = playerGear.GetItemName(itemIndex);
            Debug.Log("ItemChange" + itemIndex + " - " + itemName);
            go = playerGear.GetItem(itemName);
        }


        //Debug.Log("TotalRounds:" + go)
        //Debug.Log(Input.GetAxis("RightHorizontal") + " , " + Input.GetAxis("RightVertical"));
        
        //if (!Mathf.Approximately(Input.GetAxis("RightHorizontal"), 0.0f) || !Mathf.Approximately(Input.GetAxis("RightVertical"), 0.0f))
        if (Input.GetAxis("LTRT") > 0.5)
        {
            //float hValue = Input.GetAxis("RightHorizontal");
            //float vValue = Input.GetAxis("RightVertical");
            //Vector3 shootDir = new Vector3(hValue, vValue, 0);
            ////gameObject.transform.position += moveDir * moveSpeed * Time.deltaTime;
            //go.transform.up = shootDir;

            if (go.GetComponent<RangeWeaponBase>())
                go.GetComponent<RangeWeaponBase>().Discharge(transform.position, transform.rotation);
            else if (go.GetComponent<MeleeWeaponBase>())
                go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up, transform.rotation);
        }

        

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A Pressed");
            go.GetComponent<RangeWeaponBase>().Reload();
        }
    }

    public void SetControlType(int type)
    {
        switch (type)
        {
            case 1:
                controlType = CONTROLTYPE.GAMEPAD;
                break;
            default:
                controlType = CONTROLTYPE.KEYBOARD;
                break;
        }
    }
}
