using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : RangeWeaponBase
{
    // Use this for initialization
    public override void Start()
    {
        damage = 10;
        damageOverTime = 0;

        totalRounds = 9999999;

        maxMagRounds = 1;
        magRounds = 1;
        timer = 1.0f;
        fireRate = 1.0f;
    }

    // Update is called once per frame
    public override void Update()
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
            {
                projScript.SetDamage(damage);
                projScript.projectileSpeed = 5;
                projScript.isExplosive = true;
            }
            return projGO;
        }

        return null;
    }

    // Reload Weapon (not using)
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
}
