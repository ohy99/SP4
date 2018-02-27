using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PositionMessage : MessageBase
{
    public Vector3 position;
    //public GameObject test;
}

public class HealthMessage : MessageBase
{
    public float health;
}

public class SpawnRoomMessage : MessageBase
{
    public int roomIndex;
    public DIRECTION direction;
}

public class PopulateRoomListMessage : MessageBase
{
    public int connectionId;
    public int roomID;
    public int gridX;
    public int gridY;
    public int roomType;
    public Vector3 roomPos;
    public Vector3 roomScale;
    public bool isLeft;
    public bool isRight;
    public bool isUp;
    public bool isDown;

    //public GameObject room;
}

public class UnlockDoorMessage : MessageBase
{
    public int roomId;
    public bool isClear;
}

public class SpotPuzzleRoomMessage : MessageBase
{   //yay
    public int roomId;
    public int changedObjectIndex;
    public Vector3 shape01_pos;
    public Vector3 shape02_pos;
    public Vector3 shape03_pos;
    public Vector3 shape04_pos;
    public Color changedObjectColor; 
}

public class PushPuzzleRoomMessage : MessageBase
{
    public int connectionId;
    public int roomId;
    public Vector3 objectivePos;
    public Vector3 targetPos;
}

public class LockDoorMessage : MessageBase
{
    public int roomId;
}

public class NumberToSpawnMessage : MessageBase
{
    public int roomId;
    public int spawnNumber;
    public string roomType;
}

public class PlayerIdMessage : MessageBase
{
    public int playerId;
}

public class itemCollectedMessage : MessageBase
{
    public int roomId;
    public int itemCollected;
}

public class activeRoomMessage : MessageBase
{
    public int roomId;
    public string strRoomIds;
}