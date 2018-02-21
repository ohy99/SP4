using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureKey : MonoBehaviour {

    int treasureRoomID;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTreasureRoomID(int treasureRoomID)
    {
        this.treasureRoomID = treasureRoomID;
    }

    public int GetTreasureRoomID()
    {
        return treasureRoomID;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            transform.parent = col.transform;
            transform.SetAsFirstSibling();
        }
    }
}
