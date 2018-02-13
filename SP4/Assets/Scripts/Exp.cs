using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour {

    [SerializeField]
    Player player;
    Slider expBar;

	// Use this for initialization
	void Start () {
        expBar = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        expBar.value = Mathf.Max(0.04f, Mathf.Min(1.0f, player.GetExp() / (float)player.GetMaxExp()));
	}
    
}
