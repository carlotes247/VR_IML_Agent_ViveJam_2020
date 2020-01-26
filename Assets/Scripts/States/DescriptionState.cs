using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionState : BehaviourState
{
    public string DebugMessage;


    public override void StateLogic()
    {
        maxTime = 20;
        Debug.Log(DebugMessage);
        agentAnimator.SetTrigger("02Description");       
        base.StateLogic();
    }
}
