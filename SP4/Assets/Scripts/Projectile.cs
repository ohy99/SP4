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
}
