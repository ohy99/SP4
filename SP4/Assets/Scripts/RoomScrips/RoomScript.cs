using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomScript : MonoBehaviour {

    int numOfOpenedDoors;

    [SerializeField]
    DoorScript leftDoor;
    [SerializeField]
    DoorScript rightDoor;
    [SerializeField]
    DoorScript upDoor;
    [SerializeField]
    DoorScript downDoor;

    protected class DoorInfo
    {
        public bool isLocked;
        public bool haveTriggerBox;
        public DIRECTION dir;
        public DoorInfo(bool islock, bool triggerbox, DIRECTION dir) { this.isLocked = islock; this.haveTriggerBox = triggerbox; this.dir = dir; }
    }

    ArrayList roomList;
    int roomID;
    int gridx;
    int gridy;
    public int GetGridX() { return gridx; }
    public int GetGridY() { return gridy; }

    // Use this for initialization
    void Start () {
        //Random.Range(1, 1);
        //wad = new ArrayList();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void GenerateRoom(DIRECTION side)
    {
        RoomGenerator.Instance.SetRoomActive(roomID, side);
    }

    //sets the roomid, and the grid x, y
    public void Set(int roomID, int x, int y, bool openLeft, bool openRight, bool openUp, bool openDown)
    {
        this.roomID = roomID;
        this.gridx = x;
        this.gridy = y;

        //Debug.Log("Left: " + openLeft + " Right: " + openRight + " Up: " + openUp + " Down: " + openDown);

        leftDoor.ToggleLock(!openLeft);
        rightDoor.ToggleLock(!openRight);
        upDoor.ToggleLock(!openUp);
        downDoor.ToggleLock(!openDown);
    }

    public void OffTriggerBox(DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.LEFT: leftDoor.OffTriggerBox(); break;
            case DIRECTION.RIGHT: rightDoor.OffTriggerBox(); break;
            case DIRECTION.UP: upDoor.OffTriggerBox(); break;
            case DIRECTION.DOWN: downDoor.OffTriggerBox(); break;
        }
    }
    public void OnTriggerBox(DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.LEFT: leftDoor.OnTriggerBox(); break;
            case DIRECTION.RIGHT: rightDoor.OnTriggerBox(); break;
            case DIRECTION.UP: upDoor.OnTriggerBox(); break;
            case DIRECTION.DOWN: downDoor.OnTriggerBox(); break;
        }
    }

    public bool GetIsLocked(DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.LEFT: return leftDoor.GetIsLocked();
            case DIRECTION.RIGHT:
                return rightDoor.GetIsLocked();
            case DIRECTION.UP:
                return upDoor.GetIsLocked();
            case DIRECTION.DOWN:
                return downDoor.GetIsLocked();
        }
        return false;
    }
    public bool GetHasTriggerBox(DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.LEFT: return leftDoor.GetHasTriggerBox();
            case DIRECTION.RIGHT:
                return rightDoor.GetHasTriggerBox();
            case DIRECTION.UP:
                return upDoor.GetHasTriggerBox();
            case DIRECTION.DOWN:
                return downDoor.GetHasTriggerBox();
        }
        return false;
    }

    public void UnlockDoor(DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.LEFT: leftDoor.ToggleLock(false); break;
            case DIRECTION.RIGHT: rightDoor.ToggleLock(false); break;
            case DIRECTION.UP:  upDoor.ToggleLock(false); break;
            case DIRECTION.DOWN:  downDoor.ToggleLock(false); break;
        }
    }
    public void LockDoor(DIRECTION side)
    {
        switch (side)
        {
            case DIRECTION.LEFT: leftDoor.ToggleLock(true); break;
            case DIRECTION.RIGHT: rightDoor.ToggleLock(true); break;
            case DIRECTION.UP: upDoor.ToggleLock(true); break;
            case DIRECTION.DOWN: downDoor.ToggleLock(true); break;
        }
    }

    void GenerateTreasureKey(GameObject TreasureKey)
    {
        Transform keyParent = RoomGenerator.Instance.GenerateKeyPos(roomID);
        GameObject temp = Instantiate(TreasureKey, keyParent);
        Debug.Log(temp);
        temp.GetComponent<TreasureKey>().SetTreasureRoomID(roomID);
    }

    public int GetRoomID()
    {
        return roomID;
    }
}
