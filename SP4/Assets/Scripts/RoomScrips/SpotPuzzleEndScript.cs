using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPuzzleEndScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if (other.gameObject.CompareTag("Player"))
            SendMessageUpwards("OnTarget", this.gameObject);
    }
}
