using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : MonoBehaviour
{
    int totalWaves;
    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject enemyMelee;
    [SerializeField]
    GameObject enemyRange;

    [SerializeField]
    GameObject genericSpawner;

    float elapsedTime;
    [SerializeField]
    float spawnDelay = 5.0f;

    GameObject player;
    GameObject[] playersList;

    void Awake()
    {
        totalWaves = Random.Range(3, 10);
    }

    // Use this for initialization
    void Start()
    {
        //totalWaves = Random.Range(3, 10);
        //gameObject.AddComponent<NetworkIdentity>();
        //gameObject.GetComponent<NetworkIdentity>().serverOnly = true;
        playersList = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("numPlayer: " + playersList.Length);
        for (int i = 0; i < playersList.Length; i++)
        {
            if (playersList[i].GetComponent<NetworkIdentity>().isLocalPlayer == true)
            {
                player = playersList[i];
                break;
            }
        }

        if (player.GetComponent<NetworkIdentity>().isServer)
        {
            //send to all client the no of item spawned
            MessageHandler.Instance.SendNumberToSpawn_S2C(map.GetComponent<RoomScript>().GetRoomID(), totalWaves, "enemyRoom");
        }

        StartCoroutine("SpawnEnemy", spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemy(float waitTime)
    {
        for (int i = 0; i < totalWaves; ++i)
        {
            yield return new WaitForSeconds(waitTime);
            CallSpawner();
        }

        gameObject.SendMessageUpwards("UnlockDoor");
    }

    void CallSpawner()
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        {
            genericSpawner.GetComponent<GenericSpawner>().Init(map);
            genericSpawner.GetComponent<GenericSpawner>().SpawnObject(3, enemyMelee);
            genericSpawner.GetComponent<GenericSpawner>().SpawnObject(3, enemyRange);
        }
        else
            Debug.Log("not server/host");
        
        //for (int j = 0; j < 3; ++j)
        //{
        //    Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
        //       Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        //    var enemy = Instantiate(enemyMelee, pos, Quaternion.identity);

        //    NetworkServer.Spawn(enemy);
        //}
        //for (int j = 0; j < 3; ++j)
        //{
        //    Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
        //       Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        //    var enemy = Instantiate(enemyRange, pos, Quaternion.identity);

        //    NetworkServer.Spawn(enemy);
        //}
    }

    public void StartSpawner()
    {
        StartCoroutine("SpawnEnemy", spawnDelay);
    }

    public void SetTotalWave(int _totalWaves)
    {
        totalWaves = _totalWaves;
    }
}


/*
 * room gen done by server, spawning of enemies by server, exp/hp pickup server,  
 */