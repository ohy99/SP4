using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour {

    [SerializeField]
    Projectile proj;


    StateMachine sm;

    void Awake()
    {
        sm = GetComponent<StateMachine>();

        Debug.Log("Add IdleState");
        AttackState attackState = new AttackState(gameObject, sm);
        attackState.AttackProjPrefab(proj);
        sm.AddState(attackState);
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
