using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerNavManager : MonoBehaviour {

    [SerializeField]
    GameObject mainMenuCN;

    [SerializeField]
    GameObject shopCN;
    [SerializeField]
    GameObject shopCanvas;

    [SerializeField]
    GameObject optionsCN;
    [SerializeField]
    GameObject optionsPanel;

    [SerializeField]
    GameObject confirmationPanel;

    [SerializeField]
    GameObject connectionnCN;
    [SerializeField]
    GameObject connectionPanel;

    GameObject currActive;

	// Use this for initialization
	void Start () {
        currActive = mainMenuCN;
	}

    // Update is called once per frame
    void Update()
    {
        if (shopCanvas.activeSelf)
        {
            if (shopCN.activeSelf == false)
            {
                currActive.SetActive(false);
                currActive = shopCN;
                currActive.SetActive(true);
            }
            else
            {
                if (confirmationPanel.activeSelf)
                {
                    shopCN.SetActive(false);
                }
                else
                {
                    shopCN.SetActive(true);
                }
            }
        }

        else if (optionsPanel.activeSelf)
        {
            if (optionsCN.activeSelf == false)
            {
                currActive.SetActive(false);
                currActive = optionsCN;
                currActive.SetActive(true);
            }
        }
        else if (connectionPanel.activeSelf)
        {
            if (connectionnCN.activeSelf ==false)
            {
                currActive.SetActive(false);
                currActive = connectionnCN;
                currActive.SetActive(true);
            }
        }
        else
        {
            //all inactive
            if (mainMenuCN.activeSelf == false)
            {
                currActive.SetActive(false);
                currActive = mainMenuCN;
                currActive.SetActive(true);
            }

        }


    }
	
}
