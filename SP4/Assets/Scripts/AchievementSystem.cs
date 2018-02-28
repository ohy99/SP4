using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AchievementSystem : MonoBehaviour {


    //Should be heavily using playerpref

    [SerializeField]
    List<GameObject> achievementPrefabList = new List<GameObject>();
    List<AchievementBase> achievementList;
    public List<AchievementBase> GetAchievementList() { return achievementList; }
    //[SerializeField]
    //AchievementBase wad;

    int U_ID = 0;
    int GetUID() { return U_ID++; }

    [SerializeField]
    GameObject achievementPanel;
    [SerializeField]
    Canvas canvas;
    [System.Serializable]
    class PanelInfo
    {
        public float offsetFromTop = 10;
        public float width = 200;
        public float height = 50;
        public float lifeTime = 2.0f;
        public float moveTime = 0.5f;
    }
    [SerializeField]
    PanelInfo panelinfo;

    class ActivePanelInfo
    {
        public GameObject panel;
        public float elapsed;
        public float lifeTime;
        public Vector3 startPos;
        public Vector3 endPos; //relative pos form start/previous guy
        public float moveElasped;
        public float moveTime;
        public float moveSpd;
        public bool hasReached;
        public bool backingOut;
        public ActivePanelInfo(GameObject panel, float lifetime, Vector3 startPos, Vector3 endPos, float moveTime) { this.panel = panel; this.lifeTime = lifetime; this.elapsed = 0.0f;
            this.startPos = startPos; this.endPos = endPos; this.moveTime = moveTime; this.moveElasped = 0.0f; this.hasReached = false; this.backingOut = false;
        }
    }
    List<ActivePanelInfo> activepanels;
    List<ActivePanelInfo> removePanelList = new List<ActivePanelInfo>();

    [SerializeField]
    bool onHacks = false;
    bool hackOned = false;
    float hackelapsed = 0.0f;
    float hackdelay = 0.1f;
    int hackindex = 0;

    void Awake()
    {
        achievementList = new List<AchievementBase>();
        foreach (GameObject a in achievementPrefabList)
        {
            GameObject clone = Instantiate(a, this.transform);
            AchievementBase abase = clone.GetComponent<AchievementBase>();
            abase.aSystem = this;
            achievementList.Add(abase);

            if (abase.IsCompletedCheck()) //deactivate it if it is done
                clone.SetActive(false);

        }
        activepanels = new List<ActivePanelInfo>();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        

        int currIndex = -1;
        Vector3 firstEndReference = new Vector3(0, canvas.GetComponent<RectTransform>().rect.height * 0.5f - panelinfo.height * 0.5f - panelinfo.offsetFromTop);
        Vector3 totalEndReference = new Vector3(0, 0, 0);
        Vector3 spawnStartPos = new Vector3(0, canvas.GetComponent<RectTransform>().rect.height * 0.5f + panelinfo.height * 0.5f);
        //               active panel info
        for (int i = 0; i < activepanels.Count; )
        {
            ActivePanelInfo api = activepanels[i];
            //use move spd

            //api.moveElasped = Mathf.Min(api.moveElasped + Time.deltaTime, api.moveTime);
            //api.panel.transform.localPosition = Vector3.Lerp(api.startPos, firstEndReference + totalEndReference + api.endPos, api.moveElasped / api.moveTime);
            Vector3 dir = (-api.panel.transform.localPosition + (firstEndReference + totalEndReference + api.endPos)).normalized;
            Vector3 movement = (dir * api.moveSpd * Time.deltaTime);
            float distance = movement.magnitude;
            if (distance >= (-api.panel.transform.localPosition + (firstEndReference + totalEndReference + api.endPos)).magnitude)
                api.panel.transform.localPosition = firstEndReference + totalEndReference + api.endPos;
            else
                api.panel.transform.localPosition += movement;

            if (api.panel.transform.localPosition == firstEndReference + totalEndReference + api.endPos)
                api.hasReached = true;

            if (api.hasReached)
            {
                api.elapsed = Mathf.Min(api.elapsed + Time.deltaTime, api.lifeTime);
                if (api.elapsed >= api.lifeTime)
                {
                    if (i < activepanels.Count - 1)
                        activepanels[i + 1].endPos = api.endPos; //child/nextguy end pos is my now end pos
                    api.backingOut = true;
                    api.startPos = firstEndReference + totalEndReference + api.endPos;
                    api.endPos = spawnStartPos;
                    api.moveElasped = 0.0f;
                    activepanels.Remove(api);
                    removePanelList.Add(api);
                    continue;
                }
            }

            totalEndReference += api.endPos;
            ++i;
        }

        List<ActivePanelInfo> deletelist = new List<ActivePanelInfo>();
        //Going back to top and delete
        foreach (ActivePanelInfo api in removePanelList)
        {
            Vector3 dir = (-api.panel.transform.localPosition + (firstEndReference + totalEndReference + api.endPos)).normalized;
            Vector3 movement = (dir * api.moveSpd * Time.deltaTime);
            float distance = movement.magnitude;
            if (distance >= (-api.panel.transform.localPosition + (firstEndReference + totalEndReference + api.endPos)).magnitude)
                api.panel.transform.localPosition = firstEndReference + totalEndReference + api.endPos;
            else
                api.panel.transform.localPosition += movement;

            //api.moveElasped = Mathf.Min(api.moveElasped + Time.deltaTime, api.moveTime);
            //api.panel.transform.localPosition = Vector3.Lerp(api.startPos, api.endPos, api.moveElasped / api.moveTime);
            if (api.panel.transform.localPosition == firstEndReference + totalEndReference + api.endPos)
            {
                deletelist.Add(api);
            }
        }
        foreach(ActivePanelInfo dapi in deletelist)
        {
            removePanelList.Remove(dapi);
            Destroy(dapi.panel);
        }
        deletelist.Clear();

        if (onHacks)
        {
            
            if (Input.GetKeyUp(KeyCode.Space))
            {
               // hackOned = true;
                hackindex = 0;
                AchievementUnlocked(achievementList[hackindex]);
            }

            if (hackOned)
            {
                hackelapsed += Time.deltaTime;
                if (hackelapsed >= hackdelay)
                {
                    if (hackindex < achievementList.Count)
                    {
                        if (!achievementList[hackindex].completed)
                            AchievementUnlocked(achievementList[hackindex]);
                        ++hackindex;
                    }
                    else
                        hackOned = false;
                    hackelapsed = 0.0f;
                }
            }
        }
	}

    public void AchievementUnlocked(AchievementBase ach)
    {
        //queue up the visual effect of achievement unlock thing
        TriggerAchievementAnimation(ach);
        Debug.Log("Achievement Unlocked: " + ach.ach_name);
    }

    void TriggerAchievementAnimation(AchievementBase ach)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float midX = canvasRect.rect.width * 0.5f;
        float height = canvasRect.rect.height;
        GameObject panel = Instantiate(achievementPanel);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        //Set Parent
        panel.transform.SetParent(canvas.transform, false);
        //Set Scale
        float dx = canvasRect.rect.width - panelinfo.width;
        float dy = height - panelinfo.height;
        panelRect.sizeDelta = new Vector2(-dx, -dy);

        //Set Pos
        Vector3 endPos; //= new Vector3(0, height * 0.5f - panelinfo.height * 0.5f - panelinfo.offsetFromTop - panelinfo.height * activepanels.Count);
        Vector3 firstEndReference = new Vector3(0, height * 0.5f - panelinfo.height * 0.5f - panelinfo.offsetFromTop);
        if (activepanels.Count <= 0)  endPos = new Vector3(0,0,0);
        else endPos = new Vector3(0, - panelinfo.height);
        Vector3 startPos = new Vector3(0, height * 0.5f + panelinfo.height * 0.5f);
        panelRect.localPosition = startPos;
        float moveSpd = (firstEndReference - startPos).magnitude / panelinfo.moveTime;

        //Init values/text
        AchievementPanel panelScript = panel.GetComponent<AchievementPanel>();
        panelScript.Init(ach.ach_name, ach.description);

        ActivePanelInfo api = new ActivePanelInfo(panel, panelinfo.lifeTime, startPos, endPos, panelinfo.moveTime);
        api.moveSpd = moveSpd;
        activepanels.Add(api);
    }

    //new void OnDestroy()
    //{
    //    PlayerPrefs.DeleteAll();
    //}
}

public enum PREFTYPE
{
    NUM_OF_ENTERGAME, //number of times entering the game
    NUM_OF_KILLS, //number of enemy killed
    NUM_OF_DEATHS, //number of deaths
}
