using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObjScript : MonoBehaviour {

    [SerializeField]
    int expValue = 10;

    ExpSpawner spawner;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.IndexOf("Player") == 0)//if the collided is player OR player(clone)
        {
            col.gameObject.SendMessage("IncExp", expValue);
            Destroy(gameObject);
            if (spawner != null)
                spawner.RemoveOne();
        }

    }

    void SetSpawner(ExpSpawner spawner)
    {
        this.spawner = spawner;
    }
}
