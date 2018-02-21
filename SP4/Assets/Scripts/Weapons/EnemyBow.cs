using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBow : RangeWeaponBase
{

    // Use this for initialization
    public override void Start()
    {
        damage = 10;
        damageOverTime = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    // Fire Weapon 
    public override GameObject Discharge(Vector3 pos, Quaternion rotation)
    {
        //Debug.Log("weaponDischarge");
        GameObject go =Instantiate(projectile, pos, rotation);
        return go;
    }

}
