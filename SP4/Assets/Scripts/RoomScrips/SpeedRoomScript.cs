using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRoomScript : RoomScript {

    GameObject player;

    RoomScript roomScript;

    //List<bool> doorList = new List<bool>();
    List<DoorInfo> doorInfoList = new List<DoorInfo>();

    SpeedRoomItemSpawner spawnerScript;

    GUIStyle style;

    GUIContent content;

    int itemCollected;

    bool puzzleComplete;

    float elapsedTime;
    float textTimer;

    int fontSize = 50;
    string text;
    float textWidth;
    float screenCenter;

    Vector2 size;

    // Use this for initialization

    void Start()
    {
        //Random.Range(1, 1);
        //wad = new ArrayList();
        roomScript = this.GetComponent<RoomScript>();
        spawnerScript = this.transform.GetChild(0).GetComponent<SpeedRoomItemSpawner>();

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

        style = new GUIStyle();

        style.alignment = TextAnchor.UpperCenter;

        content = new GUIContent();

        itemCollected = 0;
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

    }

    void OnGUI()
    {
        style.fontSize = Mathf.Min(Mathf.FloorToInt(Screen.width * fontSize / 1000), Mathf.FloorToInt(Screen.height * fontSize / 1000));
        style.alignment = TextAnchor.UpperCenter;

        screenCenter = Screen.width * 0.5f;

        float yPos = 0.0f;

        if (elapsedTime < 5.0f && !spawnerScript.spawnerActive)
        {
            text = "Collect as many coins as possible";
            content.text = text;
            size = style.CalcSize(content);
            GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
            yPos += size.y;
            text = "Colide into the middle circle to start";
            content.text = text;
            size = style.CalcSize(content);
            GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
            yPos += size.y;
        }

        if (spawnerScript.spawnerActive && !puzzleComplete)
        {
            text = "Item: " + spawnerScript.numOfItemSpawned + "/" + spawnerScript.maxSpawns;
            content.text = text;
            size = style.CalcSize(content);
            GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
            yPos += size.y;
            text = "You collected: " + itemCollected;
            content.text = text;
            size = style.CalcSize(content);
            GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
            yPos += size.y;
        }
    }

    void AddCollect()
    {
        itemCollected++;
    }

    void SpawnEnemies(int maxItem)
    {
        StartCoroutine("EnemySpawn", maxItem - itemCollected);
    }

    IEnumerator EnemySpawn(int numberOfWavesToSpawn)
    {
        Debug.Log(numberOfWavesToSpawn);
        for (int i = 0; i < numberOfWavesToSpawn; ++i)
        {
            yield return new WaitForSeconds(3.0f);
            SendMessage("SpawnEnemy");
        }

        puzzleComplete = true;

        foreach (DoorInfo doorInfo in doorInfoList)
        {
            if (!doorInfo.isLocked)
                roomScript.UnlockDoor(doorInfo.dir);
            if (!doorInfo.haveTriggerBox)
                roomScript.OffTriggerBox(doorInfo.dir);
        }

    }
}
