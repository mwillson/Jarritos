using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroFieldScript : MonoBehaviour {

    public float ShrinkRate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 NewVector;
        NewVector.x = 0.0f;
        NewVector.y = -ShrinkRate;
        GetComponent<BoxCollider2D>().size += NewVector;
        NewVector.y = -ShrinkRate / 2.0f;
        GetComponent<BoxCollider2D>().offset += NewVector;
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.GetComponent<BallMover>() != null) {
			other.GetComponent<BallMover> ().JumpOut ();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.GetComponent<BallMover>() != null) {
			other.GetComponent<BallMover> ().BackIn();
		}
	}

}
