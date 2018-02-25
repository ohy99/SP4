using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private int initialScore;
    private int score;
    private Health hpScript;
    private int exp;
    private int maxExp;
    private int skillPoints;
    private int currentLevel;
    private int clientIndex;
    bool onDeadTrigger = false;
    //GameObject[] playersList;

    public override void OnStartLocalPlayer()
    {

        //if (!NetworkServer.active)
        //    Debug.Log("SERVER NOT ACTIVE");
        //else
        //    Debug.Log("SERVER ACTIVE");

        GetComponent<SpriteRenderer>().color = Color.green;
        
        if(Global.Instance.player == null)
        {
            Debug.Log("global player is null");
        }
        else
            Debug.Log("global player in use wtf");

        //wiil tis worko
        //playersList = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log("numPlayer: " + playersList.Length);
        //for (int i = 0; i < playersList.Length; i++)
        //{
        //    if (playersList[i].GetComponent<NetworkIdentity>().isLocalPlayer == true)
        //    {
        //        Global.Instance.player = playersList[i];
        //        break;
        //    }
        //}

        Global.Instance.player = gameObject;
        Camera.main.GetComponent<CameraScript>().playerTransform = gameObject.transform;

        //Debug.Log(Network.player.ToString());
        //clientIndex = NetworkClient.allClients[0].GetHashCode();
        //Debug.Log("REGISTER_INDEX: " + clientIndex);
        //MessageHandler.Instance.index = clientIndex;
        MessageHandler.Instance.Register(NetworkClient.allClients[0]);
        MessageHandler.Instance.index = Network.player.GetHashCode();

        if (isServer)
            RoomGenerator.Instance.Init();
        else
            MessageHandler.Instance.SendRoom_C2S(); //sent to server/host to get mapinfo

        //for (int i = 0; i < RoomGenerator.Instance.roomDataList.Count; ++i)
        //    Debug.Log(RoomGenerator.Instance.roomDataList[i].roomID);

        //for (int i = 0; i < RoomGenerator.Instance.GetRoomList().Count; ++i)
        //    Debug.Log(RoomGenerator.Instance.GetRoomList()[i].GetComponent<RoomScript>().GetRoomID());
    }


    // Use this for initialization
    void Start() {
        score = PlayerPrefs.GetInt("Score", 0);
        initialScore = score;
        float maxHealth = PlayerPrefs.GetFloat("Max Health", 100);

        hpScript = gameObject.GetComponent<Health>();

        hpScript.SetHp(maxHealth);

        //Starts with no exp when entered game
        exp = 0;
        maxExp = 100;
        skillPoints = 0;
        currentLevel = 1;

        Global.Instance.player = gameObject;

        if (!NetworkServer.active)
            Debug.Log("SERVER NOT ACTIVE");
        else
            Debug.Log("SERVER ACTIVE");

  
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer && Input.GetKeyUp(KeyCode.L))
        {   //this for testing 
            //if (GetComponent<MessageHandler>().myClient != null)
            //    Debug.Log("server_send");
            MessageHandler.Instance.SendPosition_C2S(gameObject.transform.position);
        }

        if (hpScript.GetCurrHp() <= 0)
        {
            if (!onDeadTrigger)
            {
                PlayerPrefs.SetInt(PREFTYPE.NUM_OF_DEATHS.ToString(), PlayerPrefs.GetInt(PREFTYPE.NUM_OF_DEATHS.ToString(), 0) + 1);
                onDeadTrigger = true;
            }
            Save();
        }

        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(KeyCode.Equals))
            {
                hpScript.ModifyHp(1);
            }

            if (Input.GetKey(KeyCode.Minus))
            {
                hpScript.ModifyHp(-1);
            }
        }
        gameObject.GetComponent<Rigidbody2D>().velocity.Set(0, 0);
    }

    void Save()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetFloat("Max Health", hpScript.GetMaxHp());
        PlayerPrefs.SetInt("Money", Mathf.Max(0,score - initialScore));

        /*
         * By default Unity writes preferences to disk during OnApplicationQuit(). 
         * In cases when the game crashes or otherwise prematuraly exits, you might want to write the PlayerPrefs at sensible 'checkpoints' in your game. 
         * This function will write to disk potentially causing a small hiccup, therefore it is not recommended to call during actual gameplay.
         */
        PlayerPrefs.Save();
        
    }

    public void IncreaseSpeed()
    {
        this.GetComponent<PlayerMovement>().moveSpeed += 5.0f;
    }

    public int GetSkillPoint()
    {
        return skillPoints;
    }

    public void DecreaseSkillPoint()
    {
        skillPoints -= 1;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void IncExp(int exp)
    {
        this.exp += exp;
        Debug.Log("exp: " + this.exp);

        if (this.exp >= this.maxExp)
        {
            this.exp -= this.maxExp;
            currentLevel += 1;
            skillPoints += 1;
            maxExp += 20;
            Debug.Log("exp: " + this.exp);
        }
    }
    public int GetExp() { return exp; }
    public int GetMaxExp() { return maxExp; }
    public bool IsDead() { return hpScript.GetCurrHp() <= 0; }
    public void AddHealth(float value) { hpScript.ModifyHp(value); }
}