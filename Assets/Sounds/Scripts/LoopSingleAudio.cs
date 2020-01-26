 using UnityEngine;
 using System.Collections;
 
 public class LoopSingleAudio : MonoBehaviour {
 
     public AudioSource soundSource;
     public int minDur;
     public int maxDur;
 
     // Use this for initialization
     void Start () {
				
		// Create an array
         CallAudio ();
     }
 
 
     private void CallAudio()
     {
         Invoke ("PlayRandomSound", Random.Range(minDur,maxDur));
     }
 
     private void PlayRandomSound()
     {
         soundSource.Play();
         CallAudio ();
     }
 }