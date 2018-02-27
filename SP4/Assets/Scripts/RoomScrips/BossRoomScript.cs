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
    GameObject[] playersList;

    bool isBossSpawn;

	// Use this for initialization
	void Start () {
		
	}
    void Awake()
    {
        isBossSpawn = false;
        //GameObject spawnBoss = Instantiate(boss, transform.position, Quaternion.identity);
        //spawnBoss.transform.parent = this.transform;
        //Global.Instance.boss = spawnBoss;

        //cahnge to be in update ltr
        //if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        //{
        //    Debug.Log("Spwning da boss");
        //    GameObject spawnBoss = genericSpawner.GetComponent<GenericSpawner>().SpawnObject(transform.position, boss);
        //    spawnBoss.transform.parent = this.transform;
        //    Global.Instance.boss = spawnBoss;
        //}
        //else
        //{
        //    Debug.Log("not server/host");
        //    bossList = GameObject.FindGameObjectsWithTag("EnemyBoss");
        //    Debug.Log("boss_array_length: " + bossList.Length);
        //}   

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBossSpawn)
        {
            //might puts this in global instead, but use it like this for now
            playersList = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < playersList.Length; i++)
            {
                if (Vector3.Distance(playersList[i].transform.position, transform.position) < transform.localScale.x * 0.5f - 2.0f)
                    SpawnBoss();
            }
        }

        //boss not deaded
        if(!Global.Instance.boss)
        {
            bossList = GameObject.FindGameObjectsWithTag("EnemyBoss");
            if(bossList.Length > 0)
            {
                Global.Instance.boss = bossList[0];
                Global.Instance.boss.transform.parent = this.transform;
            }
        }
    }

    void SpawnBoss()
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        {
            Debug.Log("Spwning da boss");
            GameObject spawnBoss = genericSpawner.GetComponent<GenericSpawner>().SpawnObject(transform.position, boss);
            spawnBoss.transform.parent = this.transform;
            Global.Instance.boss = spawnBoss;
        }
        else
            Debug.Log("not server/host");

        //send a msg to say boss spawn
        isBossSpawn = true;

    }

}
