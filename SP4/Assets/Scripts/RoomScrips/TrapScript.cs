using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour {

    private float damageValue = 20.0f;
    private float elapsedTime = 0.0f;
    private float activeTime = 5.0f;
    private float prepTime = 3.0f;

    private bool isActive = false;
    private bool alreadyTriggered = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        if (isActive)
        {
            if (activeTime < elapsedTime)
            {
                elapsedTime = 0.0f;
                isActive = false;
            }

            Color temp = gameObject.GetComponent<SpriteRenderer>().color;
            temp.a = Mathf.Lerp(temp.a, 1, 5.0f * Time.deltaTime);
            gameObject.GetComponent<SpriteRenderer>().color = temp;
        }
        else
        {
            if (prepTime < elapsedTime)
            {
                elapsedTime = 0.0f;
                isActive = true;
                alreadyTriggered = false;
            }

            Color temp = gameObject.GetComponent<SpriteRenderer>().color;
            temp.a = Mathf.Lerp(temp.a, 0, 5.0f * Time.deltaTime);
            gameObject.GetComponent<SpriteRenderer>().color = temp;
        }
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player") && isActive && !alreadyTriggered)//if the collided is player OR player(clone)
        {
            alreadyTriggered = true;
            col.gameObject.SendMessage("TakeDamage", damageValue);
        }
    }
}
