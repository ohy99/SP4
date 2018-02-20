using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPuzzleRoomScript : RoomScript {
    
    GameObject player;

    Vector3 ObjectivePos;
    Vector3 TargetPos;

    RoomScript roomScript;
    //List<bool> doorList = new List<bool>();

    List<DoorInfo> doorInfoList = new List<DoorInfo>();

    bool puzzleComplete;

    float elapsedTime;

    // Use this for initialization

    void Start()
    {
        //Random.Range(1, 1);
        //wad = new ArrayList();
        ObjectivePos.Set(Random.Range((transform.position.x - transform.localScale.x * 0.5f + 2.0f), transform.position.x), Random.Range((transform.position.y - transform.localScale.y * 0.5f + 2.0f), (transform.position.y + transform.localScale.y * 0.5f - 2.0f)), 0);
        TargetPos.Set(Random.Range((transform.position.x + transform.localScale.x * 0.5f - 2.0f), transform.position.x), Random.Range((transform.position.y - transform.localScale.y * 0.5f + 2.0f), (transform.position.y + transform.localScale.y * 0.5f - 2.0f)), 0);

        transform.GetChild(0).position = ObjectivePos;
        transform.GetChild(1).position = TargetPos;

        roomScript = this.GetComponent<RoomScript>();

        //doorList.Add(roomScript.GetIsLocked(DIRECTION.LEFT)) ;
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.RIGHT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.UP));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.DOWN));

        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.LEFT), roomScript.GetHasTriggerBox(DIRECTION.LEFT), DIRECTION.LEFT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.RIGHT), roomScript.GetHasTriggerBox(DIRECTION.RIGHT), DIRECTION.RIGHT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.UP), roomScript.GetHasTriggerBox(DIRECTION.UP), DIRECTION.UP));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.DOWN), roomScript.GetHasTriggerBox(DIRECTION.DOWN), DIRECTION.DOWN));


        player = Global.Instance.player;

        elapsedTime = 0.0f;

        puzzleComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Vector3.Distance(player.transform.position, transform.position) < transform.localScale.x * 0.5f - 2.0f && !puzzleComplete)
        {
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
            style.fontSize = 15;
            GUI.TextField(new Rect(Screen.width * 0.5f - 110.0f,0,220.0f,20.0f)  , "Push the red circle to the blue circle", 50, style);
        }
    }


    void OnTarget()
    {
        puzzleComplete = true;

        foreach (DoorInfo doorInfo in doorInfoList)
        {
            if (!doorInfo.isLocked)
                roomScript.UnlockDoor(doorInfo.dir);
            if (!doorInfo.haveTriggerBox)
                roomScript.OffTriggerBox(doorInfo.dir);
        }

    }

    void OffTarget()
    {
        puzzleComplete = false;

        roomScript.LockDoor(DIRECTION.LEFT);
        roomScript.LockDoor(DIRECTION.RIGHT);
        roomScript.LockDoor(DIRECTION.UP);
        roomScript.LockDoor(DIRECTION.DOWN);
    }

    void ResetPuzzle()
    {
        transform.GetChild(0).position = ObjectivePos;
        transform.GetChild(1).position = TargetPos;
    }
}