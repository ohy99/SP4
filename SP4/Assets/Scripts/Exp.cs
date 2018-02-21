using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour {

    [SerializeField]
    Player player;
    Slider expBar;

    private GameObject playerGO;

	// Use this for initialization
	void Start () {
        expBar = GetComponent<Slider>();
        playerGO = Global.Instance.player;
    }
	
	// Update is called once per frame
	void Update () {
        if (player)
            expBar.value = Mathf.Lerp(expBar.value, (Mathf.Max(0.04f, Mathf.Min(1.0f, player.GetExp() / (float)player.GetMaxExp()))), 10.0f * Time.deltaTime);
        else //to work with network stuff
            expBar.value = Mathf.Lerp(expBar.value, (Mathf.Max(0.04f, Mathf.Min(1.0f, playerGO.GetComponent<Player>().GetExp() / (float)playerGO.GetComponent<Player>().GetMaxExp()))), 10.0f * Time.deltaTime);
	}
    
}
