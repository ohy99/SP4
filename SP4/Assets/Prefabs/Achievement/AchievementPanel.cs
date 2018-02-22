using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanel : MonoBehaviour {

    [SerializeField]
    Text titleText;
    [SerializeField]
    Text descriptionText;

    public void Init(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
