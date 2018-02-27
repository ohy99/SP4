using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBase : ItemBase
{
    protected float damage;
    protected float damageOverTime;
    protected float range;
    protected bool isAttack;

    [SerializeField]
    protected GameObject meleeCollider;

    public MeleeWeaponBase()
    {
    }

    // Use this for initialization
    public override void Start()
    {
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    // Fire Weapon 
    virtual public GameObject Attack(Vector3 pos, Quaternion rotation)
    {
        return null;
    }

    // Getter/Setter
    virtual public float GetDamage()
    {
        return damage;
    }

    virtual public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    virtual public float GetDamageOverTime()
    {
        return damageOverTime;
    }

    virtual public void SetDamageOverTime(float _damageOverTime)
    {
        damageOverTime = _damageOverTime;
    }

    virtual public float GetRange()
    {
        return range;
    }

    virtual public void SetRange(float _range)
    {
        range = _range;
    }

    virtual public bool GetIsAttack()
    {
        return isAttack;
    }

    virtual public void SetIsAttack(bool _isAttack)
    {
        isAttack = _isAttack;
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
