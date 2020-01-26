using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourState : MonoBehaviour
{
    public BehaviourState NextState;
    public BehaviourState LowScoreNextState;
    public bool EndOfCurrentState;
    public string AnimationClipName;

    protected Animator agentAnimator;

    private void Awake()
    {
        agentAnimator = GameObject.Find("AgentFemale").GetComponent<Animator>();
    }
    public virtual void StateLogic()
    {
       //CheckEndOfState();
    }

    public void StopTalking()
    {
        //Talking = false;
        Debug.Log("Agent has stopped talking");
        agentAnimator.SetLayerWeight(1, 0.1f);

    }

    public void ContinueTalking()
    {
        //Talking = true;
        Debug.Log("Agent has started talking again");
        agentAnimator.SetLayerWeight(1, 1f);

    }
    // checks if it´s the end of the current state. At the end of the state, the clip will go into an idle clip
    // if the current animation clip is named IDLE then the state can be changed. 
    public bool CheckEndOfState()
    {
        string endOfStateName = "idle";
        AnimatorStateInfo currentStateInfo;
        currentStateInfo = agentAnimator.GetCurrentAnimatorStateInfo(1);//the active layer is at index 1 
        //Access the Animation clip name
        Debug.Log("clip name: " + currentStateInfo.IsName(endOfStateName));
        return (currentStateInfo.IsName(endOfStateName));
        //if (currentClipInfo[0].clip.name == endOfStateClipName) EndOfCurrentState = true;
        //else EndOfCurrentState = false;


    }


}
