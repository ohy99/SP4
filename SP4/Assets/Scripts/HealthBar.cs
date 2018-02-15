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
    void Start () {
        image = GetComponent<Image>();
        hpScript = player.GetComponent<Health>();
	}
	
	// Update is called once per frame
	void Update () {
        fillAmount = Mathf.Lerp(fillAmount, hpScript.GetCurrHp() / hpScript.GetMaxHp(), 10.0f  * Time.deltaTime);

        image.fillAmount = fillAmount;
	}
}
