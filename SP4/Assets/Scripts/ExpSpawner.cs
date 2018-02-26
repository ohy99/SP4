using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExpSpawner : MonoBehaviour {

    int numOfActiveExp;
    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject expObj;
    [SerializeField]
    GameObject genericSpawner;

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

        if(!Global.Instance.player)
            return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= spawnDelay && numOfActiveExp < maxSpawns)
        {
            if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            {
                Vector3 pos = new Vector3(Random.Range(-map.transform.localScale.x * 0.5f, map.transform.localScale.x * 0.5f),
        Random.Range(-map.transform.localScale.y * 0.5f, map.transform.localScale.y * 0.5f), 0);
                genericSpawner.GetComponent<GenericSpawner>().SpawnObject(pos, expObj);
            }
                
            //GameObject temp = Instantiate(expObj, pos, Quaternion.identity);
            //temp.GetComponent<ExpObjScript>().SetSpawner(this);
            ++numOfActiveExp;
            elapsedTime = 0.0f;
            //Debug.Log(numOfActiveExp);
        }
	}

    public void RemoveOne() { --numOfActiveExp; }
}
