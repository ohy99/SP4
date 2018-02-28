using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleEndScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D Other)
    {
        Debug.Log("Enter");
        if (Other.tag.Equals("Objective"))
        {
            Debug.Log("Send");
            gameObject.SendMessageUpwards("OnTarget");
            //if(!Global.Instance.player.GetComponent<NetworkIdentity>().isServer)

        }


    }
}
