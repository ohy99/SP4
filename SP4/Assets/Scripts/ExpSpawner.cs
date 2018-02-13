using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSpawner : MonoBehaviour {

    int numOfActiveExp;
    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject expObj;

    float elapsedTime;
    [SerializeField]
    float spawnDelay = 3.0f;
    [SerializeField]
    int maxSpawns = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnDelay)
        {
            Vector3 pos = new Vector3(Random.Range(-map.transform.localScale.x * 0.5f, map.transform.localScale.x * 0.5f),
            Random.Range(-map.transform.localScale.y * 0.5f, map.transform.localScale.y * 0.5f), 0 );
            GameObject temp = Instantiate(expObj, pos, Quaternion.identity);
            ++numOfActiveExp;
            elapsedTime = 0.0f;
        }
	}

    public void RemoveOne() { --numOfActiveExp; }
}
