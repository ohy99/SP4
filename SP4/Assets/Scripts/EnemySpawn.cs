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
        Vector3 pos = new Vector3(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
           Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        Instantiate(enemyMelee, pos, Quaternion.identity);

        pos.Set(Random.Range(map.transform.localPosition.x + -map.transform.localScale.x * 0.5f + 2.0f, map.transform.localPosition.x + map.transform.localScale.x * 0.5f - 2.0f),
           Random.Range(map.transform.localPosition.y + -map.transform.localScale.y * 0.5f + 2.0f, map.transform.localPosition.y + map.transform.localScale.y * 0.5f - 2.0f), 0);
        Instantiate(enemyRange, pos, Quaternion.identity);
    }

    //[Command]
    //void SpawnEnemy()
    //{
    //    Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    //    var enemyGo = (GameObject)Instantiate(enemy, position, Quaternion.identity);

    //    NetworkServer.Spawn(enemyGo);
    //}
}
