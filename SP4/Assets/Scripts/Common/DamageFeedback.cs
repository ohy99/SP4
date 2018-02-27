using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedback : Singleton<DamageFeedback> {

    [SerializeField]
    GameObject SpriteText;

    [SerializeField]
    float duration = 0.5f;
    [SerializeField]
    float moveSpd = 2.0f;
    [SerializeField]
    float zOffset = -2.0f;
    Vector3 moveDir = new Vector3(0, 1, 0);

    class DMGINFO
    {
        public float elapsed;
        public float value;
        public GameObject spriteGO;
    }
    List<DMGINFO> dmginfos;

	// Use this for initialization
	void Start () {

    }
    void Awake() {
        dmginfos = new List<DMGINFO>();
        SpriteText = (GameObject)Resources.Load("Feedback/DmgNumber");
    }
	
	// Update is called once per frame
	void Update () {

        List<DMGINFO> removeList = new List<DMGINFO>();
        foreach (DMGINFO df in dmginfos)
        {
            df.elapsed += Time.deltaTime;
            if (df.elapsed > duration)
            {
                Destroy(df.spriteGO);
                removeList.Add(df);
                continue;
            }

            df.spriteGO.transform.position += moveDir * Time.deltaTime * moveSpd;
        }

        //Remove them
        foreach (DMGINFO remove in removeList)
            dmginfos.Remove(remove);
        removeList.Clear();
	}

    public void ShowDamage(float damage, Vector3 pos)
    {
        DMGINFO df = new DMGINFO();
        df.elapsed = 0.0f;
        df.value = damage;
        df.spriteGO = Instantiate(SpriteText, new Vector3(pos.x, pos.y, zOffset), Quaternion.identity);

        TextMesh textmesh = df.spriteGO.transform.GetChild(0).GetComponent<TextMesh>();
        textmesh.text = damage.ToString("#.##");//up to 2 dp

        dmginfos.Add(df);
    }
}
