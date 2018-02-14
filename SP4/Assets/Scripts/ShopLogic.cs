using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLogic : MonoBehaviour
{
    [SerializeField]
    GameObject shopCanvas;


    GameObject go;
    // Use this for initialization
    void Start ()
    {
        // Load items in
        shopCanvas.SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
	}




    public void SetShopActive(bool _isActive)
    {
        shopCanvas.SetActive(_isActive);
    }
}
