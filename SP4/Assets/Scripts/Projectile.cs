using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 10;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * projectileSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        //  Debug.Log(other.gameObject.name);
    }
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log(coll.gameObject.name);
        if (!coll.gameObject.CompareTag(this.gameObject.tag))
        {
            //Enters when the tags are different

            //TODO: DO DAMAGE
            Destroy(gameObject);
        }
    }
}
