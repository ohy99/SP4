using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomScript : RoomScript {

    [SerializeField]
    EnemyBoss boss;
	// Use this for initialization
	void Start () {
		
	}
    void Awake() {
        boss = Instantiate(boss, transform.position, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
