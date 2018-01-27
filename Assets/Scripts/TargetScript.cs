using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

    //The amount that the ElectroField should grow when this target is hit
    public float GrowAmountOnHit;
    //The duration for which the above grow amount should be applied
    public float SecondsToGrow;

    //Reference to the ElectroField in the game world; defined in Start()
    public GameObject ElectroObject;

    //Called when this target is hit by the ball
    public IEnumerator OnTargetHit()
    {
        //If we can get the ElectroFieldScript component
        if (ElectroObject.GetComponent<ElectroFieldScript>())
        {
            //Temporarily set the ElectroField to grow by GrowAmount
            ElectroObject.GetComponent<ElectroFieldScript>().ScaleRate = GrowAmountOnHit;

            //Set a timer
            yield return new WaitForSeconds(SecondsToGrow);

            //When the timer is up, set the ElectroField to shrink again
            ElectroObject.GetComponent<ElectroFieldScript>().ScaleRate = -ElectroObject.GetComponent<ElectroFieldScript>().ShrinkRate;
        }
    }

	// Use this for initialization
	void Start ()
    {
        //Get the ElectroField from the game world
        ElectroObject = GameObject.Find("ElectroField");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
