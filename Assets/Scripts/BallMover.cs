using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour {

	Vector3 direction;
	Collider2D electroCollider;
	bool outOfField;
	Rigidbody2D myRB { get; set; }

	// Use this for initialization
	void Start () {
		outOfField = false;
		direction = new Vector3 (Random.Range(-.5f,.5f), -1.2f, 0f);
		electroCollider = GameObject.Find ("ElectroField").GetComponent<BoxCollider2D> ();
		myRB = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		if (outOfField) {
			//keep track of direction as we are "flying"
			//direction = new Vector3(myRB.velocity.x, myRB.velocity.y, 0f);
			//Debug.Log (direction);
			if (myRB.velocity.y == 0f) {
				myRB.velocity = new Vector3 (0f, 0f, 0f);
				myRB.AddForce (new Vector2 (0f, -70f));
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if (outOfField) {
			return;
		}
		myRB.velocity = direction;

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
		//Debug.Log ("old dir:" + direction);
		direction = new Vector3 (newx, newy, 0f);
		//Debug.Log ("new dir: " + direction);
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.parent != null)
        {


            if (other.transform.parent.name == "BG")
            {
                string bouncedir = "";
                Debug.Log(other.gameObject.name);
                switch (other.gameObject.name)
                {
                    case "Bottom":
                        //special case
                        GameObject.FindObjectOfType<GameManager>().RespawnBall();
                        return;
                    case "Top":
                        bouncedir = "horizontal";
                        break;
                    case "Left":
                        bouncedir = "vertical";
                        break;
                    case "Right":
                        bouncedir = "vertical";
                        break;
                    case "Paddle":

                        bouncedir = "horizontal";
                        break;
                }
                Bounce(bouncedir);
            }
        }
        else if (other.gameObject.name == "Paddle")
        {
            //if ball collided below paddle, DONT BOUNCE. just return, keep it on it's trajectory

            if (transform.position.y < other.transform.position.y)
                return;
			float xOffset = (transform.position.x - other.transform.position.x)*2f;
			direction.x += xOffset;
            Bounce("horizontal");
        }

        if (other.gameObject.GetComponent<TargetScript>() != null)
        {
            string bouncedir = "";
            if (Mathf.Abs(other.transform.position.x) - Mathf.Abs(transform.position.x) - Mathf.Abs(other.transform.position.y) - Mathf.Abs(transform.position.y) < -0.1f)
            {
                bouncedir = "horizontal";
                Bounce(bouncedir);
                bouncedir = "vertical";
            }
            else if (Mathf.Abs(other.transform.position.x) - Mathf.Abs(transform.position.x) > Mathf.Abs(other.transform.position.y) - Mathf.Abs(transform.position.y))
            {
                bouncedir = "vertical";
            }
            else
            {
                bouncedir = "horizontal";
            }
            Bounce(bouncedir);

            StartCoroutine(other.gameObject.GetComponent<TargetScript>().OnTargetHit());
            Vector2 VectorToNudge = new Vector2();
            float BaseNudge = other.gameObject.GetComponent<TargetScript>().BaseNudgeAmount;

            if (direction.x != 0.0f)
            {
                //Set the nudge vector to BaseNudge times the sign (- or +) of the velocity of that particular axis
                VectorToNudge.x = -BaseNudge * ((direction.x > 0.0f) ? (1.0f) : (-1.0f));
            }
            if (direction.y != 0.0f)
            {
                VectorToNudge.y = -BaseNudge * ((direction.y > 0.0f) ? (1.0f) : (-1.0f));
            }

            other.gameObject.GetComponent<TargetScript>().NudgeTarget(VectorToNudge);
        }
    }
		
	public void BackIn(){
		if (outOfField) {
			Debug.Log ("back in!");
			myRB.gravityScale = 0f;
			direction = new Vector3 (direction.x, direction.y * -1f, 0f);
			Debug.Log ("direction back in:" + direction);
			outOfField = false;
			//myRB.bodyType = RigidbodyType2D.Kinematic;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.name == "ElectroField") {
			JumpOut ();
		}
	}

	public void JumpOut(){
		Debug.Log (direction * 100f);
		outOfField = true;
		myRB.velocity = new Vector3 (0f, 0f, 0f);
		myRB.gravityScale = 1f;
		myRB.bodyType = RigidbodyType2D.Dynamic;
		myRB.AddForce (direction*100f);
	}
}
