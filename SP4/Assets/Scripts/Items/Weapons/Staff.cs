using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : RangeWeaponBase
{
    // Use this for initialization
    public override void Start()
    {
        damage = 10;
        damageOverTime = 0;

        totalRounds = 900000;

        maxMagRounds = 5;
        magRounds = 5;
        timer = 0.0f;
        fireRate = 0.05f;
    }

    // Update is called once per frame
    public override void Update()
    {
        timer += Time.deltaTime;
    }

    // Fire Weapon 
    public override GameObject Discharge(Vector3 pos, Quaternion rotation)
    {
        if (fireRate < timer)
        {
            timer = 0.0f;
            Debug.Log("Staff_Attack");
            SoundManager.Instance.PlayOneShot(shootEffect);
            GameObject go = Instantiate(projectile, pos, rotation);
            Projectile projScript = go.GetComponent<Projectile>();
            if (projScript)
            {
                projScript.SetDamage(damage);
                projScript.projectileSpeed = 15;
            }
            //--magRounds;
            return go;
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
