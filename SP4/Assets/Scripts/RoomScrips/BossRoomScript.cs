using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomScript : RoomScript {

    [SerializeField]
    List<GameObject> bossList = new List<GameObject>();
	// Use this for initialization
	void Start () {
        int randIndex = Random.Range(0, bossList.Count - 1);
        GameObject spawnBoss = Instantiate(bossList[randIndex], transform.position, Quaternion.identity);
        spawnBoss.transform.parent = this.transform;
        Global.Instance.boss = spawnBoss;
    }
    void Awake() {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
