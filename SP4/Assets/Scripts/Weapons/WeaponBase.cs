using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected float damage;
    protected float damageOverTime;

    protected int totalRounds;
   
    protected int maxMagRounds;
    protected int magRounds;

    public GameObject projectile;

    public WeaponBase()
    {
    }

    // Use this for initialization
    virtual public void Start()
    {
    }

    // Update is called once per frame
    virtual public void Update ()
    {
	}

    // Fire Weapon 
    virtual public void Discharge(Vector3 pos, Quaternion rotation)
    {
        Debug.Log("weaponDischarge");
    }

    // Reload Weapon
    virtual public void Reload()
    {
    }
}
