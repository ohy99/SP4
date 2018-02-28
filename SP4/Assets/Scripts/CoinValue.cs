using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinValue : MonoBehaviour {

    public int value = 1;

    public GameObject textPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        GameObject text = Instantiate(textPrefab);

        text.transform.position = this.transform.position;

        text.GetComponent<CoinText>().coinvalue = value;

        InventoryManager.Instance.GetInventory("player").AddCurrency(value);
        Destroy(this.gameObject);
    }
}
