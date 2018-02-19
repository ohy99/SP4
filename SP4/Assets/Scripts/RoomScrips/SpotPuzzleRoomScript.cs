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
    List<bool> doorList = new List<bool>();

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

        Debug.Log(OriginalGroup.childCount);

        for(int i =0; i < OriginalGroup.childCount; ++i)
        {
            Vector3 newPos = new Vector3(Random.Range(transform.position.x - transform.localScale.x * 0.5f + 2.5f, transform.position.x - 2.5f), Random.Range(transform.position.y - transform.localScale.y * 0.5f + 2.5f, transform.position.y + transform.localScale.y * 0.5f - 2.5f), 1);
            OriginalGroup.GetChild(i).position = newPos;
            Debug.Log(newPos);
            newPos.x += transform.localScale.x * 0.5f;
            Debug.Log(newPos);
            ChangedGroup.GetChild(i).position = newPos;
        }

        changedObject = ChangedGroup.GetChild(Random.Range(0, 4)).gameObject;

        Color newColor = changedObject.GetComponent<SpriteRenderer>().color;
        newColor.r = Random.Range(0,1);
        newColor.g = Random.Range(0, 1);
        newColor.b = Random.Range(0, 1);

        changedObject.GetComponent<SpriteRenderer>().color = newColor;

        roomScript = this.GetComponent<RoomScript>();

        doorList.Add(roomScript.GetIsLocked(DIRECTION.LEFT));
        doorList.Add(roomScript.GetIsLocked(DIRECTION.RIGHT));
        doorList.Add(roomScript.GetIsLocked(DIRECTION.UP));
        doorList.Add(roomScript.GetIsLocked(DIRECTION.DOWN));

        elapsedTime = 0.0f;

        puzzleComplete = false;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        //Debug.Log(player.transform.position);
        if (roomScript.GetGridX() != 0 || roomScript.GetGridY() != 0)
        {
            Debug.Log(transform.position);
            Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        }

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
        if (elapsedTime < 5.0f)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 15;
            GUI.TextField(new Rect(Screen.width * 0.5f - 150.0f, 0, 220.0f, 20.0f), "Spot the difference (Collide into the object to select)", 100, style);
        }

        if(wrongSelection)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 10;
            GUI.TextField(new Rect(Screen.width * 0.5f - 80.0f, 0, 220.0f, 20.0f), "Wrong Answer", 100, style);
        }
    }

    void OnTarget(GameObject target)
    {
        if (doorList.Count == 0)
            return;

        if (target != changedObject)
        {
            wrongSelection = true;
            textTimer = elapsedTime + 2.0f;
            return;
        }

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
}
