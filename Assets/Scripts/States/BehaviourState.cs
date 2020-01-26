using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourState : MonoBehaviour
{
    public BehaviourState NextState;
    public BehaviourState LowScoreNextState;
    public bool Talking;
    public bool InMidState;

    protected Animator agentAnimator;

    private void Awake()
    {
        agentAnimator = GameObject.Find("AgentFemale").GetComponent<Animator>();
    }
    public virtual void StateLogic()
    {
        //
    }

    public void StopTalking()
    {
        Talking = false;
        Debug.Log("Agent has stopped talking");
        agentAnimator.SetLayerWeight(1, 0.1f);
        //foreach (AnimationState state in agentAnimator)
        //{
        //    state.speed = 0F;
        //}
    }

    public void ContinueTalking()
    {
        Talking = true;
        Debug.Log("Agent has started talking again");
        agentAnimator.SetLayerWeight(1, 1f);

    }


}
