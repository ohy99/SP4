using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

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
        currHp += value;
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
