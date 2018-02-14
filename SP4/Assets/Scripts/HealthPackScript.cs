using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackScript : MonoBehaviour {

    [SerializeField]
    int healthValue = 10;

    HealthSpawner spawner;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnDestroy()
    {
        if (spawner != null)
            spawner.RemoveOne();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))//if the collided is player OR player(clone)
        {
            col.gameObject.SendMessage("AddHealth", healthValue);
            Destroy(gameObject);
        }

    }

    public void SetSpawner(HealthSpawner spawner)
    {
        this.spawner = spawner;
    }
}
