using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentIK : MonoBehaviour
{
    protected Animator m_animator;
    [SerializeField] private bool m_ikActive = true;

    public Transform m_lookObj = null;

    public float m_ikWeight = 1.0f;
    private float m_ikWeightCurDest = 1.0f;
    [SerializeField] private float m_ikBlendDuration = 4.5f;

    public AudioSource monologueAudioSource;
    private Vector3 transitionLookAtPosition;

    //usually cage, vanitybox and flowervase
    public GameObject DistanceLookAt1, DistanceLookAt2, DistanceLookAt3;
    public Transform UserHeadCamTransform;
    public int lookAtProgress;
    public bool TransitionLookAt;
    public float TransitionTime;
    public float ChangedLookAtPositionTime;

    public bool IkActive
    {
        get { return m_ikActive; }
        set { m_ikActive = value; }
    }

    public void SetIK(int val)
    {
        if (m_ikWeightCurDest != val)
        {
            m_ikWeightCurDest = val;
            StartCoroutine("UpdateIkWeight", val);
        }
    }

    void Awake()
    {
        TransitionLookAt = false;
        TransitionTime = 3.0f;//time to transition the look at from one game object to another

        lookAtProgress = 1;
        m_ikWeight = 0.9f;
        m_animator = GetComponent<Animator>();
        monologueAudioSource = GameObject.Find("AgentMonologue").GetComponent<AudioSource>();

        SetUserHeadTransform();
        // uncomment this to follow the camera(the user)
        m_lookObj = UserHeadCamTransform;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (m_animator)
        {
            //Debug.Log("Time: " + monologueAudioSource.time);

            //if the IK is active, set the position and rotation directly to the goal. 
            if (m_ikActive)
            {
                // Set the look target position, if one has been assigned
                if (m_lookObj != null)// && !TransitionLookAt)
                {
                    //SetIK(1);
                    m_animator.SetLookAtWeight(m_ikWeight, 0.2f, 0.7f, 1.0f);


                    //Vector3 rndOffset = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                    //m_animator.SetLookAtPosition(m_lookObj.position + rndOffset);

                    //Debug.Log("look at in AnimatorIK");

                    if (!TransitionLookAt)
                    {
                        m_animator.SetLookAtPosition(m_lookObj.position);
                        updateObjLookAt();
                    }
                    else m_animator.SetLookAtPosition(transitionLookAtPosition);
                }
            }
        }
    }

    private void TransitionGaze(Transform fromLookTransform, Transform toLookTransform, float transitionTime = 3.0f)
    {
        TransitionLookAt = true;
        StartCoroutine(TransitionToNewPosition(fromLookTransform, toLookTransform, Time.time, transitionTime));
        m_lookObj = toLookTransform;
    }

    private IEnumerator TransitionToNewPosition(Transform previousLookAtTransform, Transform newLookAtTransform, float changedLookAtTime, float transitionTime)
    {
        //Debug.Log("inCorotine");
        float t = (Time.time - changedLookAtTime) / TransitionTime;
        while (TransitionLookAt)
        {
            transitionLookAtPosition = Vector3.Lerp(previousLookAtTransform.position, newLookAtTransform.position, t);

            yield return new WaitForSeconds(.01f);
            t = (Time.time - changedLookAtTime) / TransitionTime;
            if (t >= 1.0f) TransitionLookAt = false;
        }
        yield return null;
    }


    // Smooth transition between IK active and inactive
    private IEnumerator UpdateIkWeight(float dest)
    {
        m_ikWeight = Mathf.Abs(1 - dest);   // set IK active/inactive end state
        m_ikActive = true;                  // enable IK

        // Lerp between current and end IK states
        for (float i = 0; i < 1f; i = (i + Time.deltaTime) / m_ikBlendDuration)
        {
            m_ikWeight = Mathf.Lerp(m_ikWeight, dest, i);
            yield return null;
        }

        // If the end IK state is inactive, disable IK updating weight and position for animator
        if (dest == 0)
            m_ikActive = false;
    }

    //based on the current state of the monologue, updates where the NPa should look at
    private void updateObjLookAt()
    {

        switch (lookAtProgress)
        {
            case 1:
                if (monologueAudioSource.time > 5f)
                {
                    //check if the game obj is the birdcage, if not, assign it to the birdcage and transition to it
                    if (m_lookObj != DistanceLookAt1.transform)
                    {
                        //transition the gaze from the current obj to the new obj, in this case the birdcage
                        TransitionGaze(m_lookObj.transform, DistanceLookAt1.transform);
                    }

                    if (monologueAudioSource.time > 9f) lookAtProgress += 1; //increase the progress so that it won't come to this branch again
                }
                else
                    m_lookObj = UserHeadCamTransform;

                break;

            case 2:
                if (monologueAudioSource.time > 43f)
                {
                    //check if the game obj is the VanityCase, if not, assign it to the VanityCase and transition to it
                    if (m_lookObj != DistanceLookAt2.transform)
                    {
                        //transition the gaze from the current obj to the new obj, in this case the birdcage
                        TransitionGaze(m_lookObj.transform, DistanceLookAt2.transform);
                    }

                    if (monologueAudioSource.time > 61f) lookAtProgress += 1; //increase the progress so that it won't come to this branch again

                }
                else//this means the agent neets to look at the camera; if it's not, transition to the camera from the obj in case 1:the birdcage
                {
                    if (m_lookObj != UserHeadCamTransform)
                    {
                        TransitionGaze(m_lookObj.transform, UserHeadCamTransform);
                    }

                }

                break;

            case 3:
                if (monologueAudioSource.time > 97f)
                {
                    //check if the game obj is the Poster, if not, assign it to the Poster and transition to it
                    if (m_lookObj != DistanceLookAt3.transform)
                    {
                        TransitionGaze(m_lookObj.transform, DistanceLookAt3.transform);
                    }

                    if (monologueAudioSource.time > 120f)
                    {
                        lookAtProgress += 1; //increase the progress so that it won't come to this branch again

                        if (m_lookObj != UserHeadCamTransform) TransitionGaze(m_lookObj.transform, UserHeadCamTransform);//also change the look at the camera here
                    }

                }
                else
                {
                    if (m_lookObj != UserHeadCamTransform)
                    {
                        TransitionGaze(m_lookObj.transform, UserHeadCamTransform);
                    }
                }

                break;

            case 4:
                if (monologueAudioSource.time > 265f)
                {
                    m_animator.SetTrigger("pointingSelf");
                    lookAtProgress += 1; //increase the progress so that it won't come to this branch again

                    //make sure it looks at the camera
                    if (m_lookObj != UserHeadCamTransform) TransitionGaze(m_lookObj.transform, UserHeadCamTransform);

                }
                break;

            case 5:
                if (monologueAudioSource.time > 285f)
                {
                    m_animator.SetTrigger("pointingOut");
                    lookAtProgress += 1; //increase the progress so that it won't come to this branch again


                }
                break;
            default:
                m_lookObj = UserHeadCamTransform;
                break;
        }
    }


    private void SetUserHeadTransform()
    {
        if (GameObject.Find("DataRec") != null)
        {
            //ManageDataRec dataRecScript = GameObject.Find("DataRec").GetComponent<ManageDataRec>();
            //if (dataRecScript && dataRecScript.Rec)//if the data rec script exists; if not then it's a playback; if it does, and the rec is false, then set it to avatar's head
            //    UserHeadCamTransform = Camera.main.transform;
            //else
            //    UserHeadCamTransform = GameObject.Find("AvatarHead").transform;
        }
        else
            UserHeadCamTransform = Camera.main.transform;


        //ManageDataRec dataRecScript = GameObject.Find("DataRec").GetComponent<ManageDataRec>();
        //if (dataRecScript)
        //    if (dataRecScript.Rec)
        //        UserHeadCamTransform = Camera.main.transform;
        //    else
        //        UserHeadCamTransform = GameObject.Find("AvatarHead").transform;

    }
}
