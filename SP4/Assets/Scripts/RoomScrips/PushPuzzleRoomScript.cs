using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PushPuzzleRoomScript : RoomScript {

    [SerializeField]
    GameObject genericSpawner;
    [SerializeField]
    GameObject objective;

    GameObject player;
    GameObject[] playersList;

    Vector3 ObjectivePos;
    Vector3 TargetPos;

    RoomScript roomScript;
    //List<bool> doorList = new List<bool>();

    List<DoorInfo> doorInfoList = new List<DoorInfo>();

    //bool puzzleComplete;

    float elapsedTime;
    float timer;
    bool isLock;
    bool isPosSet;
    // Use this for initialization

    void Start()
    {
        //Random.Range(1, 1);
        //wad = new ArrayList();
        Debug.Log("PuzzleRoomStart");

        roomScript = this.GetComponent<RoomScript>();

        playersList = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("numPlayer: " + playersList.Length);
        for (int i = 0; i < playersList.Length; i++)
        {
            if (playersList[i].GetComponent<NetworkIdentity>().isLocalPlayer == true)
            {
                player = playersList[i];
                break;
            }
        }
        //if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        //{
        
        if(!isPosSet || player.GetComponent<NetworkIdentity>().isServer)
        {
            //Debug.Log("IS SETTING AGN");
            ObjectivePos.Set(Random.Range((transform.position.x - transform.localScale.x * 0.5f + 2.0f), transform.position.x), Random.Range((transform.position.y - transform.localScale.y * 0.5f + 2.0f), (transform.position.y + transform.localScale.y * 0.5f - 2.0f)), 0);
            TargetPos.Set(Random.Range((transform.position.x + transform.localScale.x * 0.5f - 2.0f), transform.position.x), Random.Range((transform.position.y - transform.localScale.y * 0.5f + 2.0f), (transform.position.y + transform.localScale.y * 0.5f - 2.0f)), 0);

            //transform.GetChild(0).position = ObjectivePos;
            transform.GetChild(0).position = TargetPos;

            if (player.GetComponent<NetworkIdentity>().isServer)
                genericSpawner.GetComponent<GenericSpawner>().SpawnObject(ObjectivePos, objective);
        }

        if (player.GetComponent<NetworkIdentity>().isServer)
            MessageHandler.Instance.SendPushPuzzle_S2C(1, roomScript.GetRoomID(), ObjectivePos, TargetPos);
        //else
        //{
        //    Debug.Log("pushRommId: " + roomScript.GetRoomID());
        //    MessageHandler.Instance.SendPushPuzzle_C2S(roomScript.GetRoomID());
        //}

        //doorList.Add(roomScript.GetIsLocked(DIRECTION.LEFT)) ;
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.RIGHT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.UP));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.DOWN));

        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.LEFT), roomScript.GetHasTriggerBox(DIRECTION.LEFT), DIRECTION.LEFT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.RIGHT), roomScript.GetHasTriggerBox(DIRECTION.RIGHT), DIRECTION.RIGHT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.UP), roomScript.GetHasTriggerBox(DIRECTION.UP), DIRECTION.UP));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.DOWN), roomScript.GetHasTriggerBox(DIRECTION.DOWN), DIRECTION.DOWN));
        
        elapsedTime = 0.0f;
        timer = 5.0f;

        puzzleComplete = false;
        isLock = false;

        Debug.Log("minDist: " + (transform.localScale.x * 0.5f - 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        timer -= Time.deltaTime;


        //if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
        //   Debug.Log(Vector3.Distance(player.transform.position, transform.position));


        float dist = Vector3.Distance(player.GetComponent<NetworkIdentity>().transform.position, transform.position);
        if (dist < transform.localScale.x * 0.5f - 2.0f && !puzzleComplete)
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

        if (timer <= 0 && !puzzleComplete)
        {
            timer = 5.0f;
            SendMessage("SpawnEnemy");
        }

        if (!Global.Instance.player.GetComponent<NetworkIdentity>().isServer
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

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 15;

        float yPos = 0.0f;

        if (!puzzleComplete)
        {
            if (elapsedTime < 5.0)
            {
                GUI.TextField(new Rect(Screen.width * 0.5f - 110.0f, yPos, 220.0f, 20.0f), "Push the red circle to the blue circle", 50, style);
                yPos += 10.0f;
            }
            GUI.TextField(new Rect(Screen.width * 0.5f - 110.0f, yPos, 220.0f, 20.0f), "Enemy spawning in " + timer, 50, style);
        }
    }


    void OnTarget()
    {
        puzzleComplete = true;

        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            MessageHandler.Instance.SendUnlockDoor_S2C(roomScript.GetRoomID(), puzzleComplete);
        //else
           // MessageHandler.Instance.SendUnlockDoor_C2S(roomScript.GetRoomID(), puzzleComplete);

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
        //transform.GetChild(0).position = ObjectivePos;
        transform.GetChild(0).position = TargetPos;
    }

    public Vector3 GetObjectivePos()
    {
        return ObjectivePos;
    }

    public Vector3 GetTargetPos()
    {
        return TargetPos;
    }

    public void SetObjectivePos(Vector3 _objectPos)
    {
        //ObjectivePos = _objectPos;
        //transform.GetChild(0).position = ObjectivePos;
        isPosSet = true;
    }

    public void SetTargetPos(Vector3 _targetPos)
    {
        TargetPos = _targetPos;
        transform.GetChild(0).position = TargetPos;
        isPosSet = true;
    }

    public override void LockAllDoor()
    {
        Debug.Log("LOCK DOOR_Room -> " + roomScript.GetRoomID());

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