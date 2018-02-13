using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTerrain : MonoBehaviour {

    [SerializeField]
    GameObject terrainBlock;
    //[SerializeField]
    //GameObject map;
    [SerializeField]
    float minSpawnScale = 1;
    [SerializeField]
    float maxSpawnScale = 10;

	// Use this for initialization
	void Start () {
        Random.InitState(0);
        int numOfSpawns = Random.Range(10, 20);
        for (int i = 0; i < numOfSpawns; ++i)
        {
            Vector3 pos = new Vector3();
            pos.Set(Random.Range(-this.transform.localScale.x * 0.5f, this.transform.localScale.x * 0.5f),
                Random.Range(-this.transform.localScale.y * 0.5f, this.transform.localScale.y * 0.5f), 1);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Vector3 scale = new Vector3(Random.Range(minSpawnScale, maxSpawnScale),
                Random.Range(minSpawnScale, maxSpawnScale), 1);
            Debug.Log(scale);
            GameObject temp = Instantiate(terrainBlock, pos, rotation);
            temp.transform.localScale =  new Vector3(scale.x, scale.y, 1);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
