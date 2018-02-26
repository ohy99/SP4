using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossRoomScript : RoomScript {

    [SerializeField]
    GameObject boss;
    [SerializeField]
    GameObject genericSpawner;

	// Use this for initialization
	void Start () {
		
	}
    void Awake() {

        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        {
            GameObject spawnBoss = genericSpawner.GetComponent<GenericSpawner>().SpawnObject(transform.position, boss);
            spawnBoss.transform.parent = this.transform;
            Global.Instance.boss = spawnBoss;
        }
        else
            Debug.Log("not server/host");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
