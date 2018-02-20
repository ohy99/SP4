using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomScript : RoomScript {

    [SerializeField]
    GameObject boss;
	// Use this for initialization
	void Start () {
		
	}
    void Awake() {
        GameObject spawnBoss = Instantiate(boss, transform.position, Quaternion.identity);
        Global.Instance.boss = spawnBoss;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
