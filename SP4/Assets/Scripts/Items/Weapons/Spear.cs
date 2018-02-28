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

        timer = 0.0f;
        fireRate = 0.5f;
    }

    // Update is called once per frame
    public override void Update () 
    {
        timer += Time.deltaTime;
    }

    // Attack with weapon (stab - long range)
    public override GameObject Attack(Vector3 pos, Quaternion rotation)
    {
        Debug.Log("MeleeAttack");
        // Spawn a aabb here if collision occur damage is done
        if (fireRate < timer)
        {
            timer = 0;
            meleeCollider.SetActive(true);
            GameObject go = Instantiate(meleeCollider, pos, rotation);
            SoundManager.Instance.PlayOneShot(shootEffect);
            MeleeCollider meleeScript = go.GetComponent<MeleeCollider>();
            if (meleeScript)
                meleeScript.SetDamage(damage);

            meleeCollider.transform.localScale = new Vector3(0.5f, 2.5f, 1);
            //meleeCollider.transform.position = new Vector3(0, meleeCollider.transform.position.y + range, 0);
            //meleeCollider.GetComponent<BoxCollider2D>().offset = new Vector2(0, range);
            //meleeCollider.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 2.5f);
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
