using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventController : Singleton<MyEventController> {

    [SerializeField]
    GameObject endGamePanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerEndGame(bool trigger)
    {
        if (trigger)
        {
            if (!endGamePanel.activeSelf)
                endGamePanel.SetActive(true);
        }
        else
        {
            if (endGamePanel.activeSelf)
                endGamePanel.SetActive(false);
        }
    }
}
