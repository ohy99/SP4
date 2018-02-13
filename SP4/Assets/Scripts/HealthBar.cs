using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private float fillAmount;

    public Player player;

    private Image image;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        fillAmount = player.GetHealth() / (float)player.GetMaxHealth();

        image.fillAmount = fillAmount;
	}
}
