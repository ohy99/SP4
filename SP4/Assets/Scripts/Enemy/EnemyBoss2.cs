using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2 : MonoBehaviour {

    [SerializeField]
    Projectile proj;
    [SerializeField]
    int _projectileLayer = (int)PROJLAYER.ENEMYPROJ;
    [SerializeField]
    float moveSpd = 10;
    [SerializeField]
    public float maxAngle = 30.0f;
    [SerializeField]
    int _numOfProj = 10;
    [SerializeField]
    public float projectileSpd = 10.0f;
    [SerializeField]
    public float shootInterval = 0.1f;
    [SerializeField]
    public float damage = 3.0f;
    [SerializeField]
    float _hp = 500;

    public int numOfProj { get { return _numOfProj; } }
    public int projectileLayer { get { return _projectileLayer; } private set { } }

    FSMSystem sm;
    Health hp;

    public void SetTransition(Transition t) { sm.PerformTransition(t); }

    // Use this for initialization
    void Start () {
        sm = new FSMSystem();
        Debug.Log("Add IdleState");
        BossIdleState bis = new BossIdleState();
        bis.AddTransition(Transition.NearPlayer, StateID.BossAttack);

        Boss2AttackState bas = new Boss2AttackState();
        bas.AttackProjPrefab(proj);
        bas.AddTransition(Transition.LostPlayer, StateID.BossIdle);
        bas.shootInterval = shootInterval;

        sm.AddState(bis);
        sm.AddState(bas);

        //player = Global.Instance.player;

        Rigidbody2D rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.velocity = new Vector3(Random.value, Random.value, 0).normalized * moveSpd;

        Global.Instance.boss = this.gameObject;

        hp = GetComponent<Health>();
        hp.SetHp(_hp);

        EnemyHealthBar ehbscript = GetComponent<EnemyHealthBar>();
        ehbscript.cam = Global.Instance.cam;
    }
	
	// Update is called once per frame
	void Update () {
        if (!Global.Instance.player)
            return;


        sm.CurrentState.Reason(Global.Instance.player, gameObject);
        sm.CurrentState.Act(Global.Instance.player, gameObject);
    }
}

public class Boss2AttackState : FSMState
{
    Projectile proj;
    public double shootInterval = 0.5;
    double shootElapsed = 0.0;
    public float massShootInterval = 2.0f;
    public float massShootElapsed = 0.0f;
    int projShot = 0;
    float aimingAngle = 0.0f;

    public Boss2AttackState()
    {
        stateID = StateID.BossAttack;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        if ((massShootElapsed += Time.deltaTime) >= massShootInterval)
        {
            EnemyBoss2 boss = npc.GetComponent<EnemyBoss2>();
            if ((shootElapsed += Time.deltaTime) >= shootInterval)
            {
                Vector3 bossToPlayer = -boss.transform.position + player.transform.position;
                bossToPlayer = new Vector3(bossToPlayer.x, bossToPlayer.y, 0); //force z = 0;
                aimingAngle = Mathf.Rad2Deg * Mathf.Sin((projShot / (boss.numOfProj - 1)) * (2.0f * Mathf.PI));
                Quaternion quat = Quaternion.Euler(0, 0, aimingAngle);

                Projectile newProj = GameObject.Instantiate(proj, npc.transform.position, Quaternion.identity);
                
                newProj.transform.up = quat * bossToPlayer;
                newProj.gameObject.layer = boss.projectileLayer;
                newProj.projectileSpeed = boss.projectileSpd;
                newProj.SetDamage(boss.damage);

                shootElapsed = 0.0;
                ++projShot;
            }
            if (projShot >= boss.numOfProj)
            {
                projShot = 0;
                massShootElapsed = 0.0f;
            }
        }

    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 10)
        {
            npc.GetComponent<EnemyBoss2>().SetTransition(Transition.LostPlayer);
        }
    }

    public void AttackProjPrefab(Projectile proj)
    {
        this.proj = proj;
    }
}