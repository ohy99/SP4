using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //To catch argument exception

public enum DIRECTION
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN
}


public class RoomGenerator : Singleton<RoomGenerator> {

    [SerializeField]
    GameObject defaultRoom;
    //[SerializeField]
    //GameObject room1;
    //[SerializeField]
    //GameObject room2;
    //[SerializeField]
    //GameObject room3;

    //Array of rooms with ID
    List<GameObject> roomList;
    int currID;
    float zOffset = 1;

    [SerializeField]
    float scaleX = 20;
    [SerializeField]
    float scaleY = 20;
    //Positional representation of rooms
    Dictionary<int, Dictionary<int, GameObject>> roomMap;//y , x
    int smallestX = 0; int smallestY = 0;
    int biggestX = 0; int biggestY = 0;

    int numOfOpenedDoors;

	// Use this for initialization
	void Start () {
        numOfOpenedDoors = 0;
        roomList = new List<GameObject>();
        roomMap = new Dictionary<int, Dictionary<int, GameObject>>();
        currID = 0;

        //spawn at 0,0, so generate one at 0,0
        GameObject room = Instantiate(defaultRoom, new Vector3(0, 0, zOffset), Quaternion.identity);
        room.transform.localScale.Set(scaleX, scaleY, 1);

        RoomScript roomScript = room.GetComponent<RoomScript>();
        Dictionary<DIRECTION, bool> boolArray = GetDoorOpenBooleans(DIRECTION.NONE);
        roomScript.Set(currID, 0, 0, boolArray[DIRECTION.LEFT], boolArray[DIRECTION.RIGHT], boolArray[DIRECTION.UP], boolArray[DIRECTION.DOWN], DIRECTION.NONE);
        StoreRoom(currID, 0, 0, room);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateRoom(int currRoomID, DIRECTION side)
    {
        Debug.Log("Generating Room for " + side + " at " + currRoomID + "  roomListSize: " + roomList.Count);
        GameObject currentRoom = roomList[currRoomID];
        Vector3 currPos = currentRoom.transform.position;
        RoomScript currRoomScript = currentRoom.GetComponent<RoomScript>();

        float offsetX = (side == DIRECTION.LEFT ? -scaleX : (side == DIRECTION.RIGHT ? scaleX : 0));
        float offsetY = (side == DIRECTION.DOWN ? -scaleY : (side == DIRECTION.UP ? scaleY : 0));
        GameObject room = Instantiate(defaultRoom, new Vector3(currPos.x + offsetX, currPos.y + offsetY, zOffset), Quaternion.identity);
        room.transform.localScale.Set(scaleX, scaleY, 1);

        RoomScript roomScript = room.GetComponent<RoomScript>();

        DIRECTION forceTrueDir = this.GetOppositeDir(side);
        Dictionary<DIRECTION, bool> boolArray = GetDoorOpenBooleans(forceTrueDir);
        int newGridX = currRoomScript.GetGridX() + (side == DIRECTION.LEFT ? -1 : (side == DIRECTION.RIGHT ? 1 : 0));
        int newGridY = currRoomScript.GetGridY() + (side == DIRECTION.DOWN ? -1 : (side == DIRECTION.UP ? 1 : 0));
        roomScript.Set(currID, newGridX, newGridY,
            boolArray[DIRECTION.LEFT], boolArray[DIRECTION.RIGHT], boolArray[DIRECTION.UP], boolArray[DIRECTION.DOWN], forceTrueDir);

        StoreRoom(currID, newGridX, newGridY, room);
    }

    //Left, right, up, down
    private Dictionary<DIRECTION, bool> GetDoorOpenBooleans(DIRECTION forceTrueDir)
    {
        float incChance = 0.0f;
        Dictionary<DIRECTION, bool> boolArray = new Dictionary<DIRECTION, bool>();
        boolArray[DIRECTION.LEFT] = UnityEngine.Random.value < (0.5f + incChance);
        if (!boolArray[DIRECTION.LEFT])
            incChance += 0.25f;
        else
            incChance -= 0.25f;

        boolArray[DIRECTION.RIGHT] = UnityEngine.Random.value < (0.5f + incChance);
        if (!boolArray[DIRECTION.RIGHT])
            incChance += 0.25f;
        else
            incChance -= 0.25f;
        boolArray[DIRECTION.UP] = UnityEngine.Random.value < (0.5f + incChance);
        if (!boolArray[DIRECTION.UP])
            incChance += 0.25f;
        else
            incChance -= 0.25f;
        boolArray[DIRECTION.DOWN] = UnityEngine.Random.value < (0.5f + incChance);

        boolArray[forceTrueDir] = true;
        return boolArray;
    }

    DIRECTION GetOppositeDir (DIRECTION dir)
    {
        switch (dir)
        {
            case DIRECTION.LEFT:
                return DIRECTION.RIGHT;
            case DIRECTION.RIGHT:
                return DIRECTION.LEFT;
            case DIRECTION.UP:
                return DIRECTION.DOWN;
            case DIRECTION.DOWN:
                return DIRECTION.UP;
        }
        return DIRECTION.NONE;
    }

    void StoreRoom(int ID, int x, int y, GameObject room)
    {
        if (!roomMap.ContainsKey(y))
            roomMap.Add(y, new Dictionary<int, GameObject>());
        try
        {
            roomMap[y].Add(x, room);
            roomList.Add(room);
            ++currID;
            if (y < smallestY)
                smallestY = y;
            else if (y > biggestY)
                biggestY = y;
            if (x < smallestX)
                smallestX = x;
            else if (x > biggestX)
                biggestX = x;

            //Connect room
            ConnectNeighbour(x - 1, y, DIRECTION.LEFT);
            ConnectNeighbour(x + 1, y, DIRECTION.RIGHT);
            ConnectNeighbour(x, y + 1, DIRECTION.UP);
            ConnectNeighbour(x, y - 1, DIRECTION.DOWN);
        }
        catch(ArgumentException e)
        {
            Debug.Log("What? Room alr stored?! Are you sure this is intended?");
        }
    }
    
    void ConnectNeighbour(int x, int y, DIRECTION neighbourAt)
    {
        if (!roomMap.ContainsKey(y))
            return;
        if (!roomMap[y].ContainsKey(x - 1))
            return;

        DIRECTION oppoSide = this.GetOppositeDir(neighbourAt);
        GameObject neighbour = roomMap[y][x];
        RoomScript neighbourScript = neighbour.GetComponent<RoomScript>();
        neighbourScript.OffTriggerBox(oppoSide);
    }

    public Vector3 GetMidPoint(out float width, out float height)
    {
        int gridWidth = biggestX - smallestX;
        int gridHeight = biggestY - smallestY;

        width = (gridWidth + 1) * scaleX;
        height = (gridHeight + 1) * scaleY;

        Vector3 midPt = new Vector3(smallestX * scaleX + gridWidth * 0.5f * scaleX, smallestY * scaleY + gridHeight * 0.5f * scaleY);
        return midPt;
    }

    //No use atm
    int GetFurthest(DIRECTION dir)
    {
        switch (dir)
        {
            case DIRECTION.LEFT: return smallestX;
            case DIRECTION.RIGHT: return biggestX;
            case DIRECTION.UP: return biggestY;
            case DIRECTION.DOWN: return smallestY;
        }
        return 0;
    }
}
