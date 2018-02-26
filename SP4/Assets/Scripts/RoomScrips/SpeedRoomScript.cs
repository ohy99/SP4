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

    //[SyncVar]
    int itemCollected;

    float elapsedTime;
    float textTimer;

    bool isLock;

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

        isLock = false;
        puzzleComplete = false;

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
        GUIStyle style = new GUIStyle();
        style.fontSize = 15;

        float yPos = 0.0f;

        if (elapsedTime < 5.0f && !spawnerScript.spawnerActive)
        {
            GUI.TextField(new Rect(Screen.width * 0.5f - 110.0f, yPos, 220.0f, 20.0f), "Collect as many coins as possible", 100, style);
            yPos += 10.0f;
            GUI.TextField(new Rect(Screen.width * 0.5f - 110.0f, yPos, 220.0f, 20.0f), "Colide into the middle circle to start", 100, style);
            yPos += 10.0f;
        }

        if (spawnerScript.spawnerActive && !puzzleComplete)
        {
            GUI.TextField(new Rect(Screen.width * 0.5f - 10.0f, yPos, 220.0f, 20.0f), "Item: " + spawnerScript.numOfItemSpawned + "/" + spawnerScript.maxSpawns, 100, style);
            yPos += 10.0f;
            GUI.TextField(new Rect(Screen.width * 0.5f - 50.0f, yPos, 220.0f, 20.0f), "You collected: " + itemCollected, 100, style);
            yPos += 10.0f;
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

    public int _itemCollected { get { return itemCollected; } set { itemCollected = _itemCollected; } }
}
