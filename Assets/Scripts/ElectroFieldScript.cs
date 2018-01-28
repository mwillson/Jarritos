using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroFieldScript : MonoBehaviour {

    //The scale rate continuously applied in Update, manipulated by the Target objects
    public float ScaleRate;

    //The rate at which the field should shrink, applied to ScaleRate as a negative
    public float ShrinkRate;

    // Use this for initialization
    void Start()
    {
        //ScaleRate will equal negative ShrinkRate by default
        ScaleRate = -ShrinkRate;
    }


        // Update is called once per frame
    void Update ()
    {
        Vector2 ScaleVector = new Vector2(0.0f, ScaleRate);
        //Add the scale rate to our BoxCollider size
        GetComponent<BoxCollider2D>().size += ScaleVector;

        //Adjust our offset so that it will always be anchored at the bottom
        ScaleVector.y = ScaleRate / 2.0f;
        GetComponent<BoxCollider2D>().offset += ScaleVector;


        GetComponent<SpriteRenderer> ().size = new Vector2(1f,GetComponent<BoxCollider2D> ().size.y + GetComponent<BoxCollider2D>().offset.y*2f);

		if (GetComponent<SpriteRenderer> ().size.y <= 0f) {
			GameObject.FindObjectOfType<GameManager>().GameOver ();
		}
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
