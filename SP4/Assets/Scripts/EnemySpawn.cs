using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawn : NetworkBehaviour
{
    [SerializeField]
    GameObject enemyMelee;
    [SerializeField]
    GameObject enemyRange;
    [SerializeField]
    GameObject map;

    //float timer = 2.0f;

	// Use this for initialization
	void Start () {
        //NetworkIdentity
        gameObject.AddComponent<NetworkIdentity>();
        gameObject.GetComponent<NetworkIdentity>().serverOnly = true;
    }

	
	// Update is called once per frame
	void Update () {
        //timer += Time.deltaTime;
        //if(timer >= 2.0f)
        //{
        //    SpawnEnemy();
        //    timer = 0;
        //}

    }

    void SpawnEnemy()
    {
        //Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
        //   Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        //Instantiate(enemyMelee, pos, Quaternion.identity);

        //pos.Set(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
        //   Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        //Instantiate(enemyRange, pos, Quaternion.identity);
        NetSpawnEnemy();
    }

    void NetSpawnEnemy()
    {
        Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
            Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        var enemyGo = Instantiate(enemyMelee, pos, Quaternion.identity);

        NetworkServer.Spawn(enemyGo);

        pos.Set(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
           Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        enemyGo = Instantiate(enemyRange, pos, Quaternion.identity);

        NetworkServer.Spawn(enemyGo);
    }
}
