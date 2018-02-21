using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : Singleton<AchievementSystem> {

    public class Game_Info
    {
        /*
         * base class for game info to be passed to achievement as parameter to calculate the progress
         * other classes should expose their variables to achievementsysstem, and not invite achievement system to go in and take the value
         * 
         * i can make a achievement as one object so one obj one script, neater
         * BUT harder to see the whole picture
         * this way i can see all the values i need, and they can share the values, so easier handle the getter for the values (no duplicates)
         * 
         * get ready for huge file size bois
         * 
         * store in primivite types only
        */

        
        
    }


    //Should be heavily using playerpref

    [SerializeField]
    List<GameObject> achievementPrefabList = new List<GameObject>();
    List<AchievementBase> achievementList;
    Game_Info gameInfo;

    //[SerializeField]
    //AchievementBase wad;

    int U_ID = 0;
    int GetUID() { return U_ID++; }

    void Awake()
    {
        gameInfo = new Game_Info();

        InitializeAchievements();

        achievementList = new List<AchievementBase>();
        foreach (GameObject a in achievementPrefabList)
        {
            GameObject clone = Instantiate(a);
            AchievementBase abase = clone.GetComponent<AchievementBase>();
            achievementList.Add(abase);

            if (abase.IsCompletedCheck()) //deactivate it if it is done
                clone.SetActive(false);

        }
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        //Store all the info, like everything(infos) for everything(achievements) here

	}

    
    //the big switch to init all the achievements
    void InitializeAchievements()
    {
    }

    public void AchievementUnlocked(AchievementBase ach)
    {
        //queue up the visual effect of achievement unlock thing
        Debug.Log("Achievement Unlocked: " + ach.ach_name);
    }
}

public enum PREFTYPE
{
    NUM_OF_ENTERGAME, //number of times entering the game
    NUM_OF_KILLS, //number of enemy killed
    NUM_OF_DEATHS, //number of deaths
}
