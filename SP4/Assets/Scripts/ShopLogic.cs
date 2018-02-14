using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogic : MonoBehaviour
{
    [SerializeField]
    GameObject shopCanvas;

    public GameObject buttonPrefab;
    public GameObject panelToAttachButtonsTo;

    void Start ()
    {
        // Load items in
        shopCanvas.SetActive(true);

        GameObject button = (GameObject)Instantiate(buttonPrefab);
        button.transform.SetParent(panelToAttachButtonsTo.transform);//Setting button parent
        button.GetComponent<Button>().onClick.AddListener(OnClick);//Setting what button does when clicked
                                                                   //Next line assumes button has child with text as first gameobject like button created from GameObject->UI->Button
        button.transform.GetChild(0).GetComponent<Text>().text = "This is button text";//Changing text
    }
	void OnClick()
    {
        Debug.Log("ITEM");
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
