using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpeedRoomScript : RoomScript {

    GameObject player;
    GameObject[] playersList;

    RoomScript roomScript;

    //List<bool> doorList = new List<bool>();
    List<DoorInfo> doorInfoList = new List<DoorInfo>();

    SpeedRoomItemSpawner spawnerScript;

    GUIStyle style;

    GUIContent content;

    int itemCollected;

    float elapsedTime;
    float textTimer;

    int fontSize = 50;
    string text;
    float textWidth;
    float screenCenter;

    Vector2 size;

    bool isLock;

    // Use this for initialization
    void Start()
    {
        //Random.Range(1, 1);
        //wad = new ArrayList();
        Debug.Log("speed start");
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

        isLock = false;
        puzzleComplete = false;
        isCompleted = false;
        //playersList = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log("numPlayer: " + playersList.Length);
        //for (int i = 0; i < playersList.Length; i++)
        //{
        //    if (playersList[i].GetComponent<NetworkIdentity>().isLocalPlayer == true)
        //    {
        //        player = playersList[i];
        //        break;
        //    }
        //}

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
        if (isCompleted || puzzleComplete)
            return;
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

            if (!isLock)
            {
                if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
                    MessageHandler.Instance.SendLockDoor_S2C(roomScript.GetRoomID());
                else
                    MessageHandler.Instance.SendLockDoor_C2S(roomScript.GetRoomID());
                isLock = true;
            }
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

    

        if(!Global.Instance.player.GetComponent<NetworkIdentity>().isServer
            && puzzleComplete == true)
        {
            foreach (DoorInfo doorInfo in doorInfoList)
            {
                if (!doorInfo.isLocked)
                    roomScript.UnlockDoor(doorInfo.dir);
                if (!doorInfo.haveTriggerBox)
                    roomScript.OffTriggerBox(doorInfo.dir);
            }
        }
    }

    public void AddCollect()
    {
        //Debug.Log("TrueRoomId: " + GetRoomID());
        itemCollected++;

        if (player.GetComponent<NetworkIdentity>().isServer)
            MessageHandler.Instance.SendItemCollected_S2C(roomScript.GetRoomID(), itemCollected);
        else
            MessageHandler.Instance.SendItemCollected_C2S(roomScript.GetRoomID(), itemCollected);
    }

    void SpawnEnemies(int maxItem)
    {
        StartCoroutine("EnemySpawn", maxItem - itemCollected);
    }

    IEnumerator EnemySpawn(int numberOfWavesToSpawn)
    {
        Debug.Log("waves: " + numberOfWavesToSpawn);
        for (int i = 0; i < numberOfWavesToSpawn; ++i)
        {
            yield return new WaitForSeconds(3.0f);
            SendMessage("SpawnEnemy");
        }

        puzzleComplete = true;
        isCompleted = true;
        Global.Instance.roomGen.roomDataList[roomScript.GetRoomID()].isCompleted = true;
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            MessageHandler.Instance.SendUnlockDoor_S2C(roomScript.GetRoomID(), puzzleComplete);
        //else
        //    MessageHandler.Instance.SendUnlockDoor_C2S(roomScript.GetRoomID(), puzzleComplete);

        foreach (DoorInfo doorInfo in doorInfoList)
        {
            if (!doorInfo.isLocked)
                roomScript.UnlockDoor(doorInfo.dir);
            if (!doorInfo.haveTriggerBox)
                roomScript.OffTriggerBox(doorInfo.dir);
        }
    }

    public override void LockAllDoor()
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

    public SpeedRoomItemSpawner GetSpawnerScript()
    {
        return spawnerScript;
    }

    public void SetSpawnerScript()
    {
        spawnerScript = this.transform.GetChild(0).GetComponent<SpeedRoomItemSpawner>();
    }

    public void SetItemCollected(int _itemCollected)
    {
        itemCollected = _itemCollected;
        Debug.Log("itemCollected: " + itemCollected);
    }
    //public int _itemCollected { get { return itemCollected; } set { itemCollected = _itemCollected; } }
}
