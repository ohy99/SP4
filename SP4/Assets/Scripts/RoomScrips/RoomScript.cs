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
	void Update () {
		
	}

    void GenerateRoom(DIRECTION side)
    {
        RoomGenerator.Instance.GenerateRoom(roomID, side);
    }

    //sets the roomid, and the grid x, y
    public void Set(int roomID, int x, int y, bool openLeft, bool openRight, bool openUp, bool openDown, DIRECTION forceOpenSide)
    {
        this.roomID = roomID;
        this.gridx = x;
        this.gridy = y;

        //Debug.Log("Left: " + openLeft + " Right: " + openRight + " Up: " + openUp + " Down: " + openDown);

        leftDoor.ToggleLock(!openLeft);
        rightDoor.ToggleLock(!openRight);
        upDoor.ToggleLock(!openUp);
        downDoor.ToggleLock(!openDown);

        OffTriggerBox(forceOpenSide);
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
}
