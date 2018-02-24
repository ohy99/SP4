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

