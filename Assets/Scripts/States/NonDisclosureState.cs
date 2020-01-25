using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDisclosureState : BehaviourState
{
    public string DebugMessage;


    public override void StateLogic()
    {
        Debug.Log(DebugMessage);

        base.StateLogic();
    }
}
