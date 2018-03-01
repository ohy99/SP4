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

        public static short lockDoorMsgType_client = MsgType.Highest + 13;
        public static short lockDoorMsgType_server = MsgType.Highest + 14;

        public static short NumberToSpawnMsgType_client = MsgType.Highest + 15;
        public static short NumberToSpawnMsgType_server = MsgType.Highest + 16;

        public static short spotPuzzleMsgType_client = MsgType.Highest + 17;
        public static short spotPuzzleMsgType_server = MsgType.Highest + 18;

        public static short playerIdMsgType_client = MsgType.Highest + 19;
        public static short playerIdMsgType_server = MsgType.Highest + 20;

        public static short itemCollectedMsgType_client = MsgType.Highest + 21;
        public static short itemCollectedMsgType_server = MsgType.Highest + 22;

        public static short activeRoomMsgType_client = MsgType.Highest + 23;
        public static short activeRoomMsgType_server = MsgType.Highest + 24;

        public static short itemChangedMsgType_client = MsgType.Highest + 25;
        public static short itemChangedMsgType_server = MsgType.Highest + 26;

        public static short itemPossessedMsgType_client = MsgType.Highest + 27;
        public static short itemPossessedMsgType_server = MsgType.Highest + 28;
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

        myClient.RegisterHandler(MyMsgType.lockDoorMsgType_server, OnRecvLockDoor_Client);
        NetworkServer.RegisterHandler(MyMsgType.lockDoorMsgType_client, OnRecvLockDoor_Server);

        myClient.RegisterHandler(MyMsgType.NumberToSpawnMsgType_server, OnRecvNumberToSpawn_Client);
        NetworkServer.RegisterHandler(MyMsgType.NumberToSpawnMsgType_client, OnRecvNumberToSpawn_Server);

        myClient.RegisterHandler(MyMsgType.spotPuzzleMsgType_server, OnRecvSpotPuzzle_Client);
        NetworkServer.RegisterHandler(MyMsgType.spotPuzzleMsgType_client, OnRecvSpotPuzzle_Server);

        myClient.RegisterHandler(MyMsgType.playerIdMsgType_server, OnRecvPlayerId_Client);
        NetworkServer.RegisterHandler(MyMsgType.playerIdMsgType_client, OnRecvPlayerId_Server);

        myClient.RegisterHandler(MyMsgType.itemCollectedMsgType_server, OnRecvItemCollected_Client);
        NetworkServer.RegisterHandler(MyMsgType.itemCollectedMsgType_client, OnRecvItemCollected_Server);

        myClient.RegisterHandler(MyMsgType.activeRoomMsgType_server, OnRecvActiveRoom_Client);
        NetworkServer.RegisterHandler(MyMsgType.activeRoomMsgType_client, OnRecvActiveRoom_Server);

        myClient.RegisterHandler(MyMsgType.itemChangedMsgType_server, OnRecvItemChanged_Client);
        NetworkServer.RegisterHandler(MyMsgType.itemChangedMsgType_client, OnRecvItemChanged_Server);

        //myClient.RegisterHandler(MyMsgType.spawnRoomMsgType_server, OnRecvSpawnRoom_Client);
        //NetworkServer.RegisterHandler(MyMsgType.spawnRoomMsgType_client, OnRecvSpawnRoom_Server);
    }

    // SENDING TO CLIENTS
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

        Debug.Log("SendRoom_S2C");

        if (NetworkServer.active)
            NetworkServer.SendToClient(msg.connectionId, MyMsgType.roomListMsgType_server, msg);
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
            NetworkServer.SendToAll(MyMsgType.lockDoorMsgType_server, msg);
    }

    public void SendNumberToSpawn_S2C(int _roomId, int _spawnNumber, string _roomType)
    {
        NumberToSpawnMessage msg = new NumberToSpawnMessage();
        msg.roomId = _roomId;
        msg.spawnNumber = _spawnNumber;
        msg.roomType = _roomType;

        Debug.Log("SendNumberToSpawn_S2C");

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.NumberToSpawnMsgType_server, msg);
    }

    public void SendSpotPuzzle_S2C(SpotPuzzleRoomMessage _message)
    {
        SpotPuzzleRoomMessage msg = _message;
        Debug.Log("SendSpotPuzzle_S2C");

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.spotPuzzleMsgType_server, msg);
    }

    public void SendPlayerId_S2C()
    {
        PlayerIdMessage msg = new PlayerIdMessage();
        msg.playerId = NetworkServer.connections.Count - 1;
        Debug.Log("SendPlayerId_S2C: playerid = " + msg.playerId);

        if (NetworkServer.active) //playerId msg shld only be send once
            NetworkServer.SendToClient(msg.playerId, MyMsgType.playerIdMsgType_server, msg);
        //NetworkServer.SendToAll(MyMsgType.spotPuzzleMsgType_server, msg);
    }

    public void SendItemCollected_S2C(int _roomId, int _itemCollected)
    {
        itemCollectedMessage msg = new itemCollectedMessage();
        msg.roomId = _roomId;
        msg.itemCollected = _itemCollected;
        Debug.Log("SendItemCollected_S2C");

        if (NetworkServer.active)
            NetworkServer.SendToAll(MyMsgType.itemCollectedMsgType_server, msg);
    }

    public void SendActiveRoom_S2C(int _connectionId, int _roomId, bool _isRoomCompleted)
    {
        ActiveRoomMessage msg = new ActiveRoomMessage();
        msg.connectionId = _connectionId;
        msg.roomId = _roomId;
        msg.isRoomCompleted = _isRoomCompleted;
        Debug.Log("SendActiveRoom_S2C");

        if(NetworkServer.active)
            NetworkServer.SendToClient(msg.connectionId, MyMsgType.playerIdMsgType_server, msg);
    }

    public void SendItemChanged_S2C(int _playerId, string name)
    {
        ChangeItemMessage msg = new ChangeItemMessage();
        msg.itemName = name;
        msg.playerId = _playerId;
        Debug.Log("SendItemChanged_S2C");

        myClient.Send(MyMsgType.itemChangedMsgType_server, msg);
    }

    public void SendItemPossessed_S2C(int _playerId, string name)
    {
        ChangeItemMessage msg = new ChangeItemMessage();
        msg.itemName = name;
        msg.playerId = _playerId;
        Debug.Log("SendItemPossessed_S2C");

        myClient.Send(MyMsgType.itemPossessedMsgType_server, msg);
    }

    // ============================SENDING TO SERVER======================================
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

        Debug.Log("SendRoom_C2S_" + index);
        PopulateRoomListMessage msg = new PopulateRoomListMessage();
        msg.connectionId = index;

        //send to server
        myClient.Send(MyMsgType.roomListMsgType_client, msg);
    }

    public void SendUnlockDoor_C2S(int _roomId, bool _isClear)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        Debug.Log("SendUnlockDoor_C2S_" + index);
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

        Debug.Log("SendPushPuzzle_C2S_" + index);
        PushPuzzleRoomMessage msg = new PushPuzzleRoomMessage();
        msg.roomId = _roomId;
        msg.connectionId = index;

        //send to server
        myClient.Send(MyMsgType.pushPuzzleMsgType_client, msg);
    }

    public void SendLockDoor_C2S(int _roomId)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        Debug.Log("SendLockDoor_C2S_" + index);
        LockDoorMessage msg = new LockDoorMessage();
        msg.roomId = _roomId;

        //send to server
        myClient.Send(MyMsgType.lockDoorMsgType_client, msg);
    }

    public void SendNumberToSpawn_C2S(int _roomId, int _spawnNumber, string _roomType)
    {
        NumberToSpawnMessage msg = new NumberToSpawnMessage();
        msg.roomId = _roomId;
        msg.spawnNumber = _spawnNumber;
        msg.roomType = _roomType;

        Debug.Log("SendNumberToSpawnDoor_C2S");

        myClient.Send(MyMsgType.NumberToSpawnMsgType_client, msg);
    }

    public void SendSpotPuzzle_C2S(SpotPuzzleRoomMessage _message)
    {
        SpotPuzzleRoomMessage msg = _message;
        Debug.Log("SendSpotPuzzle_C2S");

        myClient.Send(MyMsgType.spotPuzzleMsgType_client, msg);
    }

    public void SendPlayerId_C2S()
    {
        PlayerIdMessage msg = new PlayerIdMessage();
        msg.playerId = -12;
        Debug.Log("SendPlayerId_C2S");

        myClient.Send(MyMsgType.playerIdMsgType_client, msg);
    }

    public void SendItemCollected_C2S(int _roomId, int _itemCollected)
    {
        itemCollectedMessage msg = new itemCollectedMessage();
        msg.roomId = _roomId;
        msg.itemCollected = _itemCollected;
        Debug.Log("SendItemCollected_C2S");

        //if (NetworkServer.active)
        //    NetworkServer.SendToAll(MyMsgType.itemCollectedMsgType_client, msg);
        myClient.Send(MyMsgType.itemCollectedMsgType_client, msg);
    }

    public void SendActiveRoom_C2S(int _connectionId)
    {
        ActiveRoomMessage msg = new ActiveRoomMessage();
        msg.connectionId = _connectionId;
        Debug.Log("SendActiveRoom_C2S");

        myClient.Send(MyMsgType.activeRoomMsgType_client, msg);
    }

    public void SendItemChanged_C2S(int _playerId, string name)
    {
        ChangeItemMessage msg = new ChangeItemMessage();
        msg.itemName = name;
        msg.playerId = _playerId;
        Debug.Log("SendItemChanged_C2S");

        myClient.Send(MyMsgType.itemChangedMsgType_client, msg);
    }

    public void SendItemPossessed_C2S(int _playerId, string name)
    {
        ChangeItemMessage msg = new ChangeItemMessage();
        msg.itemName = name;
        msg.playerId = _playerId;
        Debug.Log("SendItemPossessed_C2S");

        myClient.Send(MyMsgType.itemPossessedMsgType_client, msg);
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
        Debug.Log("Host/ServerRecv_Position_: " + msg.position);
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
        for (int i = 0; i < Global.Instance.roomGen.roomDataList.Count; ++i)
            SendRoom_S2C(Global.Instance.roomGen.roomDataList[i], msg.connectionId);
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
            roomType = Global.Instance.roomGen.GetDefaultRoom();
        else
            roomType = Global.Instance.roomGen.GetRandomRoomList()[msg.roomType];


        GameObject room = Instantiate(roomType, msg.roomPos, Quaternion.identity);
        room.transform.localScale = msg.roomScale;
        room.GetComponent<RoomScript>().Set(msg.roomID, msg.gridX, msg.gridY,
            !msg.isLeft, !msg.isRight, !msg.isUp, !msg.isDown);
        //determine if room is already visited b4 hand
        room.SetActive(msg.isActive);
        if (msg.isActive)
            room.GetComponent<RoomScript>().SetIsComplete(msg.isCompleted);
        Global.Instance.roomGen.StoreRoom(msg.roomID + 1, msg.gridX, msg.gridY, room);
        //if (msg.roomID != 0)
        Global.Instance.roomGen.roomDataList.Add(msg);

        Debug.Log("ClientRecv_Room");
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
        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<RoomScript>().SetPuzzleComplete(msg.isClear);
    }

    public void OnRecvUnlockDoor_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        UnlockDoorMessage msg = netMsg.ReadMessage<UnlockDoorMessage>();

        Debug.Log("ClientRecv_UnlockDoor : room_" + msg.roomId + " -> " + msg.isClear);
        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<RoomScript>().SetPuzzleComplete(msg.isClear);
    }

    //recv push puzzle
    public void OnRecvPushPuzzle_Server(NetworkMessage netMsg)
    {
        PushPuzzleRoomMessage msg = netMsg.ReadMessage<PushPuzzleRoomMessage>();

        Debug.Log("Host/ServerRecv_PushPuzzle_" + msg.connectionId + ": room_" + msg.roomId);

        msg.objectivePos = Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().GetObjectivePos();
        msg.targetPos = Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().GetTargetPos();

        SendPushPuzzle_S2C(msg.connectionId, msg.roomId, msg.objectivePos, msg.targetPos);
    }

    public void OnRecvPushPuzzle_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        PushPuzzleRoomMessage msg = netMsg.ReadMessage<PushPuzzleRoomMessage>();
        Debug.Log("ClientRecv_PushPuzzle_" + index + ": room_" + msg.roomId);


        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().SetObjectivePos(msg.objectivePos);
        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<PushPuzzleRoomScript>().SetTargetPos(msg.targetPos);
    }

    //recv lock door
    public void OnRecvLockDoor_Server(NetworkMessage netMsg)
    {
        LockDoorMessage msg = netMsg.ReadMessage<LockDoorMessage>();

        Debug.Log("Host/ServerRecv_LockDoor : room_" + msg.roomId);
        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<RoomScript>().LockAllDoor();
        SendLockDoor_S2C(msg.roomId);
    }

    public void OnRecvLockDoor_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        LockDoorMessage msg = netMsg.ReadMessage<LockDoorMessage>();

        Debug.Log("ClientRecv_LockDoor : room_" + msg.roomId);
        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<RoomScript>().LockAllDoor();
    }

    //recv spawn num
    public void OnRecvNumberToSpawn_Server(NetworkMessage netMsg)
    {
        NumberToSpawnMessage msg = netMsg.ReadMessage<NumberToSpawnMessage>();

        Debug.Log("Host/ServerRecv_NumberToSpawn : room_" + msg.roomId);
    

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
            if (Global.Instance.roomGen.GetRoomList()[msg.roomId] == null)
                Debug.Log("Room not started yet");

            Global.Instance.roomGen.GetRoomList()[msg.roomId].
                GetComponent<EnemyRoomScript>().GetSpawner().
                GetComponent<EnemySpawner>().SetTotalWave(msg.spawnNumber);

            //RoomGenerator.Instance.GetRoomList()[msg.roomId].GetComponent<EnemyRoomScript>().totalNumWave = msg.spawnNumber;
        }
        else if(msg.roomType == "speedRoom")
        {
            Global.Instance.roomGen.
               GetRoomList()[msg.roomId].
               GetComponent<SpeedRoomScript>().
               SetSpawnerScript();

           Global.Instance.roomGen.
                GetRoomList()[msg.roomId].
                GetComponent<SpeedRoomScript>().
                GetSpawnerScript().maxSpawns = msg.spawnNumber;
        }
    }

    public void OnRecvSpotPuzzle_Server(NetworkMessage netMsg)
    {
        SpotPuzzleRoomMessage msg = netMsg.ReadMessage<SpotPuzzleRoomMessage>();
        Debug.Log("Host/ServerRecv_SpotPuzzle : room_" + msg.roomId);
    }

    public void OnRecvSpotPuzzle_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        SpotPuzzleRoomMessage msg = netMsg.ReadMessage<SpotPuzzleRoomMessage>();

        Debug.Log("ClientRecv_SpotPuzzle : room_" + msg.roomId);
        //reinit the pos of the puzzles to match with host
        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<SpotPuzzleRoomScript>().
            RepositionObjectClient(msg);

    }

    public void OnRecvPlayerId_Server(NetworkMessage netMsg)
    {
        PlayerIdMessage msg = netMsg.ReadMessage<PlayerIdMessage>();
        Debug.Log("Host/ServerRecv_PlayerId");
        SendPlayerId_S2C();
    }

    public void OnRecvPlayerId_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        PlayerIdMessage msg = netMsg.ReadMessage<PlayerIdMessage>();
        Debug.Log("ClientRecv_PlayerId_" + msg.playerId);
        Global.Instance.player.GetComponent<Player>().playerId = msg.playerId;
        index = msg.playerId;

        Inventory temp = new Inventory();
        temp.Init();
        temp.AddCurrency(InventoryManager.Instance.GetInventory("player").GetCurrency());
        InventoryManager.Instance.AddInventory("player" + index, temp);
        Debug.Log("player" + index);
        Global.Instance.player.GetComponent<Player>().SetPlayerInventory("player" + index);

        //for (int i = 0; i < InventoryManager.Instance.GetInventory("player" + index).GetItemNameList().Count; ++i)
        //    SendItemPossessed_C2S(index, InventoryManager.Instance.GetInventory("player").GetItemName(i));

        SendRoom_C2S(); //sent to server/host to get mapinfo
    }

    public void OnRecvItemCollected_Server(NetworkMessage netMsg)
    {
        itemCollectedMessage msg = netMsg.ReadMessage<itemCollectedMessage>();
        Debug.Log("Host/ServerRecv_itemCollected = " + msg.itemCollected);

        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<SpeedRoomScript>().SetItemCollected(msg.itemCollected);
    }

    public void OnRecvItemCollected_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        itemCollectedMessage msg = netMsg.ReadMessage<itemCollectedMessage>();
        Debug.Log("ClientRecv_itemCollected = " + msg.itemCollected);

        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<SpeedRoomScript>().SetItemCollected(msg.itemCollected);
    }

    public void OnRecvActiveRoom_Server(NetworkMessage netMsg)
    {
        ActiveRoomMessage msg = netMsg.ReadMessage<ActiveRoomMessage>();
        Debug.Log("Host/ServerRecv_ActiveRoom_" + msg.connectionId);

        for(int i = 0;i < Global.Instance.roomGen.GetRoomList().Count; ++i)
        {
            if(Global.Instance.roomGen.GetRoomList()[i].activeSelf)
                SendActiveRoom_S2C(msg.connectionId, i, msg.isRoomCompleted);
        }
    }

    public void OnRecvActiveRoom_Client(NetworkMessage netMsg)
    {
        if (Global.Instance.player.GetComponent<NetworkIdentity>().isServer)
            return;

        ActiveRoomMessage msg = netMsg.ReadMessage<ActiveRoomMessage>();
        Debug.Log("Host/ServerRecv_ActiveRoom_" + msg.connectionId);

        Global.Instance.roomGen.GetRoomList()[msg.roomId].SetActive(true);
        Global.Instance.roomGen.GetRoomList()[msg.roomId].GetComponent<RoomScript>().SetIsComplete(msg.isRoomCompleted);
    }

    public void OnRecvItemChanged_Server(NetworkMessage netMsg)
    {
        ChangeItemMessage msg = netMsg.ReadMessage<ChangeItemMessage>();
        Debug.Log("Host/ServerRecv_ItemChanged_");

        for(int i = 0; i < Global.Instance.listOfPlayer.Length; ++i)
        {
            if(Global.Instance.listOfPlayer[i].GetComponent<Player>().playerId == msg.playerId)
            {
                string iName = Global.Instance.listOfPlayer[i].GetComponent<Player>().playerInventory;
                Debug.Log(msg.itemName);
                Global.Instance.listOfPlayer[i].GetComponent<PlayerShoot>().PlayerChangedWeapon(msg.itemName);
            }
        }
    }

    public void OnRecvItemChanged_Client(NetworkMessage netMsg)
    {
        ChangeItemMessage msg = netMsg.ReadMessage<ChangeItemMessage>();
        Debug.Log("ClientRecv_ItemChanged_");
    }

    public void OnRecvItemPossessed_Server(NetworkMessage netMsg)
    {
        ItemsPossessedMessage msg = netMsg.ReadMessage<ItemsPossessedMessage>();
        Debug.Log("Host/ServerRecv_ItemPossessed");

        for (int i = 0; i < Global.Instance.listOfPlayer.Length; ++i)
        {
            if (Global.Instance.listOfPlayer[i].GetComponent<Player>().playerId == msg.playerId)
            {
                string iName = Global.Instance.listOfPlayer[i].GetComponent<Player>().playerInventory;
                InventoryManager.Instance.AddInventory(iName);
                Debug.Log(msg.itemName);
                InventoryManager.Instance.GetInventory(iName).AddItem(ItemManager.Instance.items[msg.itemName], msg.itemName);
            }
        }
    }

    public void OnRecvItemPossessed_Client(NetworkMessage netMsg)
    {
        ItemsPossessedMessage msg = netMsg.ReadMessage<ItemsPossessedMessage>();
        Debug.Log("ClientRecv_ItemPossessed");
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


/* For reference
 
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
 
public class MyNetworkManager : NetworkManager {
 
    public override void OnServerDisconnect (NetworkConnection conn)
    {
        base.OnServerDisconnect (conn);
    }
 
    public override void OnClientDisconnect (NetworkConnection conn)
    {
        base.OnClientDisconnect (conn);
    }
}

 */
