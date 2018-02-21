using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRoomItemScript : MonoBehaviour {

    SpeedRoomItemSpawner spawner;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        if (this.spawner)
            spawner.AddDestroyedObjectCount();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit");
        if (col.gameObject.tag.Equals("Player"))//if the collided is player OR player(clone)
        {
            SendMessageUpwards("AddCollect");
            Destroy(gameObject);
        }
    }

    public void SetSpawner(SpeedRoomItemSpawner spawner)
    {
        this.spawner = spawner;
    }
}
