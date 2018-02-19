using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {

    protected GameObject go;
    protected StateMachine stateMachine;

    protected State(GameObject go, StateMachine sm)
    {
        this.go = go;
        this.stateMachine = sm;
    }

    // Use this for initialization
    abstract public void Start();

    // Update is called once per frame
    abstract public void Update();

    abstract public void Exit();

    public State AttachStateMachine(StateMachine sm)
    {
        stateMachine = sm;
        return this;
    }
    public State AttachGameObject(GameObject go)
    {
        this.go = go;
        return this;
    }

}
