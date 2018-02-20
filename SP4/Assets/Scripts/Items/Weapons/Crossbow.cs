﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : RangeWeaponBase
{

	// Use this for initialization
	public override void Start ()
    {
        damage = 10;
        damageOverTime = 0;

        totalRounds = 80;

        maxMagRounds = 8;
        magRounds = 8;
        timer = 0.0f;
        fireRate = 0.4f;
    }

    // Update is called once per frame
    public override void Update ()
    {
        //Debug.Log("WEAPON_UPDATE");
        
	}

    // Fire Weapon 
    public override void Discharge(Vector3 pos, Quaternion rotation)
    {
        if (magRounds > 0)
        {
            Debug.Log("weaponDischarge");
            GameObject projGO = Instantiate(projectile, pos, rotation);
            Projectile projScript = projGO.GetComponent<Projectile>();
            if (projScript)
                projScript.SetDamage(damage);
            --magRounds;
        }
    }

    // Reload Weapon
    public override void Reload()
    {   
        // dont reload if mag is full or no more ammo
        if (maxMagRounds == magRounds || 
            totalRounds <= 0)
            return;

        if (totalRounds < maxMagRounds)
        {
            magRounds = totalRounds;
            totalRounds = 0;
        }
        else
        {
            totalRounds -= maxMagRounds;
            magRounds = maxMagRounds;
        }

    }

    public override void OnClick()
    {
    }
}
