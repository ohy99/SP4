﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour {

    [SerializeField]
    Projectile proj;
    [SerializeField]
    int _projectileLayer = (int)PROJLAYER.ENEMYPROJ;
    [SerializeField]
    float flySpd = 10;
    [SerializeField]
    int _numOfProj = 10;
    [SerializeField]
    public float projectileSpd = 10.0f;
    [SerializeField]
    public float shootInterval = 0.5f;
    [SerializeField]
    public float damage = 5.0f;
    [SerializeField]
    float _hp = 600;

    public int numOfProj { get { return _numOfProj; } }
    public int projectileLayer { get { return _projectileLayer; } private set { } }
    //GameObject player;
    
    FSMSystem sm;
    Health hp;

    public void SetTransition(Transition t) { sm.PerformTransition(t); }

    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () {
        sm = new FSMSystem();
        Debug.Log("Add IdleState");
        BossIdleState bis = new BossIdleState();
        bis.AddTransition(Transition.NearPlayer, StateID.BossAttack);

        BossAttackState bas = new BossAttackState();
        bas.AttackProjPrefab(proj);
        bas.AddTransition(Transition.LostPlayer, StateID.BossIdle);
        bas.shootInterval = shootInterval;

        sm.AddState(bis);
        sm.AddState(bas);

        //player = Global.Instance.player;

        Rigidbody2D rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.velocity = new Vector3(Random.value, Random.value, 0).normalized * flySpd;

        Global.Instance.boss = this.gameObject;

        hp = GetComponent<Health>();
        hp.SetHp(_hp);

        EnemyHealthBar ehbscript = GetComponent<EnemyHealthBar>();
        ehbscript.cam = Global.Instance.cam;
    }
	
	// Update is called once per frame
	void Update () {

        if(!Global.Instance.player)
            return;
        

        sm.CurrentState.Reason(Global.Instance.player, gameObject);
        sm.CurrentState.Act(Global.Instance.player, gameObject);
    }
}

public class BossIdleState : FSMState
{
    public BossIdleState()
    {
        stateID = StateID.BossIdle;
    }

    public override void Act(GameObject player, GameObject npc)
    {
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) < 20)
        {
            if (npc.GetComponent<EnemyBoss>())
                npc.GetComponent<EnemyBoss>().SetTransition(Transition.NearPlayer);
            else if (npc.GetComponent<EnemyBoss2>())
                npc.GetComponent<EnemyBoss2>().SetTransition(Transition.NearPlayer);
        }
    }
}

public class BossAttackState : FSMState
{
    Projectile proj;
    public double shootInterval = 0.5;
    double shootElapsed = 0.0;

    public BossAttackState()
    {
        stateID = StateID.BossAttack;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        if ((shootElapsed += Time.deltaTime) >= shootInterval)
        {
            EnemyBoss boss = npc.GetComponent<EnemyBoss>();

            //Vector3 upDir = npc.transform.up;

            float rotateAngle = 180.0f / (float)boss.numOfProj; 
            for (int i = 0; i < boss.numOfProj; ++i)
            {
                Quaternion quat = Quaternion.Euler(0, 0, rotateAngle * i);
                //quat.SetFromToRotation(new Vector3(0, 1, 0), upDir);

                Projectile newProj = GameObject.Instantiate(proj, npc.transform.position, quat);
                newProj.transform.up = quat * newProj.transform.up;
                newProj.gameObject.layer = boss.projectileLayer;
                newProj.projectileSpeed = boss.projectileSpd;
                newProj.SetDamage(boss.damage);
            }

            shootElapsed = 0.0;
        }
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 10)
        {
            npc.GetComponent<EnemyBoss>().SetTransition(Transition.LostPlayer);
        }
    }

    public void AttackProjPrefab(Projectile proj)
    {
        this.proj = proj;
    }
}