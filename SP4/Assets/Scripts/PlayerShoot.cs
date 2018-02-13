using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //public GameObject weapon;
    public GameObject go;

	// Use this for initialization
	void Start ()
    {
        go.GetComponent<WeaponBase>().Start();
        Debug.Log("init");
        //pWeapon = weapon.AddComponent<WeaponBase>() as WeaponBase;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            go.GetComponent<WeaponBase>().Discharge(transform.position,transform.rotation);
        }

        if(Input.GetKey(KeyCode.R))
        {
            go.GetComponent<WeaponBase>().Reload();
        }
    }
}
