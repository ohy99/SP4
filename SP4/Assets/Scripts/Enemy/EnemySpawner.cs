using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    int totalWaves;
    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject enemyMelee;
    [SerializeField]
    GameObject enemyRange;

    float elapsedTime;
    [SerializeField]
    float spawnDelay = 5.0f;

    void Awake()
    {
        totalWaves = Random.Range(3, 10);
    }

    // Use this for initialization
    void Start()
    {
        totalWaves = Random.Range(3, 10);
        gameObject.AddComponent<NetworkIdentity>();
        gameObject.GetComponent<NetworkIdentity>().serverOnly = true;
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
            NetSpawnEnemy();
            //for (int j = 0; j < 3; ++j)
            //{
            //    Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
            //       Random.Range(map.transform.localPosition.y  + - map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
            //    Instantiate(enemyMelee, pos, Quaternion.identity);
            //}
            //for (int j = 0; j < 3; ++j)
            //{
            //    Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
            //       Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
            //    Instantiate(enemyRange, pos, Quaternion.identity);
            //}
        }

        gameObject.SendMessageUpwards("UnlockDoor");
    }

    void NetSpawnEnemy()
    {
        for (int j = 0; j < 3; ++j)
        {
            Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
               Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
            var enemy = Instantiate(enemyMelee, pos, Quaternion.identity);

            NetworkServer.Spawn(enemy);
        }
        for (int j = 0; j < 3; ++j)
        {
            Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
               Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
            var enemy = Instantiate(enemyRange, pos, Quaternion.identity);

            NetworkServer.Spawn(enemy);
        }
    }

    public void StartSpawner()
    {
        StartCoroutine("SpawnEnemy", spawnDelay);
    }
}


/*
 * room gen done by server, spawning of enemies by server, exp/hp pickup server,  
 */