using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionState : BehaviourState
{
    public string DebugMessage;


    public override void StateLogic()
    {
        bool a = agentAnimator;
        Debug.Log(DebugMessage + "and " + a);
        agentAnimator.SetTrigger("02Description");       
        base.StateLogic();
    }
}
