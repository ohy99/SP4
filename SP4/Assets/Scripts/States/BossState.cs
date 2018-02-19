using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public ChaseState(GameObject go , StateMachine sm) : base(go, sm)
    {
    }

    public override void Exit()
    {
    }

    public override void Start()
    {
    }

    public override void Update()
    {
    }
}

public class AttackState : State
{
    Projectile proj;
    double shootInterval = 0.5;
    double shootElapsed = 0.0;

    public AttackState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Exit()
    {
    }

    public override void Start()
    {
    }

    public override void Update()
    {
        if ((shootElapsed += Time.deltaTime) >= shootInterval)
        {
            Vector3 upDir = go.transform.up;
            Quaternion quat = new Quaternion();
            quat.SetFromToRotation(new Vector3(0, 1, 0), upDir);
            Projectile newProj = GameObject.Instantiate(proj, go.transform.position, quat);


            shootElapsed = 0.0;
        }
    }

    public void AttackProjPrefab(Projectile proj)
    {
        this.proj = proj;
    }
}
