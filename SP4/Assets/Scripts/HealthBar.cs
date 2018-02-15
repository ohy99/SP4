using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private float fillAmount;

    public GameObject player;

    private Image image;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        fillAmount = Mathf.Lerp(fillAmount, player.GetComponent<Health>().GetCurrHp() / player.GetComponent<Health>().GetMaxHp(), 10.0f  * Time.deltaTime);

        image.fillAmount = fillAmount;
	}
}
