using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBow : WeaponBase
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
    public override void Discharge(Vector3 pos, Quaternion rotation)
    {
        Debug.Log("weaponDischarge");
        Instantiate(projectile, pos, rotation);
    }

}
