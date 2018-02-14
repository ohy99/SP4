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

    // Use this for initialization
    void Start ()
    {
        playerGear = new Inventory();

        playerGear.Init();
        go = playerGear.GetItem("Crossbow");
        //go = Instantiate(playerGear.GetItem("Crossbow"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyUp(KeyCode.Tab))
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
            if(go.GetComponent<RangeWeaponBase>())
                go.GetComponent<RangeWeaponBase>().Discharge(transform.position,transform.rotation);
            else if (go.GetComponent<MeleeWeaponBase>())
                go.GetComponent<MeleeWeaponBase>().Attack(transform.position + transform.up, transform.rotation);
        }

        if(Input.GetKey(KeyCode.R))
        {
            go.GetComponent<RangeWeaponBase>().Reload();
        }
    }
}
