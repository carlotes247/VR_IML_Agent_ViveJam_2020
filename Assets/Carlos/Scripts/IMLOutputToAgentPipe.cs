using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractML;

/// <summary>
/// Pipes the output of an IML Controller into the Virtual Agent
/// </summary>
public class IMLOutputToAgentPipe : MonoBehaviour
{
    [Header("ML Component")]
    public IMLComponent MLComponent;

    [Header("Agent Script")]
    public StateManager AgentState;

    [Header("Value Piped")]
    public float ValueToSend;

    // Update is called once per frame
    void Update()
    {
        if (MLComponent)
        {
            // Get first feature from first output from ML component
            ValueToSend = (float) MLComponent.IMLControllerOutputs[0][0];
        }

        if (AgentState)
        {
            // Override current score with value
            AgentState.CurrentScore = ValueToSend;
        }
    }
}
