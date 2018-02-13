using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int score;
    private float health;
    private float maxHealth;

    // Use this for initialization
    void Start() {
        score = PlayerPrefs.GetInt("Score", 0);
        health = PlayerPrefs.GetFloat("Health", 10);
        maxHealth = PlayerPrefs.GetFloat("Max Health", 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
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

            health = Mathf.Clamp(health, 0, maxHealth);
        }

    }

    void Save()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetFloat("Max Health", maxHealth);
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
}