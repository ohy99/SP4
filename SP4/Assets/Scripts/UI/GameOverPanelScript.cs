using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelScript : MonoBehaviour {

    private string textResult;

    private Text text;

	// Use this for initialization
	void Start () {
        textResult = (Global.Instance.victory ? "You win" : "You are dead");

        text = this.transform.GetChild(0).GetComponent<Text>();

        text.text = "GAMEOVER\n" + textResult + "\nYour score is " + PlayerPrefs.GetInt(PREFTYPE.NUM_OF_KILLS.ToString(), 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BackButton()
    {
        LoadScene.Instance.LoadSceneCall("mainmenu");
    }
}
