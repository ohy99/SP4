using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [SerializeField]
    List<State> states = new List<State>(); //state[0] will always be default state
    State activeState;
    State nextState;

	// Use this for initialization
	void Start () {
        Debug.Log(states.Count);
        if (states.Count > 0)
        {
            activeState = states[0];
            nextState = activeState;

            
        }
	}
	
	// Update is called once per frame
	void Update () {
        CheckStateChange();

        activeState.Update();
    }


    public void SetNextState<T>(T state) where T : State
    {

    }

    void CheckStateChange()
    {

    }

    public void AddState(State state)
    {
        this.states.Add(state);
        Debug.Log("Added State " + state);
    }
}
