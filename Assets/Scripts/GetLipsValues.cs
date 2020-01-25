using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Lip;

public class GetLipsValues : MonoBehaviour
{
    // Start is called before the first frame update

    private Dictionary<LipShape, float> LipWeightings;
    private string LipsDataToPrint = "";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SRanipal_Lip.GetLipWeightings(out LipWeightings);
        Debug.Log("Keys: " + LipWeightings.Keys+ " Val: " + LipWeightings.Values);
        
        foreach (KeyValuePair<LipShape, float> kvp in LipWeightings)
        {
            //cartof
            LipsDataToPrint += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }
        Debug.Log(LipsDataToPrint);
        LipsDataToPrint = "";

    }
}
