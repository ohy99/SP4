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

        myClient.RegisterHandler(MyMsgType.spawnRoomMsgType_server, OnRecvSpawnRoom_Client);
        NetworkServer.RegisterHandler(MyMsgType.spawnRoomMsgType_client, OnRecvSpawnRoom_Server);
    }

    //SENDING TO CLIENTS
    public void SendPosition_S2C(Vector3 _position)
    {
        PositionMessage msg = new PositionMessage();
        msg.position = _position;

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.positionMsgType_server, msg);
    }

    public void SendSpawnRoom_S2C(int _roomIndex,DIRECTION _side)
    {
        SpawnRoomMessage msg = new SpawnRoomMessage();
        msg.roomIndex = _roomIndex;
        msg.direction = _side;

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.spawnRoomMsgType_server, msg);
    }


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

    public void SendSpawnRoom_C2S()
    {
        SpawnRoomMessage msg = new SpawnRoomMessage();

        //send to server
        myClient.Send(MyMsgType.spawnRoomMsgType_client, msg);
    }

    // ---Recieving the message--------------------------------------------------
  
    //POSITION
    public void OnRecvPosition_Server(NetworkMessage netMsg)
    {
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

    //SPAWN ROOM
    public void OnRecvSpawnRoom_Server(NetworkMessage netMsg)
    {
        SpawnRoomMessage msg = netMsg.ReadMessage<SpawnRoomMessage>();

        //if(Global.Instance.player.GetComponent<Player>().isServer)
        Debug.Log("Host/ServerRecv_SpawnRoom");

        //SendSpawnRoom_S2C(null);
    }

    public void OnRecvSpawnRoom_Client(NetworkMessage netMsg)
    {
        SpawnRoomMessage msg = netMsg.ReadMessage<SpawnRoomMessage>();

        Debug.Log("ClientRecv_SpawnRoom - " + RoomGenerator.Instance.GetRoomList().Count);
        Debug.Log("ClientRecv_SpawnRoom - RD: " + msg.roomIndex + " Dir:" + msg.direction);
        RoomGenerator.Instance.SetRoomActive(msg.roomIndex, msg.direction);
    }
}


/*
 * Things to do
 * 1. struct which contains id for all the msg
 * 2. class inhert from msgbase so as to send
 * 3. func to send C2S & S2C
 * 4. Register all dem msg
 * 5. the handler on what to do when it recieves the msg
 */
