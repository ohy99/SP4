using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private float fillAmount;

    private Player player;

    private Image image;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent< Player >();
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        fillAmount = (float)player.GetHealth() / (float)player.GetMaxHealth() ;

        image.fillAmount = fillAmount;
	}
}
