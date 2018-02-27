using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossRoomScript : RoomScript {

    [SerializeField]
    GameObject boss;
    [SerializeField]
    GameObject genericSpawner;

    GameObject[] bossList;

	// Use this for initialization
	void Start () {
		
	}
    void Awake() {
        Debug.Log("boss awkae");

        //GameObject spawnBoss = Instantiate(boss, transform.position, Quaternion.identity);
        //spawnBoss.transform.parent = this.transform;
        //Global.Instance.boss = spawnBoss;

        //cahnge to be in update ltr
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        {
            Debug.Log("Spwning da boss");
            GameObject spawnBoss = genericSpawner.GetComponent<GenericSpawner>().SpawnObject(transform.position, boss);
            spawnBoss.transform.parent = this.transform;
            Global.Instance.boss = spawnBoss;
        }
        else
        {
            Debug.Log("not server/host");
            bossList = GameObject.FindGameObjectsWithTag("EnemyBoss");
            Debug.Log(bossList.Length);
        }   

    }

    // Update is called once per frame
    void Update()
    {
        //might puts this in global instead, but use it like this for now
        //playersList = GameObject.FindGameObjectsWithTag("Player");
        //for (int i = 0; i < playersList.Length; i++)
        //{
        //    if (Vector3.Distance(playersList[i].transform.position, transform.position) < transform.localScale.x * 0.5f - 2.0f)
        //    {
        //        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        //        {
        //            Debug.Log("Spwning da boss");
        //            GameObject spawnBoss = genericSpawner.GetComponent<GenericSpawner>().SpawnObject(transform.position, boss);
        //            spawnBoss.transform.parent = this.transform;
        //            Global.Instance.boss = spawnBoss;
        //        }
        //        else
        //            Debug.Log("not server/host");
        //    }
        //}

    }
}
