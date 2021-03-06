﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeaponBase {

    // Use this for initialization
    public override void Start ()
    {
        damage = 10;
        damageOverTime = 0;
        range = 1.5f;
        isAttack = false;
        // meleeCollider.SetActive(false);
        timer = 0.0f;
        fireRate = 0.3f;
    }

    // Update is called once per frame
    public override void Update ()
    {
        timer += Time.deltaTime;
    }

    // Attack with weapon
    public override GameObject Attack(Vector3 pos, Quaternion rotation)
    {
        if (fireRate < timer)
        {
            timer = 0;
            meleeCollider.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // Spawn a aabb here if collision occur damage is done
            GameObject go = Instantiate(meleeCollider, pos, rotation);
            go.SetActive(true);
            SoundManager.Instance.PlayOneShot(shootEffect);
            MeleeCollider meleeScript = go.GetComponent<MeleeCollider>();
            if (meleeScript)
                meleeScript.SetDamage(damage);


            //Debug.Log(pos);
            //meleeCollider.GetComponent<BoxCollider2D>().offset = new Vector2(0, range);
            //meleeCollider.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.5f);
            //meleeCollider.transform.localScale = new Vector3(0.5f, range, 1);
            isAttack = true;
            return go;
        }

        return null;
    }

    public override void OnClick()
    {

    }
}
