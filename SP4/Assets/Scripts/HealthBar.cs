using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private float fillAmount;

    public GameObject player;

    private Image image;
    private Health hpScript;

    // Use this for initialization
    void Start ()
    {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!player) //to work with network stuff
        {
            if (!Global.Instance.player)
                return;


                player = Global.Instance.player;
            Debug.Log(player);

            hpScript = player.GetComponent<Health>();
            Debug.Log(hpScript); ;
            fillAmount = Mathf.Lerp(fillAmount, Global.Instance.player.GetComponent<Health>().GetCurrHp() / Global.Instance.player.GetComponent<Health>().GetMaxHp(), 10.0f * Time.deltaTime);
            image.fillAmount = fillAmount;
            return;
        }


        fillAmount = Mathf.Lerp(fillAmount, hpScript.GetCurrHp() / hpScript.GetMaxHp(), 10.0f  * Time.deltaTime);
        image.fillAmount = fillAmount;
	}
}
