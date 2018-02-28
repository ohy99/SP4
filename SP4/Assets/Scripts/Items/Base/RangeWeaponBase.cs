using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBase : ItemBase
{
    protected float damage;
    protected float damageOverTime;

    protected int totalRounds;
   
    protected int maxMagRounds;
    protected int magRounds;
    protected float fireRate;
    protected float timer;

    [SerializeField]
    protected AudioClip shootEffect;

    [SerializeField]
    protected GameObject projectile;

    public RangeWeaponBase()
    {
    }

    // Use this for initialization
    public override void Start()
    {
    }

    // Update is called once per frame
    public override void Update ()
    {
	}

    // Fire Weapon 
    virtual public GameObject Discharge(Vector3 pos, Quaternion rotation)
    {
        Debug.Log("weaponDischarge");
        return null;
    }

    // Reload Weapon
    virtual public void Reload()
    {
    }

    // Get projectile go
    virtual public GameObject GetGameObject()
    {
        return projectile;
    }

    virtual public void SetGameObject(GameObject _go)
    {
        projectile = _go;
    }

    public override ITEM_TYPE ItemType()
    {
        return itemType;
    }

    public override void SetItemType(ITEM_TYPE _itemType)
    {
        itemType = _itemType;
    }

    public override void OnClick()
    {
    }
}
