using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpotPuzzleRoomScript : RoomScript
{
    GameObject player;
    GameObject[] playersList;

    Transform OriginalGroup;
    Transform ChangedGroup;

    GameObject changedObject;

    RoomScript roomScript;

    //SpotPuzzleRoomMessage spotPuzzleMsg;

    //List<bool> doorList = new List<bool>();
    List<DoorInfo> doorInfoList = new List<DoorInfo>();

    //bool puzzleComplete;

    bool wrongSelection;
    bool isLock;

    float elapsedTime;
    float textTimer;

    int fontSize = 50;

    GUIStyle style;

    string text;

    Vector2 size;

    GUIContent content;

    float screenCenter;

    // Use this for initialization

    void Start()
    {
        //Random.Range(1, 1);
        //wad = new ArrayList();

        player = Global.Instance.player;

        OriginalGroup = this.transform.GetChild(0);
        ChangedGroup = this.transform.GetChild(1);
        int randIndex = 0;
        Color newColor = new Color(1,1,1);

        if (player.GetComponent<NetworkIdentity>().isServer)
        {
            for (int i = 0; i < OriginalGroup.childCount; ++i)
            {
                Vector3 newPos = new Vector3(Random.Range(transform.position.x - transform.localScale.x * 0.5f + 2.5f, transform.position.x - 2.5f), Random.Range(transform.position.y - transform.localScale.y * 0.5f + 2.5f, transform.position.y + transform.localScale.y * 0.5f - 2.5f), 1);
                OriginalGroup.GetChild(i).position = newPos;
                newPos.x += transform.localScale.x * 0.5f;
                ChangedGroup.GetChild(i).position = newPos;
            }

            randIndex = Random.Range(0, 4);
            changedObject = ChangedGroup.GetChild(randIndex).gameObject;

            newColor = changedObject.GetComponent<SpriteRenderer>().color;
            newColor.r = Random.Range(0.0f, 1.0f);
            newColor.g = Random.Range(0.0f, 1.0f);
            newColor.b = Random.Range(0.0f, 1.0f);
            newColor.a = Random.Range(0.0f, 1.0f);

            changedObject.GetComponent<SpriteRenderer>().color = newColor;
        }
       


        roomScript = this.GetComponent<RoomScript>();

        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.LEFT), roomScript.GetHasTriggerBox(DIRECTION.LEFT), DIRECTION.LEFT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.RIGHT), roomScript.GetHasTriggerBox(DIRECTION.RIGHT), DIRECTION.RIGHT));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.UP), roomScript.GetHasTriggerBox(DIRECTION.UP), DIRECTION.UP));
        doorInfoList.Add(new DoorInfo(roomScript.GetIsLocked(DIRECTION.DOWN), roomScript.GetHasTriggerBox(DIRECTION.DOWN), DIRECTION.DOWN));

        //doorList.Add(roomScript.GetIsLocked(DIRECTION.LEFT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.RIGHT));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.UP));
        //doorList.Add(roomScript.GetIsLocked(DIRECTION.DOWN));

        //if (spotPuzzleMsg != null)
        //    RepositionObjectClient(spotPuzzleMsg);

        elapsedTime = 0.0f;

        puzzleComplete = false;
        isCompleted = false;
        isLock = false;

        style = new GUIStyle();
        content = new GUIContent();

        //if is host/server
        if(player.GetComponent<NetworkIdentity>().isServer)
        {
            //send msg to clients
            SpotPuzzleRoomMessage message = new SpotPuzzleRoomMessage();
            message.roomId = roomScript.GetRoomID();
            message.shape01_pos = OriginalGroup.GetChild(0).position;
            message.shape02_pos = OriginalGroup.GetChild(1).position;
            message.shape03_pos = OriginalGroup.GetChild(2).position;
            message.shape04_pos = OriginalGroup.GetChild(3).position;
            message.changedObjectIndex = randIndex;
            message.changedObjectColor = newColor;

            MessageHandler.Instance.SendSpotPuzzle_S2C(message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        elapsedTime += Time.deltaTime;
        if (isCompleted || puzzleComplete)
            return;
        //Debug.Log(player.transform.position);

        // put a list of player connected in global and then cehck them
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

        if (textTimer < elapsedTime)
            wrongSelection = false;


        if(puzzleComplete)
        {
            foreach (DoorInfo doorinfo in doorInfoList)
            {
                if (!doorinfo.isLocked)
                    roomScript.UnlockDoor(doorinfo.dir);
                if (!doorinfo.haveTriggerBox)
                    roomScript.OffTriggerBox(doorinfo.dir);
            }
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
            if (elapsedTime < 5.0f)
            {
                text = "Spot the difference (Collide into the object to select)";
                content.text = text;
                size = style.CalcSize(content);
                GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
                yPos += size.y;
            }

            if (wrongSelection)
            {
                text = "Wrong Answer";
                content.text = text;
                size = style.CalcSize(content);
                GUI.Label(new Rect(screenCenter - size.x, yPos, size.x * 2.0f, size.y * 2.0f), text, style);
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
        isCompleted = true;
        //if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        //    MessageHandler.Instance.SendUnlockDoor_S2C(roomScript.GetRoomID(), puzzleComplete);
        //else
        //    MessageHandler.Instance.SendUnlockDoor_C2S(roomScript.GetRoomID(), puzzleComplete);

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

    public void RepositionObjectClient(SpotPuzzleRoomMessage _msg)
    {
        List<Vector3> tempList = new List<Vector3>();
        tempList.Add(_msg.shape01_pos);
        tempList.Add(_msg.shape02_pos);
        tempList.Add(_msg.shape03_pos);
        tempList.Add(_msg.shape04_pos);

        OriginalGroup = this.transform.GetChild(0);
        ChangedGroup = this.transform.GetChild(1);

        for (int i = 0; i < OriginalGroup.childCount; ++i)
        {
            Vector3 newPos = tempList[i];
            OriginalGroup.GetChild(i).position = newPos;
            newPos.x += transform.localScale.x * 0.5f;
            ChangedGroup.GetChild(i).position = newPos;
        }

        changedObject = ChangedGroup.GetChild(_msg.changedObjectIndex).gameObject;

        Color newColor = changedObject.GetComponent<SpriteRenderer>().color;
        newColor = _msg.changedObjectColor;

        changedObject.GetComponent<SpriteRenderer>().color = newColor;
    }

    //public void SetSpotPuzzleMsg(SpotPuzzleRoomMessage _msg)
    //{
    //    spotPuzzleMsg = _msg;
    //}
}
