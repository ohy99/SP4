﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnLoadSceneButtonClick(string sceneName)
    {
        LoadScene.Instance.LoadSceneCall(sceneName);
    }

    public void OnwadButtonClick()
    {
        LoadScene.Instance.LoadSceneCall("achievements");
    }
}
