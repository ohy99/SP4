using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackScript : MonoBehaviour {

    [SerializeField]
    int healthValue = 10;

    HealthSpawner spawner;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player") && col.gameObject.GetComponent<Player>().GetHealth() < col.gameObject.GetComponent<Player>().GetMaxHealth())//if the collided is player OR player(clone)
        {
            col.gameObject.SendMessage("AddHealth", healthValue);
            Destroy(gameObject);
            if (spawner != null)
                spawner.RemoveOne();
        }

    }

    void SetSpawner(HealthSpawner spawner)
    {
        this.spawner = spawner;
    }
}
