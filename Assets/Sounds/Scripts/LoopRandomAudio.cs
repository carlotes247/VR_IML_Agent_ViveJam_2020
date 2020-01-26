 using UnityEngine;
 using System.Collections;
 
 public class LoopRandomAudio : MonoBehaviour {
 
     public AudioSource randomSound;
     public string resourceFolderName;
     public int minDur;
     public int maxDur;
     public AudioClip[] audioSources;
 
     // Use this for initialization
     void Start () {
				
		// Create an array
		Debug.Log(resourceFolderName);
	     audioSources =  Resources.LoadAll<AudioClip>(resourceFolderName);
         CallAudio ();
     }
 
 
     private void CallAudio()
     {
         Invoke ("PlayRandomSound", Random.Range(minDur,maxDur));
     }
 
     private void PlayRandomSound()
     {
         randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
         randomSound.Play();
         CallAudio ();
     }
 }