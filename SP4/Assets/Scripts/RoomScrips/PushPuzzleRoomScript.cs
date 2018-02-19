using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPuzzleRoomScript : RoomScript {
    
    GameObject player;

    Vector3 ObjectivePos;
    Vector3 TargetPos;

    RoomScript roomScript;
    List<bool> doorList = new List<bool>();

    bool puzzleComplete;

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

        doorList.Add(roomScript.GetIsLocked(DIRECTION.LEFT)) ;
        doorList.Add(roomScript.GetIsLocked(DIRECTION.RIGHT));
        doorList.Add(roomScript.GetIsLocked(DIRECTION.UP));
        doorList.Add(roomScript.GetIsLocked(DIRECTION.DOWN));

        player = GameObject.FindGameObjectWithTag("Player");

        puzzleComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.transform.position);
        if (roomScript.GetGridX() != 0 || roomScript.GetGridY() != 0)
        {
            Debug.Log(transform.position);
            Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        }

        if (Vector3.Distance(player.transform.position, transform.position) < transform.localScale.x * 0.5f - 2.0f && !puzzleComplete)
        {
            Debug.Log("Locking");
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

    void OnTarget()
    {
        if (doorList.Count == 0)
            return;

        puzzleComplete = true;

        if (doorList[0])
            roomScript.LockDoor(DIRECTION.LEFT);
        else
            roomScript.UnlockDoor(DIRECTION.LEFT);

        if (doorList[1])
            roomScript.LockDoor(DIRECTION.RIGHT);
        else
            roomScript.UnlockDoor(DIRECTION.RIGHT);

        if (doorList[2])
            roomScript.LockDoor(DIRECTION.UP);
        else
            roomScript.UnlockDoor(DIRECTION.UP);

        if (doorList[3])
            roomScript.LockDoor(DIRECTION.DOWN);
        else
            roomScript.UnlockDoor(DIRECTION.DOWN);
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