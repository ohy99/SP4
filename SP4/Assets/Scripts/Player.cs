using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int initialScore;
    private int score;
    private Health hpScript;
    private int exp;
    private int maxExp;
    private int skillPoints;
    private int currentLevel;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (hpScript.GetCurrHp() <= 0)
        {
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