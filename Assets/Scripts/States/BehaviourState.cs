using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourState : MonoBehaviour
{
    public BehaviourState NextState;
    public BehaviourState LowScoreNextState;
    public string AnimationClipName;
    public float timer;
    public float maxTime;

    protected Animator agentAnimator;

    private void Awake()
    {
        agentAnimator = GameObject.Find("AgentFemale").GetComponent<Animator>();
        maxTime = 10;//default is 10
    }
    public virtual void StateLogic()
    {
        //CheckEndOfState();
        timer = 0;
        //int seconds = timer % 60;
    }

    public void StopTalking()
    {
        //Talking = false;
        //Debug.Log("Agent has stopped talking");
        agentAnimator.SetLayerWeight(1, 0.1f);

    }

    public void ContinueTalking()
    {
        //Talking = true;
        //Debug.Log("Agent has started talking again");
        agentAnimator.SetLayerWeight(1, 1f);

    }


}
