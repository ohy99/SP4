using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    [SerializeField]
    bool isInvincible = false;

    [SyncVar]
    private float currHp;
    private float maxHp;

    [SerializeField]
    private GameObject coinPrefab;

	// Use this for initialization
	void Start () {
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

        //syncing of hp
        if (!isServer)
            return;

        currHp += value;

        if (currHp <= 0)
        {
            if (gameObject.tag.Equals("EnemyBoss"))
            {
                Global.Instance.bossIsDead = true;
                Global.Instance.victory = true;
            }
            if (!gameObject.tag.Equals("Player"))
            {
                PlayerPrefs.SetInt(PREFTYPE.NUM_OF_KILLS.ToString(), PlayerPrefs.GetInt(PREFTYPE.NUM_OF_KILLS.ToString(), 0) + 1);
                Global.Instance.player.SendMessage("AddScore", 2);

                float chance = Random.Range(0.0f, 1.0f);
                if (chance > 0.75f)
                {
                    GameObject temp = Instantiate(coinPrefab);
                    temp.transform.position = this.transform.position;
                }

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
