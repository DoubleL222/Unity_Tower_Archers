using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class ManagerFSM {
    public static HashSet<StateFSM> AllStates = new HashSet<StateFSM>();
    public static StateFSM currentState;
    public static bool managerInited = false;

    public static void AddAllStates()
    {
        List<enumStates> _AllStates = Enum.GetValues(typeof(enumStates)).Cast<enumStates>().ToList();
        Debug.Log("ALL STATES ARE : " + _AllStates);
        AllStates = new HashSet<StateFSM>();
        foreach (enumStates s in _AllStates)
        {
            StateFSM newState = new StateFSM(s);
            AllStates.Add(newState);
        }
        managerInited = true;
    }
    public static bool InState(enumStates _state)
    {
        return currentState == stateFsmFromEnumState(_state);
    } 

    public static void AddOneState(enumStates _state)
    {
        AllStates.Add(new StateFSM(_state));
    }

    public static void AddOnExitCall(enumStates _state, Action _method)
    {
        stateFsmFromEnumState(_state).exit+=_method;
    }

    public static void AddOnEnterCall(enumStates _state, Action _method)
    {
        stateFsmFromEnumState(_state).enter += _method;
    }

    public static void AddEventConsequence(enumStates _fromState, enumEvents _event, enumStates _toState)
    {
        stateFsmFromEnumState(_fromState).AddConsequence(_event, _toState);
    }
    public static void InvokeEvent(enumEvents _event)
    {
        Debug.Log("FSMLog - invoking event " + _event.ToString());
        if (currentState.HasConsequence(_event))
        {
            if (currentState.exit != null)
                currentState.exit();
            enumStates nextState = currentState.NextState(_event);
            var query = AllStates.Where(a => a.fsmState == nextState);
            currentState = stateFsmFromEnumState(nextState);
            if (currentState.enter != null)
                currentState.enter();
            Debug.Log("FSMLog - Entering state " + currentState.fsmState.ToString());
        }
        else
        {
            Debug.Log("FSMERROR - state "+currentState.fsmState +" has no consequence for the event "+ _event);
        }
       // StateFSM tempState ;
    }

    public static void ForceState(enumStates _state)
    {
        currentState = stateFsmFromEnumState(_state);
    }

    public static StateFSM stateFsmFromEnumState(enumStates _state)
    {
        var query = AllStates.Where(a => a.fsmState == _state);
        if (query.Count() > 0)
        {
            return query.First();
        }
        else
        {
            Debug.Log("FSMError - couldnt find state");
            return null;
        }
    }

}
