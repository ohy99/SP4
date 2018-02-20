using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //[SerializeField]
    //GameObject playerObj;
    [SerializeField]
    Joystick joyStick;
    
    private GameObject go;
    private Inventory playerGear;

    private int itemIndex = 0;

    enum CONTROLTYPE
    {
        KEYBOARD,
        GAMEPAD,
        MOBILE,
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
            case CONTROLTYPE.GAMEPAD:
                GamePadUpdate();
                break;
            case CONTROLTYPE.MOBILE:
                MobileUpdate();
                break;
            default:
                KeyboardUpdate();
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

        if (Input.GetButton("Fire1"))
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

        if(Input.GetKeyUp(KeyCode.I))
        {
            if(!playerGear.GetIsInventory())
                playerGear.SetIsInventory(true);
            else
                playerGear.SetIsInventory(false);
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
        if (Input.GetButtonDown("LB"))
        {
            --itemIndex;
            if (itemIndex < 0)
                itemIndex = playerGear.GetItemNameList().Count - 1;

            string itemName = playerGear.GetItemName(itemIndex);
            Debug.Log("ItemChange" + itemIndex + " - " + itemName);
            go = playerGear.GetItem(itemName);
        }
        if (Input.GetButtonDown("RB"))
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

        

        if (Input.GetButtonDown("A"))
        {
            Debug.Log("Reloading");
            go.GetComponent<RangeWeaponBase>().Reload();
        }
    }

    void MobileUpdate()
    {
        if (!Mathf.Approximately(joyStick.GetXAxis(), 0.0f) || !Mathf.Approximately(joyStick.GetYAxis(), 0.0f))
        {
            float rhValue = joyStick.GetXAxis();
            float rvValue = joyStick.GetYAxis();
            Vector3 shootDir = new Vector3(rhValue, rvValue, 0);
            //gameObject.transform.position += moveDir * moveSpeed * Time.deltaTime;
            Transform playerTransform = transform.parent.parent; //hacks but fast heh
            //Debug.Log(playerTransform.gameObject);
            playerTransform.up = shootDir;

            if (go.GetComponent<RangeWeaponBase>())
                go.GetComponent<RangeWeaponBase>().Discharge(transform.position, transform.rotation);
            else if (go.GetComponent<MeleeWeaponBase>())
                go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up, transform.rotation);
        }
    }

    public void SetControlType(int type)
    {
        controlType = (CONTROLTYPE)type;
    }
}
