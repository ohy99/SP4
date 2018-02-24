using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class MessageHandler : Singleton<MessageHandler>
{
    //Types of msg key/index
    public struct MyMsgType
    {
        public static short positionMsgType_client = MsgType.Highest + 1;
        public static short positionMsgType_server = MsgType.Highest + 2;

        public static short healthMsgType_client = MsgType.Highest + 3;
        public static short healthMsgType_server = MsgType.Highest + 4;

        public static short spawnRoomMsgType_client = MsgType.Highest + 5;
        public static short spawnRoomMsgType_server = MsgType.Highest + 6;

        public static short roomListMsgType_client = MsgType.Highest + 7;
        public static short roomListMsgType_server = MsgType.Highest + 8;

        public static short unlockDoorMsgType_client = MsgType.Highest + 9;
        public static short unlockDoorMsgType_server = MsgType.Highest + 10;
    };

    //variables
    //public List<NetworkClient> myClient = new List<NetworkClient>();
    public NetworkClient myClient;
    public int index;

    public void Awake()
    {
        //myClient = new List<NetworkClient>();
    }


    // RegisterHandler for clients and server
    public void Register(NetworkClient client)
    {
        myClient = client;

        myClient.RegisterHandler(MyMsgType.positionMsgType_server, OnRecvPosition_Client);
        NetworkServer.RegisterHandler(MyMsgType.positionMsgType_client, OnRecvPosition_Server);

        myClient.RegisterHandler(MyMsgType.roomListMsgType_server, OnRecvRoom_Client);
        NetworkServer.RegisterHandler(MyMsgType.roomListMsgType_client, OnRecvRoom_Server);

        //myClient.RegisterHandler(MyMsgType.spawnRoomMsgType_server, OnRecvSpawnRoom_Client);
        //NetworkServer.RegisterHandler(MyMsgType.spawnRoomMsgType_client, OnRecvSpawnRoom_Server);
    }

    //SENDING TO CLIENTS
    public void SendPosition_S2C(Vector3 _position)
    {
        PositionMessage msg = new PositionMessage();
        msg.position = _position;

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.positionMsgType_server, msg);
    }

    public void SendRoom_S2C(PopulateRoomListMessage _container, int _connectionId)
    {
        PopulateRoomListMessage msg = _container;
        msg.connectionId = _connectionId;

        //Debug.Log(_room.GetComponent<RoomScript>().GetRoomID());

        if (NetworkServer.active)
            NetworkServer.SendToClient(_connectionId, MyMsgType.roomListMsgType_server, msg);
            //NetworkServer.SendToAll(MyMsgType.spawnRoomMsgType_server, msg);
    }
    //public void SendSpawnRoom_S2C(int _roomIndex,DIRECTION _side)
    //{
    //    SpawnRoomMessage msg = new SpawnRoomMessage();
    //    msg.roomIndex = _roomIndex;
    //    msg.direction = _side;

    //    if (NetworkServer.active)
    //        NetworkServer.SendToAll(MyMsgType.spawnRoomMsgType_server, msg);
    //}

    //SENDING TO SERVER
    public void SendPosition_C2S(Vector3 _position)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        PositionMessage msg = new PositionMessage();
        msg.position = _position;

        //send to server
        myClient.Send(MyMsgType.positionMsgType_client, msg);
    }

    public void SendRoom_C2S()
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        Debug.Log("SendRoom_C2S");
        PopulateRoomListMessage msg = new PopulateRoomListMessage();
        msg.connectionId = myClient.connection.connectionId;

        //send to server
        myClient.Send(MyMsgType.roomListMsgType_client, msg);
    }
    //public void SendSpawnRoom_C2S()
    //{
    //    SpawnRoomMessage msg = new SpawnRoomMessage();

    //    //send to server
    //    myClient.Send(MyMsgType.spawnRoomMsgType_client, msg);
    //}

   
    // ---Recieving the message--------------------------------------------------

    //POSITION
    public void OnRecvPosition_Server(NetworkMessage netMsg)
    {
        //if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
        //    return;

        PositionMessage msg = netMsg.ReadMessage<PositionMessage>();

        //if(Global.Instance.player.GetComponent<Player>().isServer)
        Debug.Log("Host/ServerRecv_Position_" + index + " : " + msg.position);
    }
    public void OnRecvPosition_Client(NetworkMessage netMsg)
    {
        PositionMessage msg = netMsg.ReadMessage<PositionMessage>();

        //if (!Global.Instance.player.GetComponent<Player>().isServer)
            Debug.Log("ClientRecv_Position_" + index + " : " + msg.position);
    }

    //pop room list
    public void OnRecvRoom_Server(NetworkMessage netMsg)
    {
        PopulateRoomListMessage msg = netMsg.ReadMessage<PopulateRoomListMessage>();

        //if(Global.Instance.player.GetComponent<Player>().isServer)
        Debug.Log("Host/ServerRecv_Room_");
        for (int i = 0; i < RoomGenerator.Instance.roomDataList.Count; ++i)
            SendRoom_S2C(RoomGenerator.Instance.roomDataList[i], msg.connectionId);
    }

    public void OnRecvRoom_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        PopulateRoomListMessage msg = netMsg.ReadMessage<PopulateRoomListMessage>();

        //make the damn room here
        //roomgen func gen da room,  
        GameObject roomType;
        if (msg.roomType == -1)
            roomType = RoomGenerator.Instance.GetDefaultRoom();
        else
            roomType = RoomGenerator.Instance.GetRandomRoomList()[msg.roomType];


        GameObject room = Instantiate(roomType, msg.roomPos, Quaternion.identity);
        room.transform.localScale = msg.roomScale;
        room.GetComponent<RoomScript>().Set(msg.roomID, msg.gridX, msg.gridY,
            !msg.isLeft, !msg.isRight, !msg.isUp, !msg.isDown);
        RoomGenerator.Instance.StoreRoom(msg.roomID + 1, msg.gridX, msg.gridY, room);
        if(msg.roomID != 0)
            room.SetActive(false);


        Debug.Log("===================================");
        Debug.Log("ClientRecv_Room_" + msg.connectionId);
        Debug.Log("ClientRecv_Room_RoomId" + msg.roomID);
        Debug.Log("ClientRecv_Room_gridX" + msg.gridX);
        Debug.Log("ClientRecv_Room_gridY" + msg.gridY);
        Debug.Log("ClientRecv_Room_Pos" + msg.roomPos);
        Debug.Log("ClientRecv_Room_roomType" + msg.roomType);
        Debug.Log("ClientRecv_Room_LEFT" + msg.isLeft);
        Debug.Log("ClientRecv_Room_RIGHT" + msg.isRight);
        Debug.Log("ClientRecv_Room_UP" + msg.isUp);
        Debug.Log("ClientRecv_Room_DOWN" + msg.isDown);
        Debug.Log("===================================");
    }

    //SPAWN ROOM
    //public void OnRecvSpawnRoom_Server(NetworkMessage netMsg)
    //{
    //    SpawnRoomMessage msg = netMsg.ReadMessage<SpawnRoomMessage>();

    //    //if(Global.Instance.player.GetComponent<Player>().isServer)
    //    Debug.Log("Host/ServerRecv_SpawnRoom");

    //    //SendSpawnRoom_S2C(null);
    //}

    //public void OnRecvSpawnRoom_Client(NetworkMessage netMsg)
    //{
    //    SpawnRoomMessage msg = netMsg.ReadMessage<SpawnRoomMessage>();

    //    Debug.Log("ClientRecv_SpawnRoom - " + RoomGenerator.Instance.GetRoomList().Count);
    //    Debug.Log("ClientRecv_SpawnRoom - RD: " + msg.roomIndex + " Dir:" + msg.direction);
    //    RoomGenerator.Instance.SetRoomActive(msg.roomIndex, msg.direction);
    //}
}


/*
 * Things to do
 * 1. struct which contains id for all the msg
 * 2. class inhert from msgbase so as to send
 * 3. func to send C2S & S2C
 * 4. Register all dem msg
 * 5. the handler on what to do when it recieves the msg
 */
