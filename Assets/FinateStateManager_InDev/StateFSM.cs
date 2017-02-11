using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateFSM {

    public Action enter;
    public Action exit;
    public enumStates fsmState;
   // public event OnStateExit exit;
    Dictionary<enumEvents, enumStates> consequences;

    public StateFSM(enumStates _fsmState)
    {
        consequences = new Dictionary<enumEvents, enumStates>();
        fsmState = _fsmState;
    }

    public StateFSM(enumStates _fsmState, Dictionary<enumEvents, enumStates> _consequences )
    {
        consequences = _consequences;
        fsmState = _fsmState;
    }

    public bool HasConsequence(enumEvents _event)
    {
        if (consequences != null)
        {
            return consequences.ContainsKey(_event);
        }
        return false;
    }

    public enumStates NextState(enumEvents _event)
    {
        return consequences[_event];
    }

    public void AddConsequence(enumEvents _fsmEvent, enumStates _nextFsmState)
    {
        consequences.Add(_fsmEvent, _nextFsmState);
    }
}
