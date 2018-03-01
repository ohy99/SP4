using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum PROJLAYER
{
    ENEMYPROJ = 9,
    PLAYERPROJ = 11,
}

//public enum PROJECTILE_TYPE //last min stuff
//{
//    BULLET,
//    BOMB,
//}

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 10;
    public bool isExplosive = false;
    private float damage = 1;
    // public PROJECTILE_TYPE pType;
    [SerializeField]
    GameObject explosiveCollider;
    [SerializeField]
    GameObject genericSpawner;

    // Use this for initialization
    void Start ()
    {
       // pType = PROJECTILE_TYPE.BULLET;
        Destroy(gameObject, 5.0f);
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
                //Debug.Log("Projectile: " + damage + " name: " + coll.gameObject.name + " maxhp: " + hpScript.GetMaxHp());

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

            if (isExplosive)
            {
                if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
                    genericSpawner.GetComponent<GenericSpawner>().SpawnObject(transform.position, explosiveCollider);
            }

            Destroy(gameObject);
        }
    }

    public void SetDamage(float dmg) { damage = dmg; }
}
