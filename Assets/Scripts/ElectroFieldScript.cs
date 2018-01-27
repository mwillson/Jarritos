using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroFieldScript : MonoBehaviour {

    //The scale rate continuously applied in Update, manipulated by the Target objects
    public float ScaleRate;

    //The rate at which the field should shrink, applied to ScaleRate as a negative
    public float ShrinkRate;

	// Use this for initialization
	void Start ()
    {
        //ScaleRate will equal negative ShrinkRate by default
        ScaleRate = -ShrinkRate;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 NewVector;
        NewVector.x = 0.0f;
        NewVector.y = ScaleRate;
        //Add the scale rate to our BoxCollider size
        GetComponent<BoxCollider2D>().size += NewVector;

        //Adjust our offset so that it will always be anchored at the bottom
        NewVector.y = ScaleRate / 2.0f;
        GetComponent<BoxCollider2D>().offset += NewVector;
	}
}
