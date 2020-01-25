using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public BehaviourState CurrentState;
    public float CurrentScore;
    public float LowScoreStreshold;

    //public bool InMidState;

    private void Start()
    {
        CurrentState.StateLogic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {           
            if (CurrentState.InMidState)
            {   //if the score is low and the agent is not towards the end of the current state, than it stops talking
                if (CurrentScore < LowScoreStreshold) CurrentState.StopTalking();
                //if the agent was not talking and the score is higher or equal to the threshold, than continue talking from where it left
                if (!CurrentState.Talking && CurrentScore >= LowScoreStreshold) CurrentState.ContinueTalking();
            }else
            {
                //just making sure the talking is true
                CurrentState.Talking = true;
                if (CurrentScore < LowScoreStreshold) GoToLowScoreNextState();
                else GoToNextState();
            }

            CurrentState.StateLogic();
        }
    }

    public void GoToNextState()
    {
        if(CurrentState.NextState != null)
        {
            CurrentState = CurrentState.NextState;
        }
    }


    public void GoToLowScoreNextState()
    {
        if (CurrentState.LowScoreNextState != null)
        {
            CurrentState = CurrentState.LowScoreNextState;
        }
    }
}
