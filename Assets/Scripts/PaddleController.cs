using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {

    //Left and right movement boundaries
    public float LeftBound = -2.3f;
    public float RightBound = 2.3f;

    //Input sensitivity
    public float InputMultiplier = 1.0f;

    //Translates the paddle along the X axis according to InputScale
    void MovePaddle(float InputScale)
    {
        if (InputScale != 0.0f)
        {
            //Multiply InputScale by InputMultiplier
            //and divide it by 10 for ease of use in editor
            InputScale *= InputMultiplier / 10.0f;

            //Create a new transform and set X to InputScale as defined above
            Vector3 NewTransform;
            NewTransform.x = InputScale;
            NewTransform.y = 0.0f;
            NewTransform.z = 0.0f;

            //Translate the Paddle towards NewTransform
            transform.Translate(NewTransform);
        }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If the left input button is held and we're not at the left boundary
		if (Input.GetButton("Left") && transform.position.x > LeftBound)
        {
            //Move the paddle left by -1.0
            MovePaddle(-1.0f);
        }

        //If the right input button is held and we're not at the right boundary
        if (Input.GetButton("Right") && transform.position.x < RightBound)
        {
            //Move the paddle right by 1.0
            MovePaddle(1.0f);
        }
	}
}
