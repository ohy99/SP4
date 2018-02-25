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
{
    public int roomId;

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