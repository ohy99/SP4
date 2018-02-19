using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour {

    [SerializeField]
    Projectile proj;
    GameObject player;

    FSMSystem sm;

    public void SetTransition(Transition t) { sm.PerformTransition(t); }

    void Awake()
    {
        sm = new FSMSystem();
        Debug.Log("Add IdleState");
        BossIdleState bis = new BossIdleState();
        bis.AddTransition(Transition.NearPlayer, StateID.BossAttack);

        BossAttackState bas = new BossAttackState();
        bas.AttackProjPrefab(proj);
        bas.AddTransition(Transition.LostPlayer, StateID.BossIdle);

        sm.AddState(bis);
        sm.AddState(bas);

        player = Global.Instance.player;
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        sm.CurrentState.Reason(player, gameObject);
        sm.CurrentState.Act(player, gameObject);
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
        if (Vector3.Distance(npc.transform.position, player.transform.position) < 5)
        {
            npc.GetComponent<EnemyBoss>().SetTransition(Transition.NearPlayer);
        }
    }
}

public class BossAttackState : FSMState
{
    Projectile proj;
    double shootInterval = 0.5;
    double shootElapsed = 0.0;

    public BossAttackState()
    {
        stateID = StateID.BossAttack;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        if ((shootElapsed += Time.deltaTime) >= shootInterval)
        {
            Vector3 upDir = npc.transform.up;
            Quaternion quat = new Quaternion();
            quat.SetFromToRotation(new Vector3(0, 1, 0), upDir);
            Projectile newProj = GameObject.Instantiate(proj, npc.transform.position, quat);


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