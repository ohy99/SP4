using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BossRoomScript : RoomScript {

    [SerializeField]
    List<GameObject> bossPrefabList = new List<GameObject>();

    [SerializeField]
    GameObject genericSpawner;

    GameObject[] bossList;
    GameObject[] playersList;

    bool isBossSpawn;
    int randIndex;

    // Use this for initialization
    void Start ()
    {
        isBossSpawn = false;
        isCompleted = false;
        randIndex = Random.Range(0, bossPrefabList.Count - 1);
        //GameObject spawnBoss = Instantiate(bossPrefabList[randIndex], transform.position, Quaternion.identity);
        //spawnBoss.transform.parent = this.transform;
        //Global.Instance.boss = spawnBoss;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBossSpawn && !isCompleted)
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
            GameObject spawnBoss = genericSpawner.GetComponent<GenericSpawner>().SpawnObject(transform.position, bossPrefabList[randIndex]);
            spawnBoss.transform.parent = this.transform;
            Global.Instance.boss = spawnBoss;
        }
        else
            Debug.Log("not server/host");

        //send a msg to say boss spawn
        isBossSpawn = true;
    }

}
