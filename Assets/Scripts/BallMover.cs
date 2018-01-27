using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour {

	Vector3 direction;

	// Use this for initialization
	void Start () {
		direction = new Vector3 (Random.Range(-.03f,.03f), -.05f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (direction);
	}

	void Bounce(string dir){
		float xchange = 1f, ychange = 1f;
		switch (dir) {
		case "horizontal":
			//flip x direction
			ychange = -1f;
			break;
		case "vertical":
			//flip y direction
			xchange = -1f;
			break;
		}
		float newx = direction.x * xchange;
		float newy = direction.y * ychange;
		direction = new Vector3 (newx, newy, 0f);
	}

	void OnTriggerEnter2D(Collider2D other){
		string bouncedir="";
		switch (other.gameObject.name) {
		case"Bottom":
			bouncedir = "horizontal";
			break;
		case"Top":
			bouncedir = "horizontal";
			break;
		case"Left":
			bouncedir = "vertical";
			break;
		case"Right":
			bouncedir = "vertical";
			break;
		case "Paddle":
			bouncedir = "horizontal";
			break;
		}
		Bounce (bouncedir);
	}
}
