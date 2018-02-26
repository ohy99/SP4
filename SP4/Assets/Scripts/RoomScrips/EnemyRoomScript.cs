﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomScript : RoomScript {

    GameObject player;

    RoomScript roomScript;

    List<DoorInfo> doorInfoList = new List<DoorInfo>();

    GameObject Spawner;

    //List<bool> doorList = new List<bool>();

    float elapsedTime;

    bool completedWaves;

    int fontSize = 60;

    // Use this for initialization
    void Start()
    {
        roomScript = this.GetComponent<RoomScript>();

        //doorList.Add(roomScript.GetIsLocked(DIRECTION.LEFT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.RIGHT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.UP));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.DOWN));

        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.LEFT), roomScript.GetHasTriggerBox(DIRECTION.LEFT), DIRECTION.LEFT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.RIGHT), roomScript.GetHasTriggerBox(DIRECTION.RIGHT), DIRECTION.RIGHT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.UP), roomScript.GetHasTriggerBox(DIRECTION.UP), DIRECTION.UP));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.DOWN), roomScript.GetHasTriggerBox(DIRECTION.DOWN), DIRECTION.DOWN));

        Spawner = transform.GetChild(0).gameObject;

        player = Global.Instance.player;

        elapsedTime = 0.0f;
        completedWaves = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        elapsedTime += Time.deltaTime;

        if (Vector3.Distance(player.transform.position, transform.position) < transform.localScale.x * 0.5f - 2.0f && !completedWaves)
        {
            if (!Spawner.activeSelf)
            {
                Spawner.SetActive(true);
                Spawner.SendMessage("StartSpawner");
            }

            LockDoor(DIRECTION.LEFT);
            LockDoor(DIRECTION.RIGHT);
            LockDoor(DIRECTION.UP);
            LockDoor(DIRECTION.DOWN);

            OnTriggerBox(DIRECTION.LEFT);
            OnTriggerBox(DIRECTION.RIGHT);
            OnTriggerBox(DIRECTION.UP);
            OnTriggerBox(DIRECTION.DOWN);
        }

    }

    void OnGUI()
    {
        if (elapsedTime < 5.0f)
        {
            GUIStyle style = new GUIStyle();
            string text = "Survive the wave of enemies";
            float screenCenter = Screen.width * 0.5f;

            style.fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * fontSize / 1000), Mathf.FloorToInt(Screen.height * fontSize / 1000));

            style.alignment = TextAnchor.UpperCenter;

            GUI.Label(new Rect(screenCenter -text.Length * 1.5f, 0, 220.0f, 20.0f), text, style);
        }
    }


    void UnlockDoor()
    {
        completedWaves = true;

        foreach (DoorInfo doorInfo in doorInfoList)
        {
            if (!doorInfo.isLocked)
                roomScript.UnlockDoor(doorInfo.dir);
            if (!doorInfo.haveTriggerBox)
                roomScript.OffTriggerBox(doorInfo.dir);
        }

    }
}
