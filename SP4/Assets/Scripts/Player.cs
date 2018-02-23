using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    private int initialScore;
    private int score;
    private Health hpScript;
    private int exp;
    private int maxExp;
    private int skillPoints;
    private int currentLevel;
    bool onDeadTrigger = false;

    //public override void OnStartLocalPlayer()
    //{
    //    short id = GetComponent<NetworkIdentity>().playerControllerId;
    //    Debug.Log("playerID" + id);
    //    GetComponent<SpriteRenderer>().color = Color.green;
    //    Global.Instance.player = gameObject;
    //    Camera.main.GetComponent<CameraScript>().playerTransform = gameObject.transform;
    //}


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
    }

    // Update is called once per frame
    void Update()
    {
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