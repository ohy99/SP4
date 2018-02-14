using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int initialScore;
    private int score;
    private float health;
    private float maxHealth;
    private int exp;
    private int maxExp;
    private int skillPoints;
    private int currentLevel;

    // Use this for initialization
    void Start() {
        score = PlayerPrefs.GetInt("Score", 0);
        initialScore = score;
        health = PlayerPrefs.GetFloat("Health", 10);
        maxHealth = PlayerPrefs.GetFloat("Max Health", 100);
        health = maxHealth;

        //Starts with no exp when entered game
        exp = 0;
        maxExp = 100;
        skillPoints = 0;
        currentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Save();
        }

        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(KeyCode.Equals))
            {
                health++;
            }

            if (Input.GetKey(KeyCode.Minus))
            {
                health--;
            }
        }

        health = Mathf.Clamp(health, 0, maxHealth);
    }

    void Save()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetFloat("Max Health", maxHealth);
        PlayerPrefs.SetInt("Money", Mathf.Max(0,score - initialScore));
        PlayerPrefs.Save();
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void AddHealth(float health)
    {
        this.health += health;
    }

    public void IncreaseMaxHealth(float increment)
    {
        this.maxHealth += increment;
        this.health += increment;
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
    public bool IsDead() { return health <= 0; }
}