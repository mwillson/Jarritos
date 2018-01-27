using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroFieldScript : MonoBehaviour {

    public float ScaleRate;
    public float ShrinkRate;

	// Use this for initialization
	void Start () {
        ScaleRate = -ShrinkRate;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 NewVector;
        NewVector.x = 0.0f;
        NewVector.y = ScaleRate;
        GetComponent<BoxCollider2D>().size += NewVector;
        NewVector.y = ScaleRate / 2.0f;
        GetComponent<BoxCollider2D>().offset += NewVector;
	}
}
