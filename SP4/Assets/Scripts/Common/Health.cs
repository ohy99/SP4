using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    bool isInvincible = false;

    private float currHp;
    private float maxHp;

	// Use this for initialization
	void Start () {
        maxHp = 10;
        currHp = maxHp;
	}
	
	// Update is called once per frame
	void Update () {
        currHp = Mathf.Clamp(currHp, 0, maxHp);
    }

    public float GetCurrHp()
    {
        return currHp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }
    
    public void ModifyHp(float value)
    {
        if (isInvincible)
            return;

        currHp += value;

        if (currHp <= 0)
        {
            if (gameObject.tag.Equals("EnemyBoss"))
                Global.Instance.bossIsDead = true;
            if (!gameObject.tag.Equals("Player"))
            {
                PlayerPrefs.SetInt(PREFTYPE.NUM_OF_KILLS.ToString(), PlayerPrefs.GetInt(PREFTYPE.NUM_OF_KILLS.ToString(), 0) + 1);
                Destroy(gameObject);
            }
        }
    }

    public void IncreaseMaxHp(float increment)
    {
        this.maxHp += increment;
        this.currHp += increment;
    }

    public void SetHp(float maxHp)
    {
        this.maxHp = maxHp;
        this.currHp = this.maxHp;
    }
}
