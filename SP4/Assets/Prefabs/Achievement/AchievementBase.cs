using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementBase : MonoBehaviour {

    [System.NonSerialized]
    public AchievementSystem aSystem = null;
    
    public int id { get { return id; } set { this.id = value; } } //unique id of achievement, can be used to retrieve data from player pref
    [SerializeField]
    public string ach_name; //name of the achievement to be shown to players
    [SerializeField]
    public string description;//description of achievement, aka requirement in text
    //optional 2 variable below
    [System.NonSerialized]
    public int curr_progress;//numerical representation of the current progress to the max
    [SerializeField]
    public int max_progress; //if curr progress == max, means completed
    public float progress_percent { get { return (float)curr_progress / (float)max_progress; } }
    [System.NonSerialized]
    public bool completed = false;
    [SerializeField]
    PREFTYPE _prefType;
    string prefString { get { return _prefType.ToString(); } }
    
    //public delegate void a_func(Game_Info info);
    //public a_func func; //calculation for the progress
    
    public void Set(int uid, string ach_name, string description, int currprog, int maxprog, bool completed)
    {
        this.id = uid; this.ach_name = ach_name;
        this.description = description; this.curr_progress = currprog; this.max_progress = maxprog; this.completed = completed;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.completed)
        {
            if (IsCompletedCheck())
                aSystem.AchievementUnlocked(this);
        }
    }

    public bool IsCompletedCheck()
    {
        //everything is eiter 0,1 or 1-~
        this.curr_progress = PlayerPrefs.GetInt(prefString, 0);
        if (this.curr_progress >= this.max_progress)
        {
            this.completed = true;
            return true;
        }
        return false;
    }
}
