using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamScript : MonoBehaviour {

    [SerializeField]
    float offset = 5;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject playerIcon;
    [SerializeField]
    GameObject bossIcon;

    [SerializeField]
    float zPosOffset = -60;
    [System.Serializable]
    class IconInfo
    {
        public float iconPosZOffset = -50;
        public float scale = 2;
    }
    [SerializeField]
    IconInfo iconInfo;

    //object, then icon
    Dictionary<GameObject, GameObject> iconMap;

    void Awake()
    {
        iconMap = new Dictionary<GameObject, GameObject>();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float w = 0;
        float h = 0;
        Vector3 outPos = RoomGenerator.Instance.GetMidPoint(out w, out h);
        cam.transform.position = new Vector3(outPos.x, outPos.y, zPosOffset);
        cam.orthographicSize = (w > h? w * 0.5f: h * 0.5f) + offset;
        

        if (Global.Instance.player)
        {
            GameObject player = Global.Instance.player;
            if (!iconMap.ContainsKey(player))
            {
                CreateIcon(player, playerIcon);
            }
        }
        GameObject boss = Global.Instance.boss;
        if (boss && !iconMap.ContainsKey(boss))
        {
            CreateIcon(boss, bossIcon);
        }

        foreach (KeyValuePair<GameObject, GameObject> pair in iconMap)
        {
            Transform objTransform = pair.Key.GetComponent<Transform>();
            Transform spriteTransform = pair.Value.GetComponent<Transform>();
            spriteTransform.position = new Vector3(objTransform.position.x, objTransform.position.y, iconInfo.iconPosZOffset);
            //spriteTransform.localScale = new Vector3(w * 0.1f, h * 0.1f);
        }
	}

    void CreateIcon(GameObject obj, GameObject iconImage)
    {
        GameObject sprite2D = Instantiate(iconImage);
        Transform spriteTransform = sprite2D.GetComponent<Transform>();
        Transform objTransform = obj.GetComponent<Transform>();
        spriteTransform.position = new Vector3(objTransform.position.x, objTransform.position.y, iconInfo.iconPosZOffset);
        spriteTransform.localScale = new Vector3(iconInfo.scale, iconInfo.scale);
        iconMap.Add(obj, sprite2D);
    }
}
