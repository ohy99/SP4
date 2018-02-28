using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpeedRoomItemScript : MonoBehaviour {

    SpeedRoomItemSpawner spawner;
    public int roomID;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 2.0f);
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
            if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            {
                SendMessageUpwards("AddCollect");
                col.gameObject.SendMessage("AddScore", 5);
            }
            else
            {
                Global.Instance.roomGen.GetRoomList()[roomID].
                   GetComponent<SpeedRoomScript>().AddCollect();
            }

            Destroy(gameObject);
        }
    }

    public void SetSpawner(SpeedRoomItemSpawner spawner)
    {
        this.spawner = spawner;
    }
}
