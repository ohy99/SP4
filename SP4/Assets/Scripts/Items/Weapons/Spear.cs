using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MeleeWeaponBase {

    // Use this for initialization
    public override void Start ()
    {
        damage = 10;
        damageOverTime = 0;
        range = 2.5f;
        isAttack = false;
        meleeCollider.SetActive(false);
	}

    // Update is called once per frame
    public override void Update () 
    {
    }

    // Attack with weapon (stab - long range)
    public override void Attack(Vector3 pos, Quaternion rotation)
    {
        Debug.Log("MeleeAttack");
        // Spawn a aabb here if collision occur damage is done
        meleeCollider.SetActive(true);
        Instantiate(meleeCollider, pos, rotation);
        meleeCollider.transform.localScale = new Vector3(0.5f, range, 1);
        isAttack = true;
    }
}
