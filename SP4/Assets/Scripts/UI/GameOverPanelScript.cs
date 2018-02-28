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

        int score = PlayerPrefs.GetInt("Score", 0);

        int highScore;

        highScore = PlayerPrefs.GetInt("Highscore", 0);

        if (highScore < score)
        {
            highScore = score;
            PlayerPrefs.SetInt("Highscore", highScore);
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetInt("Highscore", highScore);
        PlayerPrefs.Save();

        text.text = "GAMEOVER\n" + textResult + "\nYour score is " + score + "\nYour highscore is " + highScore;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void BackButton()
    {
        LoadScene.Instance.LoadSceneCall("mainmenu");
    }
}
