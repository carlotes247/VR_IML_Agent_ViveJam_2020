using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourState : MonoBehaviour
{
    public BehaviourState NextState;
    public BehaviourState LowScoreNextState;
    public bool Talking;
    public bool InMidState;

    public virtual void StateLogic()
    {
        //
    }

    public void StopTalking()
    {
        Talking = false;
        Debug.Log("Agent has stopped talking");
    }

    public void ContinueTalking()
    {
        Talking = true;
        Debug.Log("Agent has started talking again");
    }


}
