﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour {

    [SerializeField]
    GameObject TreasureKey;

    float timer = 2.0f;

    bool textTrigger = false;

    int fontSize = 60;

	// Use this for initialization
	void Start () {
        SendMessageUpwards("GenerateTreasureKey", TreasureKey);
	}
	
	// Update is called once per frame
	void Update () {
        if (textTrigger)
            timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 2.0f;
            textTrigger = false;
        }

	}

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * fontSize / 1000), Mathf.FloorToInt(Screen.height * fontSize / 1000));
        style.alignment = TextAnchor.UpperCenter;
        float screenCenter = Screen.width * 0.5f;

        string text;

        if (textTrigger)
        {
            text = "Find the key around the map to unlock this chest";

            GUI.Label(new Rect(screenCenter - text.Length * 1.5f, 0, 220.0f, 20.0f), text, style);
        }
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
            else if (!textTrigger)
            {
                textTrigger = true;
            }
        }
    }
}
