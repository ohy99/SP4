using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    // Use this for initialization
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        EnemyBoss bossScript = go.GetComponent<EnemyBoss>();

        go.transform.position += new Vector3(0.01f,0,0);

        //Debug.Log("In IdleStateUpdate");
    }
    public override void Exit()
    {

    }
}
