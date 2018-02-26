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
    float timer;

    int fontSize = 60;

    GUIContent content;

    GUIStyle style;

    float screenCenter;

    string text;

    Vector2 size;

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
        timer = 5.0f;

        style = new GUIStyle();

        content = new GUIContent();

        puzzleComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        timer -= Time.deltaTime;

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

        if (timer <= 0 && !puzzleComplete)
        {
            timer = 5.0f;
            SendMessage("SpawnEnemy");
        }
    }

    void OnGUI()
    {
        style.fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * fontSize / 1000), Mathf.FloorToInt(Screen.height * fontSize / 1000));
        style.alignment = TextAnchor.UpperCenter;

        screenCenter = Screen.width * 0.5f;

        float yPos = 0.0f;

        if (!puzzleComplete)
        {
            if (elapsedTime < 5.0)
            {
                text = "Push the red circle to the blue circle";
                content.text = text;
                size = style.CalcSize(content);
                GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
                yPos += size.y;
            }
            text = "Enemy spawning in " + timer.ToString("F2");
            content.text = text;
            size = style.CalcSize(content);
            GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
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

    void ResetPuzzle()
    {
        transform.GetChild(0).position = ObjectivePos;
        transform.GetChild(1).position = TargetPos;
    }
}