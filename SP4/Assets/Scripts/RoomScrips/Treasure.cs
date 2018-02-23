using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour {

    [SerializeField]
    GameObject TreasureKey;


	// Use this for initialization
	void Start () {
        SendMessageUpwards("GenerateTreasureKey", TreasureKey);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            Debug.Log(col.transform.GetChild(0).tag);
            if (col.transform.GetChild(0).tag.Equals("Key") && col.transform.GetChild(0).GetComponent<TreasureKey>().GetTreasureRoomID() == this.GetComponentInParent<RoomScript>().GetRoomID())
            {
                //InventoryManager.Instance.GetInventory("player").AddCurrency(100);
                Destroy(this.gameObject);
                Destroy(col.transform.GetChild(0).gameObject);
            }
        }
    }
}
