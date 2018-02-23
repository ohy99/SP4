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
    public int roomID;
    public int gridX;
    public int gridY;
    public Vector3 roomPos;
    public Vector3 rooomScale;
}