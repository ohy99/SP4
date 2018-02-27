using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : RangeWeaponBase
{
    public AudioClip shootEffect;

    // Use this for initialization
    public override void Start ()
    {
        damage = 10;
        damageOverTime = 0;

        totalRounds = 9999999;

        maxMagRounds = 8;
        magRounds = 8;
        timer = 0.0f;
        fireRate = 0.4f;
    }

    // Update is called once per frame
    public override void Update ()
    {
        //Debug.Log("WEAPON_UPDATE");
        timer += Time.deltaTime;
	}


    // Fire Weapon 
    public override GameObject Discharge(Vector3 pos, Quaternion rotation)
    {
        if (fireRate < timer)
        {
            timer = 0.0f;
            SoundManager.Instance.PlayOneShot(shootEffect);
            GameObject projGO = Instantiate(projectile, pos, rotation);
            Projectile projScript = projGO.GetComponent<Projectile>();
            if (projScript)
                projScript.SetDamage(damage);
            //--magRounds;
            return projGO;
        }

        return null;
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
