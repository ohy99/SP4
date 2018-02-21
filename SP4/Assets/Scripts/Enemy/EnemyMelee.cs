using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour {

    public GameObject player;
    private Vector3[] path;
    private FSMSystem sm;
    private Health hpScript;

    public void SetTransition(Transition t) { sm.PerformTransition(t); }

    // Use this for initialization
    void Start () {
        path = new [] { new Vector3(transform.position.x - 3.0f, transform.position.y + 2.5f), new Vector3(transform.position.x - 3.0f, transform.position.y - 2.5f), new Vector3(transform.position.x + 3.0f, transform.position.y - 2.5f), new Vector3(transform.position.x + 3.0f, transform.position.y + 2.5f) };

        hpScript = GetComponent<Health>();

        hpScript.SetHp(10.0f);

        player = Global.Instance.player;

        MakeFSM();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
            return;

        sm.CurrentState.Reason(player, gameObject);
        sm.CurrentState.Act(player, gameObject);
    }

    private void MakeFSM()
    {
        FollowPathState follow = new FollowPathState(path);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasePlayer);

        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowPath);
        chase.AddTransition(Transition.NearPlayer, StateID.AttackPlayer);

        AttackPlayerState attack = new AttackPlayerState();
        attack.AddTransition(Transition.SawPlayer, StateID.ChasePlayer);

        sm = new FSMSystem();
        sm.AddState(follow);
        sm.AddState(chase);
        sm.AddState(attack);
    }
}

public class FollowPathState : FSMState
{
    private int currentWayPoint;
    private Vector3[] waypoints;

    public FollowPathState(Vector3[] wp)
    {
        waypoints = wp;
        currentWayPoint = 0;
        stateID = StateID.FollowPath;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        const float moveSpeed = 0.5f;
        const float rotateSpeed = 0.5f;

        Vector2 vel = npc.GetComponent<Rigidbody2D>().velocity;

        Vector3 moveDir = waypoints[currentWayPoint] - npc.transform.position;

        Vector2 moveUp = new Vector2(moveDir.x, moveDir.y);

        if (moveDir.magnitude < 0.1)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
            {
                currentWayPoint = 0;
            }
        }
        else
        {
            vel = moveUp.normalized * moveSpeed;

            npc.transform.up = Vector2.Lerp(npc.transform.up, moveUp, rotateSpeed * Time.deltaTime);

            npc.transform.eulerAngles = new Vector3(0, 0, npc.transform.eulerAngles.z);
        }

        npc.GetComponent<Rigidbody2D>().velocity = vel;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) < 5)
        {
            npc.GetComponent<EnemyMelee>().SetTransition(Transition.SawPlayer);
        }
    }
}

public class ChasePlayerState : FSMState
{
    public ChasePlayerState()
    {
        stateID = StateID.ChasePlayer;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        const float moveSpeed = 1.0f;
        const float rotateSpeed = 0.5f;

        Vector3 moveDir = player.transform.position - npc.transform.position;

        Vector2 moveUp = new Vector2(moveDir.x, moveDir.y);

        Vector2 vel = moveUp.normalized * moveSpeed;

        npc.transform.up = Vector2.Lerp(npc.transform.up, moveUp, rotateSpeed * Time.deltaTime);

        npc.transform.eulerAngles = new Vector3(0, 0, npc.transform.eulerAngles.z);

        npc.GetComponent<Rigidbody2D>().velocity = vel;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 5)
        {
            npc.GetComponent<EnemyMelee>().SetTransition(Transition.LostPlayer);
        }
        else if (Vector3.Distance(npc.transform.position, player.transform.position) < 2)
        {
            npc.GetComponent<EnemyMelee>().SetTransition(Transition.NearPlayer);
        }
    }
}

public class AttackPlayerState : FSMState
{
    const float rotateSpeed = 0.5f;
    private float damageValue = -10.0f;

    float attackDelay = 1.0f;
    float elapsedTime;

    public AttackPlayerState()
    {
        stateID = StateID.AttackPlayer;
    }

    public override void OnEnter()
    {
        elapsedTime = 0.0f;
    }

    public override void Act(GameObject player, GameObject npc)
    {

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= attackDelay)
        {
            player.SendMessage("ModifyHp", damageValue);
            elapsedTime = 0.0f;
        }

        Vector3 moveDir = player.transform.position - npc.transform.position;

        Vector2 moveUp = new Vector2(moveDir.x, moveDir.y);

        npc.transform.up = Vector2.Lerp(npc.transform.up, moveUp, rotateSpeed * Time.deltaTime);
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 2)
        {
            npc.GetComponent<EnemyMelee>().SetTransition(Transition.SawPlayer);
        }
    }
}