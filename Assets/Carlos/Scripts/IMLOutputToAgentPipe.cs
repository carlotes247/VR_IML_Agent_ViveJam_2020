using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractML;

/// <summary>
/// Simple containing of gesture verbs
/// </summary>
[System.Serializable]
public class GestureVerb
{
    public string GestureName;
    public float GestureExpectedValue;
    public int GestureIMLOutputIndex;
}

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

    [Header("Gesture Vocabulary")]
    public List<GestureVerb> GestureVocab;

    [Header("Reward for Agent")]
    public float RewardAgent;

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
            foreach (var gesture in GestureVocab)
            {
                // Get current value from model
                float valueFromMLModel = (float)MLComponent.IMLControllerOutputs[gesture.GestureIMLOutputIndex][0];
                // If the value is the same as the one in the model
                if (ReusableMethods.Floats.nearlyEqual(gesture.GestureExpectedValue, valueFromMLModel, 0.05f))
                {
                    // Add reward to agent
                    AgentState.CurrentScore += RewardAgent;
                }
                // If not, then punish score
                else
                {
                    if (AgentState.CurrentScore < 0f)
                    {
                        AgentState.CurrentScore = 0f;
                    }
                    else
                    {
                        AgentState.CurrentScore -= RewardAgent / 4;

                    }
                }
            }
            
            // Override current score with value
            //AgentState.CurrentScore = ValueToSend;
        }
    }

}
