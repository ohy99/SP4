using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCollider : MonoBehaviour
{

    [SerializeField]
    float countDown = 0.25f;
    private float damage = 13;
    private float timer;

    // Use this for initialization
    void Start()
    {
        Debug.Log("explosive collider start");
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= countDown)
        {
            timer = 0;
            Debug.Log("delete");
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("hit");
        if (!coll.gameObject.CompareTag(this.gameObject.tag) && coll.gameObject.tag != "Player")
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
