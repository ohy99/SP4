using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public GameObject player;
    public Vector3[] path;
    private FSMSystem sm;

    public void SetTransition(Transition t) { sm.PerformTransition(t); }

    // Use this for initialization
    void Start () {
        MakeFSM();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        sm.CurrentState.Reason(player, gameObject);
        sm.CurrentState.Act(player, gameObject);
    }

    private void MakeFSM()
    {
        FollowPathState follow = new FollowPathState(path);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasePlayer);

        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowPath);

        sm = new FSMSystem();
        sm.AddState(follow);
        sm.AddState(chase);
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
        const float moveSpeed = 1.0f;
        const float rotateSpeed = 5.0f;

        Vector3 moveDir = waypoints[currentWayPoint] - npc.transform.position;

        if (moveDir.magnitude < 1)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
            {
                currentWayPoint = 0;
            }
        }
        else
        {
            npc.transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(moveDir), rotateSpeed * Time.deltaTime);

            npc.transform.eulerAngles = new Vector3(0, 0, npc.transform.eulerAngles.z);
        }
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) < 5)
        {
            npc.GetComponent<EnemyAI>().SetTransition(Transition.SawPlayer);
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
        const float rotateSpeed = 5.0f;

        Vector3 moveDir = player.transform.position - npc.transform.position;


        npc.transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(moveDir), rotateSpeed * Time.deltaTime);

        npc.transform.eulerAngles = new Vector3(0, 0, npc.transform.eulerAngles.z);
    }

    public override void Reason(GameObject player, GameObject npc)
    {
            if (Vector3.Distance(npc.transform.position, player.transform.position) >= 5)
            {
                npc.GetComponent<EnemyAI>().SetTransition(Transition.LostPlayer);
            }
    }
}