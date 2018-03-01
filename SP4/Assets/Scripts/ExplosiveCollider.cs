using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCollider : MonoBehaviour
{

    [SerializeField]
    float countDown = 0.25f;
    private float damage = 1;
    private bool isAttacking = false;

    // Use this for initialization
    void Start()
    {
        Debug.Log("explosive collider start");
        isAttacking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            Debug.Log("minusing: " + countDown);
            countDown -= Time.deltaTime;
            //transform.Rotate(Vector3.forward * -1.5f * Mathf.Rad2Deg * Time.deltaTime);
            if (countDown <= 0)
            {
                Debug.Log("delete");
                gameObject.SetActive(false);
                Destroy(gameObject);
            }

        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("hit");
        if (!coll.gameObject.CompareTag(this.gameObject.tag))
        {
            //Enters when the tags are different
            Health hpScript = coll.gameObject.GetComponent<Health>();
            if (hpScript)
            {
                //hpScript
                hpScript.ModifyHp(-damage);
                Debug.Log("Projectile: " + damage + " name: " + coll.gameObject.name + " maxhp: " + hpScript.GetMaxHp());

                DamageFeedback.Instance.ShowDamage(damage, coll.gameObject.transform.position + coll.gameObject.transform.lossyScale);
                switch (coll.gameObject.tag)
                {
                    case "Enemy":
                        ParticleManager.Instance.GenerateParticle(ParticleManager.PARTICLETYPE.HITENEMY, coll.gameObject.transform.position);
                        break;
                    case "Player":
                        ParticleManager.Instance.GenerateParticle(ParticleManager.PARTICLETYPE.HITPLAYER, coll.gameObject.transform.position);
                        break;
                }

            }

        }

        Destroy(gameObject);
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public void SetIsAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    public float GetCountDown()
    {
        return countDown;
    }

    public void SetCountDown(float _countDown)
    {
        countDown = _countDown;
    }

    public void SetDamage(float dmg) { damage = dmg; }
}
