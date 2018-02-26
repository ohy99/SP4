using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePanelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MainMenuButton()
    {
        LoadScene.Instance.LoadSceneCall("mainmenu");
    }

    public void GameOverButton()
    {
        Global.Instance.roomGen.ReStart();
    }
}
