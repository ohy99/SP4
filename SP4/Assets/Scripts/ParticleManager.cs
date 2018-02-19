using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager> {

    public enum PARTICLETYPE
    {
        HITENEMY,
        HITPLAYER,

        PT_COUNT //end
    }
    public PARTICLETYPE enumType;
    [SerializeField]
    List<ParticleSystem> psList = new List<ParticleSystem>();

    List<ParticleSystem> activePSList = new List<ParticleSystem>();

    //testing
    float et = 0.0f;


	// Use this for initialization
	void Start() {
		
	}
    void Awake() {
    }
	
	// Update is called once per frame
	void Update () {
        
        //et += Time.deltaTime;
        //if (et > 2.0f)
        //{
        //    GenerateParticle(PARTICLETYPE.HITENEMY, new Vector3(0, 0, 0));
        //    et = 0.0f;
        //}
	}

    public void GenerateParticle(PARTICLETYPE type, Vector3 pos)
    {
        ParticleSystem ps = Instantiate(psList[(int)type], pos, Quaternion.identity);
        //activePSList.Add(ps);
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
