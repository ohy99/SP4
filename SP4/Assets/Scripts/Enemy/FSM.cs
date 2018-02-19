using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition = 0,
    SawPlayer,
    LostPlayer,
    NearPlayer,
}

public enum StateID
{
    NullStateID = 0,
    ChasePlayer,
    FollowPath,
    AttackPlayer,

    BossIdle,
    BossAttack,
}

public abstract class FSMState {
    // Basically a map
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();

    protected StateID stateID;
    public StateID ID { get { return stateID; } }

    // Adds transition to another state
    public void AddTransition(Transition trans, StateID id)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("Transitioning to a no state fucker. - Giggs 2018");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError("Not real ID like Shishanth");
            return;
        }

        if (map.ContainsKey(trans))
        {
            Debug.LogError("Transition already exists for that state");
            return;
        }

        map.Add(trans, id);
    }

    // Deletes transition to another state
    public void DeleteTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("Delete NULL hue");
            return;
        }

        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("Delete something that doesn't exist? :thinking:");
    }

    // Returns new state for FSM
    public StateID GetOutputState(Transition trans)
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    // Set up State  and called automatically
    public virtual void OnEnter() { }

    // Reset state and called automatically
    public virtual void OnExit() { }

    // NPC is reference to object controlled by this class. Decides if state should transition
    public abstract void Reason(GameObject player, GameObject npc);

    // NPC is reference to object controlled by this class. Controls action of npc in world
    public abstract void Act(GameObject player, GameObject npc);
}

public class FSMSystem
{
    private List<FSMState> states;

    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    // Add State into SM
    public void AddState(FSMState s)
    {
        if (s == null)
        {
            Debug.LogError("Null states are not allowed");
        }

        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        foreach(FSMState state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("State exist already");
                return;
            }
        }
        states.Add(s);
    }

    // Delete State from SM
    public void DeleteState(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("Deleting null state");
            return;
        }

        foreach(FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("Deleting state that doesn't exist");
    }

    // SM switching state
    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("Not real transition");
            return;
        }

        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("Unable to switch to that state as transition doesn't exist");
            return;
        }

        currentStateID = id;
        foreach(FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                currentState.OnExit();

                currentState = state;

                currentState.OnEnter();
                break;
            }
        }
    }
}