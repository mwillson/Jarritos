using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

    public float GrowAmountOnHit;
    public float SecondsToGrow;
    public GameObject ElectroObject;

    public IEnumerator OnTargetHit()
    {
        if (ElectroObject.GetComponent<ElectroFieldScript>())
        {
            ElectroObject.GetComponent<ElectroFieldScript>().ScaleRate = GrowAmountOnHit;
            yield return new WaitForSeconds(SecondsToGrow);
            ElectroObject.GetComponent<ElectroFieldScript>().ScaleRate = -ElectroObject.GetComponent<ElectroFieldScript>().ShrinkRate;
        }
    }

	// Use this for initialization
	void Start ()
    {
        ElectroObject = GameObject.Find("ElectroField");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
