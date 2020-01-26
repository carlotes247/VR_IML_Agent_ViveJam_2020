﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseState : BehaviourState
{
    public string DebugMessage;


    public override void StateLogic()
    {
        Debug.Log(DebugMessage); 
        agentAnimator.SetTrigger("01Surprise");

        base.StateLogic();
    }
}
