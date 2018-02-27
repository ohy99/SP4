using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinsInfo : MonoBehaviour {

    [SerializeField]
    Text textfield;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        textfield.text = InventoryManager.Instance.GetInventory("player").GetCurrency().ToString();
	}
}
