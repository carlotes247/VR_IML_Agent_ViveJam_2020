﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughterState : BehaviourState
{
    public string DebugMessage;


    public override void StateLogic()
    {
        Debug.Log(DebugMessage);
        agentAnimator.SetTrigger("03Laughter");
        base.StateLogic();
    }
}
