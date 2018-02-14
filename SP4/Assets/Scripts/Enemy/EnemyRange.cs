using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{

    public GameObject player;
    private FSMSystem sm;

    public void SetTransition(Transition t) { sm.PerformTransition(t); }

    // Use this for initialization
    void Start()
    {
        MakeFSM();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sm.CurrentState.Reason(player, gameObject);
        sm.CurrentState.Act(player, gameObject);
    }

    private void MakeFSM()
    {
        Vector3 target = new Vector3(Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f), Random.Range(transform.position.y - 5.0f, transform.position.y + 5.0f));
        RoamingState follow = new RoamingState(target);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasePlayer);

        FollowPlayerState chase = new FollowPlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowPath);
        chase.AddTransition(Transition.NearPlayer, StateID.AttackPlayer);

        ShootPlayerState attack = new ShootPlayerState();
        attack.AddTransition(Transition.SawPlayer, StateID.ChasePlayer);

        sm = new FSMSystem();
        sm.AddState(follow);
        sm.AddState(chase);
        sm.AddState(attack);
    }
}

public class RoamingState : FSMState
{
    private Vector3 target;

    public RoamingState(Vector3 target)
    {
        this.target = target;
        stateID = StateID.FollowPath;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        const float moveSpeed = 0.5f;
        const float rotateSpeed = 0.5f;

        Vector3 moveDir = target - npc.transform.position;

        Vector2 moveUp = new Vector2(moveDir.x, moveDir.y);

        if (moveDir.magnitude < 0.1)
        {
            target = new Vector3(Random.Range(npc.transform.position.x - 5.0f, npc.transform.position.x + 5.0f), Random.Range(npc.transform.position.y - 5.0f, npc.transform.position.y + 5.0f));
        }
        else
        {
            npc.transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

            npc.transform.up = Vector2.Lerp(npc.transform.up, moveUp, rotateSpeed * Time.deltaTime);

            npc.transform.eulerAngles = new Vector3(0, 0, npc.transform.eulerAngles.z);
        }
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) < 10)
        {
            npc.GetComponent<EnemyRange>().SetTransition(Transition.SawPlayer);
        }
    }
}

public class FollowPlayerState : FSMState
{
    public FollowPlayerState()
    {
        stateID = StateID.ChasePlayer;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        const float moveSpeed = 1.0f;
        const float rotateSpeed = 0.5f;

        Vector3 moveDir = player.transform.position - npc.transform.position;

        Vector2 moveUp = new Vector2(moveDir.x, moveDir.y);

        npc.transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

        npc.transform.up = Vector2.Lerp(npc.transform.up, moveUp, rotateSpeed * Time.deltaTime);

        npc.transform.eulerAngles = new Vector3(0, 0, npc.transform.eulerAngles.z);
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 10)
        {
            npc.GetComponent<EnemyRange>().SetTransition(Transition.LostPlayer);
        }
        else if (Vector3.Distance(npc.transform.position, player.transform.position) < 5)
        {
            npc.GetComponent<EnemyRange>().SetTransition(Transition.NearPlayer);
        }
    }
}

public class ShootPlayerState : FSMState
{
    const float rotateSpeed = 0.5f;
    float elapsedTime;
    float shootDelay = 1.5f;

    public ShootPlayerState()
    {
        stateID = StateID.AttackPlayer;
    }

    public override void OnEnter()
    {
        elapsedTime = shootDelay;
    }

    public override void Act(GameObject player, GameObject npc)
    {

        elapsedTime += Time.deltaTime;

        Vector3 moveDir = player.transform.position - npc.transform.position;

        Vector2 moveUp = new Vector2(moveDir.x, moveDir.y);

        if (elapsedTime >= shootDelay)
        {
            GameObject go = npc.transform.GetChild(0).gameObject;
            Debug.Log(go);
            go.GetComponent<RangeWeaponBase>().Discharge(go.transform.GetChild(0).position, go.transform.GetChild(0).rotation);
            elapsedTime = 0.0f;
        }

        npc.transform.up = Vector2.Lerp(npc.transform.up, moveUp, rotateSpeed * Time.deltaTime);
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 5)
        {
            npc.GetComponent<EnemyRange>().SetTransition(Transition.SawPlayer);
        }
    }
}