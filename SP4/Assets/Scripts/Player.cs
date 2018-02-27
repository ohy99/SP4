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
    //dont change player id = connection id
    public int playerId;
    bool onDeadTrigger = false;

    GameObject[] playersList;

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = Color.green;

        //wiil tis worko
        //playersList = GameObject.FindGameObjectsWithTag("Player");
        //for (int i = 0; i < playersList.Length; i++)
        //{
        //    if (playersList[i].GetComponent<NetworkIdentity>().isLocalPlayer == true)
        //    {
        //        //set global instance of player to local player
        //        Global.Instance.player = playersList[i];
        //        break;
        //    }
        //}

        Camera.main.GetComponent<CameraScript>().playerTransform = gameObject.transform;

        Debug.Log("connectionId: " + NetworkClient.allClients[0].connection.connectionId);

        Global.Instance.player = gameObject;

        Debug.Log("Starting up");

        MessageHandler.Instance.Register(NetworkClient.allClients[0]);

        if (isServer)
            playerId = NetworkServer.connections.Count - 1;
        else //send msg to server to get da id
        {
            Debug.Log("Sending for id");
            MessageHandler.Instance.SendPlayerId_C2S();
            
        }

        if (isServer)
            Global.Instance.roomGen.Init();
            //RoomGenerator.Init();
        //else
          //  MessageHandler.Instance.SendRoom_C2S(); //sent to server/host to get mapinfo
    }

    // Use this for initialization
    void Start()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        initialScore = score;
        float maxHealth = PlayerPrefs.GetFloat("Max Health", 100);

        //float maxHealth = 100;
        hpScript = gameObject.GetComponent<Health>();

        hpScript.SetHp(maxHealth);

        //Starts with no exp when entered game
        exp = 0;
        maxExp = 100;
        skillPoints = 0;
        currentLevel = 1;

        //Global.Instance.player = gameObject;

        if (!NetworkServer.active)
            Debug.Log("SERVER NOT ACTIVE");
        else
            Debug.Log("SERVER ACTIVE");


        if (isServer)
            playerId = NetworkServer.connections.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.L))
        {   //this for testing 

            if (isServer)
            {
                Debug.Log("keypressed");
                Debug.Log(NetworkServer.connections.Count);
                for (int i = 0; i < NetworkServer.connections.Count; ++i)
                {
                    Debug.Log("ConnectionId: " + NetworkServer.connections[i].connectionId);
                }
            }

            //MessageHandler.Instance.SendPosition_C2S(gameObject.transform.position);
        }

        if (hpScript.GetCurrHp() <= 0)
        {
            if (!onDeadTrigger)
            {
                PlayerPrefs.SetInt(PREFTYPE.NUM_OF_DEATHS.ToString(), PlayerPrefs.GetInt(PREFTYPE.NUM_OF_DEATHS.ToString(), 0) + 1);
                onDeadTrigger = true;
            }
            Save();

            if (onDeadTrigger)
            {
                //Debug.Log("Test");
                Global.Instance.victory = false;
                LoadScene.Instance.LoadSceneCall("GameOver");
            }
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

/*
 * server will set id of clients that connects -> set in start
 * client will msg server to get index
 */