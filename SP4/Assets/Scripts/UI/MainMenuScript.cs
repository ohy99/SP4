using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartButtonClick()
    {
        LoadScene.Instance.LoadSceneCall("game");
    }

    public void OnOnlineButtonClick()
    {
        LoadScene.Instance.LoadSceneCall("mp_game");
    }
}
