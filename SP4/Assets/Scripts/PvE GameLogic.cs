using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvEGameLogic : MonoBehaviour {

    [SerializeField]
    Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player.IsDead())
        {

        }
	}


}
