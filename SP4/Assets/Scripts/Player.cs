using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int score;
    private int health;
    private int maxHealth;

    // Use this for initialization
    void Start() {
        score = PlayerPrefs.GetInt("Score", 0);
        health = PlayerPrefs.GetInt("Health", 10);
        maxHealth = PlayerPrefs.GetInt("MaxHealth", 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            health++;
        }

            if (Input.GetKeyDown(KeyCode.Minus))
        {
            health--;
        }
    }

    void Save()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Max Health", maxHealth);
        PlayerPrefs.Save();

    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}