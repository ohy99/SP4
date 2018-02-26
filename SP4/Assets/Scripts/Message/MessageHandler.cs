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

        public static short pushPuzzleMsgType_client = MsgType.Highest + 11;
        public static short pushPuzzleMsgType_server = MsgType.Highest + 12;

        public static short lockDoorMsg_client = MsgType.Highest + 13;
        public static short lockDoorMsg_server = MsgType.Highest + 14;

        public static short NumberToSpawnMsg_client = MsgType.Highest + 15;
        public static short NumberToSpawnMsg_server = MsgType.Highest + 16;
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

        myClient.RegisterHandler(MyMsgType.unlockDoorMsgType_server, OnRecvUnlockDoor_Client);
        NetworkServer.RegisterHandler(MyMsgType.unlockDoorMsgType_client, OnRecvUnlockDoor_Server);

        myClient.RegisterHandler(MyMsgType.pushPuzzleMsgType_server, OnRecvPushPuzzle_Client);
        NetworkServer.RegisterHandler(MyMsgType.pushPuzzleMsgType_client, OnRecvPushPuzzle_Server);

        myClient.RegisterHandler(MyMsgType.lockDoorMsg_server, OnRecvLockDoor_Client);
        NetworkServer.RegisterHandler(MyMsgType.lockDoorMsg_client, OnRecvLockDoor_Server);

        myClient.RegisterHandler(MyMsgType.NumberToSpawnMsg_server, OnRecvNumberToSpawn_Client);
        NetworkServer.RegisterHandler(MyMsgType.NumberToSpawnMsg_client, OnRecvNumberToSpawn_Server);

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

    public void SendUnlockDoor_S2C(int _roomId, bool _isClear)
    {
        UnlockDoorMessage msg = new UnlockDoorMessage();
        msg.roomId = _roomId;
        msg.isClear = _isClear;

        Debug.Log("SendUnlockDoor_S2C");
        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.unlockDoorMsgType_server, msg);
    }

    public void SendPushPuzzle_S2C(int _connectionId, int _roomId, Vector3 _objectivePos, Vector3 _targetPos)
    {
        PushPuzzleRoomMessage msg = new PushPuzzleRoomMessage();
        msg.connectionId = _connectionId;
        msg.roomId = _roomId;
        msg.objectivePos = _objectivePos;
        msg.targetPos = _targetPos;

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.pushPuzzleMsgType_server, msg);
        //NetworkServer.SendToClient(_connectionId, MyMsgType.pushPuzzleMsgType_server, msg);
    }

    public void SendLockDoor_S2C(int _roomId)
    {
        LockDoorMessage msg = new LockDoorMessage();
        msg.roomId = _roomId;

        Debug.Log("SendLockDoor_S2C");
        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.lockDoorMsg_server, msg);
    }

    public void SendNumberToSpawn_S2C(int _roomId, int _spawnNumber, string _roomType)
    {
        NumberToSpawnMessage msg = new NumberToSpawnMessage();
        msg.roomId = _roomId;
        msg.spawnNumber = _spawnNumber;
        msg.roomType = _roomType;

        Debug.Log("SendNumberToSpawnDoor_S2C");

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.NumberToSpawnMsg_server, msg);
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

    public void SendRoom_C2S()
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        Debug.Log("SendRoom_C2S_" + myClient.connection.connectionId);
        PopulateRoomListMessage msg = new PopulateRoomListMessage();
        msg.connectionId = myClient.connection.connectionId;

        //send to server
        myClient.Send(MyMsgType.roomListMsgType_client, msg);
    }

    public void SendUnlockDoor_C2S(int _roomId, bool _isClear)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        Debug.Log("SendUnlockDoor_C2S_" + myClient.connection.connectionId);
        UnlockDoorMessage msg = new UnlockDoorMessage();
        msg.roomId = _roomId;
        msg.isClear = _isClear;

        //send to server
        myClient.Send(MyMsgType.unlockDoorMsgType_client, msg);
    }

    public void SendPushPuzzle_C2S(int _roomId)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        Debug.Log("SendPushPuzzle_C2S_" + myClient.connection.connectionId);
        PushPuzzleRoomMessage msg = new PushPuzzleRoomMessage();
        msg.roomId = _roomId;
        msg.connectionId = myClient.connection.connectionId;

        //send to server
        myClient.Send(MyMsgType.pushPuzzleMsgType_client, msg);
    }

    public void SendLockDoor_C2S(int _roomId)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        Debug.Log("SendLockDoor_C2S_" + myClient.connection.connectionId);
        LockDoorMessage msg = new LockDoorMessage();
        msg.roomId = _roomId;

        //send to server
        myClient.Send(MyMsgType.lockDoorMsg_client, msg);
    }

    public void SendNumberToSpawn_C2S(int _roomId, int _spawnNumber, string _roomType)
    {
        NumberToSpawnMessage msg = new NumberToSpawnMessage();
        msg.roomId = _roomId;
        msg.spawnNumber = _spawnNumber;
        msg.roomType = _roomType;

        Debug.Log("SendNumberToSpawnDoor_C2S");

        myClient.Send(MyMsgType.NumberToSpawnMsg_client, msg);
    }
    //-----------------------------------------------------------------------------


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
        if (msg.roomID != 0)
            room.SetActive(false);


        //Debug.Log("==============================");
        //Debug.Log("ClientRecv_Room_" + msg.connectionId);
        //Debug.Log("ClientRecv_Room_RoomId" + msg.roomID);
        //Debug.Log("ClientRecv_Room_gridX" + msg.gridX);
        //Debug.Log("ClientRecv_Room_gridY" + msg.gridY);
        //Debug.Log("ClientRecv_Room_Pos" + msg.roomPos);
        //Debug.Log("ClientRecv_Room_roomType" + msg.roomType);
        //Debug.Log("ClientRecv_Room_LEFT" + msg.isLeft);
        //Debug.Log("ClientRecv_Room_RIGHT" + msg.isRight);
        //Debug.Log("ClientRecv_Room_UP" + msg.isUp);
        //Debug.Log("ClientRecv_Room_DOWN" + msg.isDown);
        //Debug.Log("==============================");
    }

    // Unlock door/ room is cleared
    public void OnRecvUnlockDoor_Server(NetworkMessage netMsg)
    {
        UnlockDoorMessage msg = netMsg.ReadMessage<UnlockDoorMessage>();

        Debug.Log("Host/ServerRecv_UnlockDoor : room_" + msg.roomId + " -> " + msg.isClear);
        RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<RoomScript>().SetPuzzleComplete(msg.isClear);
    }

    public void OnRecvUnlockDoor_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        UnlockDoorMessage msg = netMsg.ReadMessage<UnlockDoorMessage>();

        Debug.Log("ClientRecv_UnlockDoor : room_" + msg.roomId + " -> " + msg.isClear);
        RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<RoomScript>().SetPuzzleComplete(msg.isClear);
    }

    //recv push puzzle
    public void OnRecvPushPuzzle_Server(NetworkMessage netMsg)
    {
        PushPuzzleRoomMessage msg = netMsg.ReadMessage<PushPuzzleRoomMessage>();

        Debug.Log("Host/ServerRecv_PushPuzzle_" + msg.connectionId + ": room_" + msg.roomId);

        msg.objectivePos = RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().GetObjectivePos();
        msg.targetPos = RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().GetTargetPos();

        SendPushPuzzle_S2C(msg.connectionId, msg.roomId, msg.objectivePos, msg.targetPos);
    }

    public void OnRecvPushPuzzle_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        PushPuzzleRoomMessage msg = netMsg.ReadMessage<PushPuzzleRoomMessage>();
        Debug.Log("ClientRecv_PushPuzzle_" + msg.connectionId + ": room_" + msg.roomId);


        RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().SetObjectivePos(msg.objectivePos);
        RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().SetTargetPos(msg.targetPos);
    }

    //recv lock door
    public void OnRecvLockDoor_Server(NetworkMessage netMsg)
    {
        LockDoorMessage msg = netMsg.ReadMessage<LockDoorMessage>();

        Debug.Log("Host/ServerRecv_LockDoor : room_" + msg.roomId);
        RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<RoomScript>().LockAllDoor();
    }

    public void OnRecvLockDoor_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        LockDoorMessage msg = netMsg.ReadMessage<LockDoorMessage>();

        Debug.Log("ClientRecv_LockDoor : room_" + msg.roomId);
        RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<RoomScript>().LockAllDoor();
    }

    //recv spawn num
    public void OnRecvNumberToSpawn_Server(NetworkMessage netMsg)
    {
        NumberToSpawnMessage msg = netMsg.ReadMessage<NumberToSpawnMessage>();

        Debug.Log("Host/ServerRecv_NumberToSpawn : room_" + msg.roomId);
        //RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<RoomScript>().LockAllDoor();
    }

    public void OnRecvNumberToSpawn_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        NumberToSpawnMessage msg = netMsg.ReadMessage<NumberToSpawnMessage>();

        Debug.Log("ClientRecv_NumberToSpawn : room_" + msg.roomId);

        if(msg.roomType == "enemyRoom")
        {
            Debug.Log("RoomId: " + msg.roomId + " totalWave: " + msg.spawnNumber);
            if (RoomGenerator.Instance.GetRoomList()[msg.roomId] == null)
                Debug.Log("Room not started yet");

            RoomGenerator.Instance.GetRoomList()[msg.roomId].
                GetComponent<EnemyRoomScript>().GetSpawner().
                GetComponent<EnemySpawner>().SetTotalWave(msg.spawnNumber);

            //RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<EnemyRoomScript>().totalNumWave = msg.spawnNumber;
        }
        else if(msg.roomType == "speedRoom")
        {
            RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<SpeedRoomScript>().GetSpawnerScript().maxSpawns = msg.spawnNumber;
        }
    }
}


/*
 * Things to do
 * 2. msg class inhert from msgbase so as to send
 * 3. func to send C2S & S2C
 * 4. Register all dem msg
 * 5. the handler on what to do when it recieves the msg
 * 
 * Moar stuff to do
 * sync health of players, enemies
 * sync speed room number of item to spawn and whne item picked up
 * sync push room ball spawn location and the movement
 * sync spot room position of objects and da colors
 */
