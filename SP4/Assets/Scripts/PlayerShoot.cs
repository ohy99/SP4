using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //[SerializeField]
    //GameObject playerObj;

    private GameObject go;
    private Inventory playerGear;
    // Use this for initialization
    void Start ()
    {
        playerGear = new Inventory();

        playerGear.Init();
        go = Instantiate(playerGear.GetItem("Crossbow"));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(go.GetComponent<RangeWeaponBase>())
                go.GetComponent<RangeWeaponBase>().Discharge(transform.position,transform.rotation);
            else if (go.GetComponent<MeleeWeaponBase>())
                go.GetComponent<MeleeWeaponBase>().Attack(transform.position, transform.rotation);
        }

        if(Input.GetKey(KeyCode.R))
        {
            go.GetComponent<RangeWeaponBase>().Reload();
        }
    }
}
