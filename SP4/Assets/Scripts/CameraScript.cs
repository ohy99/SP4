using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            return;

        //transform.position.Set(player.transform.position.x, player.transform.position.y, -10);
        transform.position = player.transform.position + new Vector3(0,0,-10);
        //Debug.Log(transform.position + ", player:" +player.transform.position.x +"," +player.transform.position.y);
	}
}
