using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPuzzleRoomScript : RoomScript
{
    GameObject player;

    Transform OriginalGroup;
    Transform ChangedGroup;

    GameObject changedObject;

    RoomScript roomScript;

    //List<bool> doorList = new List<bool>();
    List<DoorInfo> doorInfoList = new List<DoorInfo>();

    bool puzzleComplete;

    bool wrongSelection;

    float elapsedTime;
    float textTimer;

    // Use this for initialization

    void Start()
    {
        //Random.Range(1, 1);
        //wad = new ArrayList();

        OriginalGroup = this.transform.GetChild(0);
        ChangedGroup = this.transform.GetChild(1);

        for(int i =0; i < OriginalGroup.childCount; ++i)
        {
            Vector3 newPos = new Vector3(Random.Range(transform.position.x - transform.localScale.x * 0.5f + 2.5f, transform.position.x - 2.5f), Random.Range(transform.position.y - transform.localScale.y * 0.5f + 2.5f, transform.position.y + transform.localScale.y * 0.5f - 2.5f), 1);
            OriginalGroup.GetChild(i).position = newPos;
            newPos.x += transform.localScale.x * 0.5f;
            ChangedGroup.GetChild(i).position = newPos;
        }

        changedObject = ChangedGroup.GetChild(Random.Range(0, 4)).gameObject;

        Color newColor = changedObject.GetComponent<SpriteRenderer>().color;
        newColor.r = Random.Range(0.0f,1.0f);
        newColor.g = Random.Range(0.0f, 1.0f);
        newColor.b = Random.Range(0.0f, 1.0f);
        newColor.a = Random.Range(0.0f, 1.0f);

        changedObject.GetComponent<SpriteRenderer>().color = newColor;

        roomScript = this.GetComponent<RoomScript>();

        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.LEFT), roomScript.GetHasTriggerBox(DIRECTION.LEFT), DIRECTION.LEFT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.RIGHT), roomScript.GetHasTriggerBox(DIRECTION.RIGHT), DIRECTION.RIGHT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.UP), roomScript.GetHasTriggerBox(DIRECTION.UP), DIRECTION.UP));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.DOWN), roomScript.GetHasTriggerBox(DIRECTION.DOWN), DIRECTION.DOWN));

        //doorList.Add(roomScript.GetIsLocked(DIRECTION.LEFT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.RIGHT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.UP));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.DOWN));

        elapsedTime = 0.0f;

        puzzleComplete = false;

        player = Global.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        //Debug.Log(player.transform.position);

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

        if (textTimer < elapsedTime)
            wrongSelection = false;

    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 15;

        float yPos = 0.0f;

        if (!puzzleComplete)
        {
            if (elapsedTime < 5.0f)
            {
                GUI.TextField(new Rect(Screen.width * 0.5f - 150.0f, yPos, 220.0f, 20.0f), "Spot the difference (Collide into the object to select)", 100, style);
                yPos += 10.0f;
            }

            if (wrongSelection)
            {
                GUI.TextField(new Rect(Screen.width * 0.5f - 60.0f, yPos, 220.0f, 20.0f), "Wrong Answer", 100, style);
            }
        }
    }

    void OnTarget(GameObject target)
    {
        //if (doorList.Count == 0)
        //   return;

        if (puzzleComplete)
            return;

        if (target != changedObject)
        {
            wrongSelection = true;
            textTimer = elapsedTime + 2.0f;
            SendMessage("SpawnEnemy");
            return;
        }

        puzzleComplete = true;

        foreach (DoorInfo doorinfo in doorInfoList)
        {
            if (!doorinfo.isLocked)
                roomScript.UnlockDoor(doorinfo.dir);
            if (!doorinfo.haveTriggerBox)
                roomScript.OffTriggerBox(doorinfo.dir);
        }
        


        //if (doorList[0])
        //    roomScript.LockDoor(DIRECTION.LEFT);
        //else
        //    roomScript.UnlockDoor(DIRECTION.LEFT);

        //if (doorList[1])
        //    roomScript.LockDoor(DIRECTION.RIGHT);
        //else
        //    roomScript.UnlockDoor(DIRECTION.RIGHT);

        //if (doorList[2])
        //    roomScript.LockDoor(DIRECTION.UP);
        //else
        //    roomScript.UnlockDoor(DIRECTION.UP);

        //if (doorList[3])
        //    roomScript.LockDoor(DIRECTION.DOWN);
        //else
        //    roomScript.UnlockDoor(DIRECTION.DOWN);
    }
}
