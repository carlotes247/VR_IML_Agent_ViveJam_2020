using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionState : BehaviourState
{
    public string DebugMessage;
    public Animator PramAnimator;
    public AudioClip secondClip;


    public override void StateLogic()
    {
        maxTime = 20;
        Debug.Log(DebugMessage);
        agentAnimator.SetTrigger("02Description");
        if (PramAnimator != null)
        {
            PramAnimator.SetTrigger("StartPram");
        }

        base.StateLogic();
    }
}
