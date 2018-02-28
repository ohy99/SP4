using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerShoot : NetworkBehaviour
{
    //[SerializeField]
    //GameObject playerObj;
    [SerializeField]
    Joystick joyStick;

    [SerializeField]
    Dictionary<string, Sprite> weaponSprite = new Dictionary<string, Sprite>();
    
    private GameObject go;
    private GameObject playerWeapon;
    private Inventory playerGear;

    [System.NonSerialized]
    public int itemIndex = 0;

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

        foreach(string item in playerGear.GetItemNameList())
        {
            string weaponName = "WeaponSprite/" + item;
            Texture2D tex = (Texture2D)Resources.Load(weaponName);
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            weaponSprite.Add(item, Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f)));
        }

        playerWeapon = this.transform.GetChild(0).gameObject;

        playerWeapon.GetComponent<SpriteRenderer>().sprite = weaponSprite["Crossbow"];

        playerWeapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        //Debug.Log(controlType);
        switch (controlType)
        {
            case CONTROLTYPE.GAMEPAD:
                GamePadUpdate();
                if (!joyStick)
                    break;
                if (!Mathf.Approximately(joyStick.GetXAxis(), 0.0f) || !Mathf.Approximately(joyStick.GetYAxis(), 0.0f))
                    controlType = (CONTROLTYPE.MOBILE);
                break;
            case CONTROLTYPE.MOBILE:
                MobileUpdate();
                break;
            default:
                KeyboardUpdate();
                if (!joyStick)
                    break;
                if (!Mathf.Approximately(joyStick.GetXAxis(), 0.0f) || !Mathf.Approximately(joyStick.GetYAxis(), 0.0f))
                    controlType = (CONTROLTYPE.MOBILE);
                break;
        }


        string itemName = playerGear.GetItemName(itemIndex);
        Debug.Log("ItemChange" + itemIndex + " - " + itemName);
        go = playerGear.GetItem(itemName);
        playerWeapon.GetComponent<SpriteRenderer>().sprite = weaponSprite[itemName];
    }

    void KeyboardUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ++itemIndex;
            if (itemIndex >= playerGear.GetItemNameList().Count)
                itemIndex = 0;

            //string itemName = playerGear.GetItemName(itemIndex);
            //Debug.Log("ItemChange" + itemIndex + " - " + itemName);
            //go = playerGear.GetItem(itemName);
            //playerWeapon.GetComponent<SpriteRenderer>().sprite = weaponSprite[itemName];
        }

        if (Input.GetButton("Fire1"))
        {
            //Debug.Log("TotalRounds:" + go)
            CmdFire(gameObject.tag);

            //if (go.GetComponent<RangeWeaponBase>())
            //    go.GetComponent<RangeWeaponBase>().Discharge(transform.position + transform.up, transform.rotation);
            //else if (go.GetComponent<MeleeWeaponBase>())
            //    go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up, transform.rotation);
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

            //string itemName = playerGear.GetItemName(itemIndex);
            //Debug.Log("ItemChange" + itemIndex + " - " + itemName);
            //go = playerGear.GetItem(itemName);
        }
        if (Input.GetButtonDown("RB"))
        {
            ++itemIndex;
            if (itemIndex >= playerGear.GetItemNameList().Count)
                itemIndex = 0;
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

            CmdFire(gameObject.tag);

           
            //if (go.GetComponent<RangeWeaponBase>())
            //    go.GetComponent<RangeWeaponBase>().Discharge(transform.position, transform.rotation);
            //else if (go.GetComponent<MeleeWeaponBase>())
            //    go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up, transform.rotation);
        }

        

        if (Input.GetButtonDown("A"))
        {
            Debug.Log("Reloading");
            go.GetComponent<RangeWeaponBase>().Reload();
        }
        if (Input.GetButtonDown("Y"))
        {
            if (!playerGear.GetIsInventory())
                playerGear.SetIsInventory(true);
            else
                playerGear.SetIsInventory(false);
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
            Transform playerTransform = gameObject.transform; //hacks but fast heh
            //Debug.Log(playerTransform.gameObject);
            playerTransform.up = shootDir;

            CmdFire(gameObject.tag);

            //if (go.GetComponent<RangeWeaponBase>())
            //    go.GetComponent<RangeWeaponBase>().Discharge(transform.position, transform.rotation);
            //else if (go.GetComponent<MeleeWeaponBase>())
            //    go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up, transform.rotation);
        }
    }

    public void SetControlType(int type)
    {
        controlType = (CONTROLTYPE)type;
    }


    [Command]
    void CmdFire(string tag)
    {
        GameObject projectile = null;
        if (go.GetComponent<RangeWeaponBase>())
            projectile = go.GetComponent<RangeWeaponBase>().Discharge(transform.position + transform.up, transform.rotation);
        else if (go.GetComponent<MeleeWeaponBase>())
            projectile = go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up * go.GetComponent<MeleeWeaponBase>().GetRange(), transform.rotation);

        if (!projectile)
            return;

        //Debug.Log(projectile);
        switch (tag)
        {
            case "Player":
                projectile.layer = (int)PROJLAYER.PLAYERPROJ;
                break;
            case "Enemy":
                projectile.layer = (int)PROJLAYER.ENEMYPROJ;
                break;
        }
        //Debug.Log(projectile.layer);

        if(projectile)
            NetworkServer.Spawn(projectile);

        Destroy(projectile, 3.0f);
    }


}
