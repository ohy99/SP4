using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawn : NetworkBehaviour
{
    [SerializeField]
    GameObject enemy;

    //float timer = 2.0f;

	// Use this for initialization
	void Start ()
    {
        //timer = 0.0f;
        StartCoroutine("SpawnEnemy", 2.0f);
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

    IEnumerator SpawnEnemy(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            SpawnEnemy();
            //Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
            //Instantiate(enemy, position, Quaternion.identity);
        }
    }

    //[Command]
    void SpawnEnemy()
    {
        Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
        var enemyGo = (GameObject)Instantiate(enemy, position, Quaternion.identity);

        NetworkServer.Spawn(enemyGo);
    }
}
