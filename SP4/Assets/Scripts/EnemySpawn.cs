using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    [SerializeField]
    GameObject enemy;

	// Use this for initialization
	void Start () {
        StartCoroutine("SpawnEnemy", 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnEnemy(float waitTime)
    {
        while(true)
        {
            yield return new WaitForSeconds(waitTime);
            Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
            Instantiate(enemy, position, Quaternion.identity);
        }
    }
}
