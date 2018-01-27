using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {

    public float XPosition;
    public float YPosition;
    public float InputMultiplier = 1.0f;

    void MovePaddle(float InputScale)
    {
        if (InputScale != 0.0f)
        {
            InputScale *= InputMultiplier / 10.0f;
            Vector3 NewTransform;
            NewTransform.x = InputScale;
            NewTransform.y = 0.0f;
            NewTransform.z = 0.0f;
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
        XPosition = transform.position.x;
        YPosition = transform.position.y;

		if (Input.GetButton("Left") && transform.position.x > -9.0f)
        {
            //print("Left pressed");
            MovePaddle(-1.0f);
        }

      if (Input.GetButton("Right") && transform.position.x < 9.0f)
        {
           // print("Right pressed");
            MovePaddle(1.0f);
        }

        else if (!Input.GetButton("Left") && !Input.GetButton("Right"))
        {
            MovePaddle(0.0f);
        }
	}
}
