using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllAchievements : MonoBehaviour {

    [SerializeField]
    float height = 50;
    [SerializeField]
    GameObject achPanel;
    [SerializeField]
    RectTransform parent;

    [SerializeField]
    List<AchievementBase> achievementPrefabList = new List<AchievementBase>();
    List<AchievementBase> achList;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
        achList = new List<AchievementBase>();
        UnityEngine.UI.VerticalLayoutGroup vlg = GetComponent<VerticalLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1, rectTransform.rect.height * achievementPrefabList.Count);
        rectTransform.position = new Vector3(rectTransform.position.x, -1000, 0); //force to see the first
        foreach (AchievementBase a in achievementPrefabList)
        {
            GameObject panel = Instantiate(achPanel);
            panel.transform.SetParent(this.gameObject.transform);

            GameObject titleObj = panel.transform.Find("Title").gameObject;
            GameObject descriptionObj = panel.transform.Find("Description").gameObject;
            GameObject sliderObj = panel.transform.Find("Slider").gameObject;

            Text titleText = titleObj.GetComponent<Text>();
            titleText.text = a.ach_name;
            Text desText = descriptionObj.GetComponent<Text>();
            desText.text = a.description;
            Slider slider = sliderObj.GetComponent<Slider>();
            slider.value = a.progress_percent;

            GameObject valueobj = sliderObj.transform.Find("ProgressValue").gameObject;
            Text valuetext = valueobj.GetComponent<Text>();
            valuetext.text = a.curr_progress.ToString() + "/" + a.max_progress.ToString();

            achList.Add(a);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
