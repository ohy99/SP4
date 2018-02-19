using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 10;
    private float damage = 1;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * projectileSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log(coll.gameObject.name);
        if (!coll.gameObject.CompareTag(this.gameObject.tag))
        {
            //Enters when the tags are different
            Health hpScript = coll.gameObject.GetComponent<Health>();
            if (hpScript)
            {
                //hpScript
                hpScript.ModifyHp(-damage);
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

            //TODO: DO DAMAGE
            Destroy(gameObject);
        }
    }

    public void SetDamage(float dmg) { damage = dmg; }
}
