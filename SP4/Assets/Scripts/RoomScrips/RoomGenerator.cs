using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //To catch argument exception

public enum DIRECTION
{
    NONE = 0,

    LEFT,
    RIGHT,
    UP,
    DOWN
}


public class RoomGenerator : Singleton<RoomGenerator> {

    const bool DEBUG_ROOMGEN = false;

    [SerializeField]
    GameObject defaultRoom;
    [SerializeField]
    List<GameObject> randomRooms;
    [SerializeField]
    GameObject bossRoom;
    //[SerializeField]
    //GameObject room1;
    //[SerializeField]
    //GameObject room2;
    //[SerializeField]
    //GameObject room3;

    [SerializeField]
    bool GenerateOnStart = true;
    [SerializeField]
    UnityEngine.UI.Text debugText;
    [SerializeField]
    UnityEngine.UI.Text chanceText;

    //Array of rooms with ID
    List<GameObject> roomList;
    int currID; //aka size of roomList. so roomList.count == currID
    float zOffset = 1;

    [SerializeField]
    const float scaleX = 20;
    [SerializeField]
    const float scaleY = 20;
    //Positional representation of rooms
    Dictionary<int, Dictionary<int, GameObject>> roomMap;//y , x
    int smallestX = 0; int smallestY = 0;
    int biggestX = 0; int biggestY = 0;

    [SerializeField]
    int estTotalRooms = 10;
    int numOfOpenedDoors;
    bool generatedBossRoom;

    enum RANDACTION
    {
        MUSTLOCK,
        CANCHOOSE,
        MUSTOPEN
    }

	// Use this for initialization
	void Start () {
        
        biggestX = biggestY = smallestY = smallestX = 0;
        numOfOpenedDoors = 0;
        generatedBossRoom = false;
        roomList = new List<GameObject>();
        roomMap = new Dictionary<int, Dictionary<int, GameObject>>();
        currID = 0;
        Global.Instance.bossIsDead = false;

        if (!GenerateOnStart)
            return;
        //Debug.Log("RoomGenerator Start()");
        //spawn at 0,0, so generate one at 0,0
        GameObject room = Instantiate(defaultRoom, new Vector3(0, 0, zOffset), Quaternion.identity);
        room.transform.localScale.Set(scaleX, scaleY, 1);

        RoomScript roomScript = room.GetComponent<RoomScript>();
        Dictionary<DIRECTION, bool> boolArray = new Dictionary<DIRECTION, bool>();
        float incChance = 0.0f;
        for(DIRECTION i = DIRECTION.LEFT; i < DIRECTION.LEFT + 4; ++i)
        {
            boolArray[i] = false;
            if (!boolArray[i])
            {
                boolArray[i] = UnityEngine.Random.value < (0.5f + incChance);
                if (boolArray[i])
                {
                    ++numOfOpenedDoors;
                    incChance -= 0.2f;
                }
                else
                    incChance += 0.25f;
            }
        }
        roomScript.Set(currID, 0, 0, boolArray[DIRECTION.LEFT], boolArray[DIRECTION.RIGHT], boolArray[DIRECTION.UP], boolArray[DIRECTION.DOWN]);
        StoreRoom(currID, 0, 0, room);

        while (numOfOpenedDoors > 0)
        {

            DIRECTION[] dirList = new DIRECTION[4];
            //check which door open
            for (int i = 0; i < roomList.Count; ++i)
            {
                Debug.Log(roomList.Count);
                GameObject daroom = roomList[i];
                RoomScript daroomscript = daroom.GetComponent<RoomScript>();

                for(DIRECTION j = DIRECTION.LEFT; j < DIRECTION.LEFT + 4; ++j)
                {
                    if (daroomscript.GetHasTriggerBox(j) && !daroomscript.GetIsLocked(j))
                    {
                        //open tat door
                        GenerateRoom(i, j);
                    }
                }
            }
        }
    }

    public void ReStart()
    {
        Debug.Log("Restarting RoomGenerator");
        if (roomList.Count > 0)
        {
            foreach (GameObject go in roomList)
                Destroy(go);
            foreach (KeyValuePair<int, Dictionary<int, GameObject>> pair in roomMap)
                pair.Value.Clear();
            roomMap.Clear();
            roomList.Clear();
        }

        Start();
    }
	
	// Update is called once per frame
	void Update () {
        if (debugText != null)
            debugText.text = "openedDoors: " + numOfOpenedDoors.ToString();
        if (chanceText != null)
            chanceText.text = "Chance to spawn when potential = 3: " + 
                ((0.5f * Mathf.Log(1 + estTotalRooms - currID, estTotalRooms) + (1.0f / Mathf.Max(10.0f * (currID / (float)estTotalRooms), numOfOpenedDoors + 3))).ToString());
    }

    public void GenerateRoom(int currRoomID, DIRECTION side)
    {
        if (DEBUG_ROOMGEN)
        Debug.Log("Generating Room for " + side + " at " + currRoomID + "  roomListSize: " + roomList.Count);
        GameObject currentRoom = roomList[currRoomID];
        Vector3 currPos = currentRoom.transform.position;
        RoomScript currRoomScript = currentRoom.GetComponent<RoomScript>();

        DIRECTION forceTrueDir = this.GetOppositeDir(side);
        int newGridX = currRoomScript.GetGridX() + (side == DIRECTION.LEFT ? -1 : (side == DIRECTION.RIGHT ? 1 : 0));
        int newGridY = currRoomScript.GetGridY() + (side == DIRECTION.DOWN ? -1 : (side == DIRECTION.UP ? 1 : 0));
        Dictionary<DIRECTION, RANDACTION> boolArray = new Dictionary<DIRECTION, RANDACTION>();//= GetDoorOpenBooleans(forceTrueDir, true);
        int numOfPotentialOpen = 0;
        boolArray[DIRECTION.NONE] = 0;
        boolArray[DIRECTION.LEFT] = IsNeighbourHaveUnlockedDoor(newGridX, newGridY, DIRECTION.LEFT);
        boolArray[DIRECTION.RIGHT] = IsNeighbourHaveUnlockedDoor(newGridX, newGridY, DIRECTION.RIGHT);
        boolArray[DIRECTION.UP] = IsNeighbourHaveUnlockedDoor(newGridX, newGridY, DIRECTION.UP);
        boolArray[DIRECTION.DOWN] = IsNeighbourHaveUnlockedDoor(newGridX, newGridY, DIRECTION.DOWN);

        List<DIRECTION> dirToOffTrigger = new List<DIRECTION>();
        List<DIRECTION> openDoors = new List<DIRECTION>();
        //pre calculate the chances to spawn doors
        foreach (KeyValuePair<DIRECTION, RANDACTION> pair in boolArray)
        {
            if (pair.Key == DIRECTION.NONE)
                continue;
            if (pair.Value == RANDACTION.MUSTOPEN)
            {
                //This side of the new room MUST open
                --numOfOpenedDoors;
                //connect the doors
                dirToOffTrigger.Add(pair.Key);
                openDoors.Add(pair.Key);
            }
            else if (pair.Value == RANDACTION.CANCHOOSE)
            {
                //THIS Side of the new room will undergo calculation
                ++numOfPotentialOpen;
            }
            else
            {
                if (DEBUG_ROOMGEN)
                    //lock this door
                    Debug.Log("MustLock activated");
            }
        }

        //calculate the opened door chances based on current situation
        foreach (KeyValuePair<DIRECTION, RANDACTION> pair in boolArray)
        {
            if (pair.Value == RANDACTION.CANCHOOSE)
            {
                bool openDaDoor = UnityEngine.Random.value < (0.5f * Mathf.Log(1 + estTotalRooms - currID, estTotalRooms) 
                    + (1.0f / Mathf.Max(10.0f * (currID / (float)estTotalRooms), numOfOpenedDoors + numOfPotentialOpen)));
                if (openDaDoor)
                {
                    openDoors.Add(pair.Key);
                    ++numOfOpenedDoors;
                }
            }
        }

        GameObject room;
        float offsetX = (side == DIRECTION.LEFT ? -scaleX : (side == DIRECTION.RIGHT ? scaleX : 0));
        float offsetY = (side == DIRECTION.DOWN ? -scaleY : (side == DIRECTION.UP ? scaleY : 0));
        if (!generatedBossRoom)
        {
            //attempt to generate bossroom
            if (numOfOpenedDoors == 0 || 0.25f * ((currID + 1) / estTotalRooms) > UnityEngine.Random.value)
            {
                room = Instantiate(bossRoom, new Vector3(currPos.x + offsetX, currPos.y + offsetY, zOffset), Quaternion.identity);
                generatedBossRoom = true;
            }
            else
            {
                int randIndex = UnityEngine.Random.Range(0, randomRooms.Count - 1);
                room = Instantiate(randomRooms[randIndex], new Vector3(currPos.x + offsetX, currPos.y + offsetY, zOffset), Quaternion.identity);
            }
        }
        else
            room = Instantiate(defaultRoom, new Vector3(currPos.x + offsetX, currPos.y + offsetY, zOffset), Quaternion.identity);
        room.transform.localScale.Set(scaleX, scaleY, 1);
        RoomScript roomScript = room.GetComponent<RoomScript>();
        roomScript.Set(currID, newGridX, newGridY,
            openDoors.Contains(DIRECTION.LEFT), openDoors.Contains(DIRECTION.RIGHT), 
            openDoors.Contains(DIRECTION.UP), openDoors.Contains(DIRECTION.DOWN));
        //foreach (DIRECTION dir in dirToOffTrigger)
        //{
        //    roomScript.OffTriggerBox(dir);
        //    RoomScript neighbourRS = GetNeighbourRoomScript(newGridX, newGridY, dir);
        //    DIRECTION oppoSide = GetOppositeDir(dir);
        //    neighbourRS.OffTriggerBox(oppoSide);
        //}
        
        StoreRoom(currID, newGridX, newGridY, room);
        room.SetActive(false);
    }

    ////Left, right, up, down. ADDTO : add to the int numOfOpeneddoor
    ////die die have one door spawn. this is only used for the very first room
    //private Dictionary<DIRECTION, bool> GetDoorOpenBooleans(DIRECTION forceTrueDir, bool addTo)
    //{
    //    //this default random door open means 100% got 1 door would open
    //    float incChance = 0.0f;
    //    float chanceDecrease = 1.0f - (float)Math.Log(estTotalRooms - currID, estTotalRooms);

    //    Dictionary<DIRECTION, bool> boolArray = new Dictionary<DIRECTION, bool>();
    //    boolArray[DIRECTION.NONE] = false;
    //    if (forceTrueDir != DIRECTION.NONE)
    //        boolArray[forceTrueDir] = true;

    //    foreach (KeyValuePair<DIRECTION, bool> pair in boolArray)
    //    {
    //        if (pair.Value && pair.Key == DIRECTION.NONE)
    //            continue;
    //        boolArray[pair.Key] = UnityEngine.Random.value < (0.5f + incChance - chanceDecrease);
    //        if (!boolArray[pair.Key])
    //            incChance += 0.25f;
    //        else
    //            incChance -= 0.25f;
    //    }

    //    if (addTo)
    //    {
    //        foreach (KeyValuePair<DIRECTION, bool> pair in boolArray)
    //        {
    //            if (pair.Value)
    //            {
    //                ++numOfOpenedDoors;
    //            }
    //        }
    //    }

    //    return boolArray;
    //}

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

            //RoomScript rs = room.GetComponent<RoomScript>();
            ////Connect room, off the triggerbox
            //ConnectNeighbour(x - 1, y, DIRECTION.LEFT, rs);
            //ConnectNeighbour(x + 1, y, DIRECTION.RIGHT, rs);
            //ConnectNeighbour(x, y + 1, DIRECTION.UP, rs );
            //ConnectNeighbour(x, y - 1, DIRECTION.DOWN, rs);
        }
        catch(ArgumentException e)
        {
            if (DEBUG_ROOMGEN)
                Debug.Log("What? Room alr stored?! Are you sure this is intended?");
        }
    }

    //void ConnectNeighbour(int x, int y, DIRECTION neighbourAt, RoomScript myRoom)
    //{
    //    if (!roomMap.ContainsKey(y))
    //        return;
    //    if (!roomMap[y].ContainsKey(x))
    //        return;

    //    DIRECTION oppoSide = this.GetOppositeDir(neighbourAt);
    //    GameObject neighbour = roomMap[y][x];
    //    RoomScript neighbourScript = neighbour.GetComponent<RoomScript>();
    //    if (!neighbourScript.GetIsLocked(oppoSide))
    //    {
    //        neighbourScript.OffTriggerBox(oppoSide);
    //        myRoom.UnlockDoor(neighbourAt);
    //        myRoom.OffTriggerBox(neighbourAt);
    //        numOfOpenedDoors -= 2;
    //    }
    //    else
    //    {
    //        //if neighbour facing me is locked, so i also lock lo IF im not locked
    //        if (!myRoom.GetIsLocked(neighbourAt))
    //        {
    //            myRoom.LockDoor(neighbourAt);
    //            myRoom.OnTriggerBox(neighbourAt);
    //            numOfOpenedDoors -= 1;
    //        }

    //    }
    //}

    //returns -1 means must lock, 0 means u can choose, 1 means mmust open
    RANDACTION IsNeighbourHaveUnlockedDoor(int m_x, int m_y, DIRECTION dir)
    {
        int checkX = m_x;
        int checkY = m_y;
        switch (dir)
        {
            case DIRECTION.LEFT: checkX -= 1; break;
            case DIRECTION.RIGHT: checkX += 1; break;
            case DIRECTION.UP: ++checkY; break;
            case DIRECTION.DOWN: --checkY; break;
        }
        if (!roomMap.ContainsKey(checkY))
            return RANDACTION.CANCHOOSE;
        if (!roomMap[checkY].ContainsKey(checkX))
            return RANDACTION.CANCHOOSE;

        RoomScript rs = roomMap[checkY][checkX].GetComponent<RoomScript>();
        if (rs.GetIsLocked(GetOppositeDir(dir)))
            return RANDACTION.MUSTLOCK;
        else
            return RANDACTION.MUSTOPEN;
        return RANDACTION.CANCHOOSE;
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

    RoomScript GetNeighbourRoomScript(int m_x, int m_y, DIRECTION dir)
    {
        int checkX = m_x;
        int checkY = m_y;
        switch (dir)
        {
            case DIRECTION.LEFT: checkX -= 1; break;
            case DIRECTION.RIGHT: checkX += 1; break;
            case DIRECTION.UP: ++checkY; break;
            case DIRECTION.DOWN: --checkY; break;
        }
        if (!roomMap.ContainsKey(checkY))
            return null;
        if (!roomMap[checkY].ContainsKey(checkX))
            return null;
        RoomScript rs = roomMap[checkY][checkX].GetComponent<RoomScript>();
        return rs;
    }

    public void SetRoomActive(int currRoomID, DIRECTION side)
    {
        GameObject currentRoom = roomList[currRoomID];
        Vector3 currPos = currentRoom.transform.position;
        RoomScript currRoomScript = currentRoom.GetComponent<RoomScript>();

        int newGridX = currRoomScript.GetGridX() + (side == DIRECTION.LEFT ? -1 : (side == DIRECTION.RIGHT ? 1 : 0));
        int newGridY = currRoomScript.GetGridY() + (side == DIRECTION.DOWN ? -1 : (side == DIRECTION.UP ? 1 : 0));

        Debug.Log("Ok");
        roomMap[newGridY][newGridX].SetActive(true);
    }
}
