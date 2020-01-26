using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public BehaviourState CurrentState;
    public float CurrentScore;
    public float LowScoreStreshold;
    public float StopTalkingStreshold;//or this could be half of the treshold itself
    public bool Talking;
    //public bool InMidState;

    //public bool InMidState;

    private void Start()
    {
        CurrentState.StateLogic();
    }

    private void Update()
    {
        if (CurrentState.CheckEndOfState())
        {
            //if is lower than stop threshold
            if (CurrentScore < LowScoreStreshold / 2)
            {
                CurrentState.StopTalking();
                Talking = false;
            }

            if (Talking)
            {                          
                if (CurrentScore < LowScoreStreshold) GoToLowScoreNextState();
                else GoToNextState();

                CurrentState.StateLogic();
            }

            //if the agent was not talking and the score is higher or equal to the threshold, than continue talking from where it left
            if (!Talking && CurrentScore >= LowScoreStreshold / 2)
            { CurrentState.ContinueTalking();
              Talking = true;
            }
        }


        //if (CurrentState.CheckEndOfState())
        ////if (Input.GetKeyDown(KeyCode.Space))
        //{           
        //    if (InMidState)
        //       //if the score is low and the agent is not towards the end of the current state, than it stops talking
        //        if (CurrentScore < LowScoreStreshold) CurrentState.StopTalking();

        //    if (Talking)
        //    {
        //        if (CurrentScore < LowScoreStreshold) GoToLowScoreNextState();
        //        else GoToNextState();

        //        CurrentState.StateLogic();

        //    }
        //    //if the agent was not talking and the score is higher or equal to the threshold, than continue talking from where it left
        //    if (!Talking && CurrentScore >= LowScoreStreshold)
        //        CurrentState.ContinueTalking();
               


        //    //if (CurrentState.Talking)
        //    //    CurrentState.StateLogic();
        //}
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
