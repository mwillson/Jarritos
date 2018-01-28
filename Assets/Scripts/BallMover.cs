using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour {

	Vector3 direction;
	Collider2D electroCollider;
	bool outOfField;
	Rigidbody2D myRB { get; set; }
	Transform nextLvl;
	GameManager gm;
	ElectroFieldScript efscript;

	// Use this for initialization
	void Start () {
		outOfField = false;
		direction = new Vector3 (Random.Range(-.5f,.5f), -1.2f, 0f);
		electroCollider = GameObject.Find ("ElectroField").GetComponent<BoxCollider2D> ();
		myRB = GetComponent<Rigidbody2D> ();
		nextLvl = GameObject.Find ("NextLvl").transform;
		gm = GameObject.FindObjectOfType<GameManager> ();
		efscript = GameObject.FindObjectOfType<ElectroFieldScript> ();
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
		if (transform.position.y > nextLvl.position.y) {
			int upper = 0;
			int lower = 0;
			if (gm.level <= 5) {
				lower = 1;
				upper = 3;
			} else if (gm.level <= 10) {
				lower = 2;
				upper = 5;
			} else if (gm.level <= 20) {
				lower = 4;
				upper = 7;
			} else {
				lower = 6;
				upper = 9;
			}
			gm.NewLevel (gm.level + 1, 2 + Random.Range(lower,upper));
		}

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
        case "ontop":
            xchange = 1f;
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
					float penaltyRate = 0f;
						if (gm.level <= 5)
							penaltyRate = -.0008f;
						else if (gm.level <= 10)
							penaltyRate = -.002F;
						else if (gm.level <= 20)
							penaltyRate = -.003F;
						else if (gm.level <= 30)
							penaltyRate = -.006F;
						else if (gm.level <= 40)
							penaltyRate = -.01F;
						StartCoroutine(GameObject.FindObjectOfType<ElectroFieldScript>().StartGrowTimer(.5f, penaltyRate));
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
            if (other.gameObject.GetComponent<TargetScript>().isBreakable)
            {
                other.gameObject.GetComponent<TargetScript>().AddHitToTarget();
            }
            string bouncedir = "";
            if (transform.position.y > other.transform.position.y)
            {
                bouncedir = "ontop";
            }
            else if (Mathf.Abs(other.transform.position.x) - Mathf.Abs(transform.position.x) - Mathf.Abs(other.transform.position.y) - Mathf.Abs(transform.position.y) < -0.1f)
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

            other.gameObject.GetComponent<TargetScript>().OnTargetHit();
            Vector2 VectorToNudge = new Vector2();
            float BaseNudge = other.gameObject.GetComponent<TargetScript>().BaseNudgeAmount;

            if (direction.x != 0.0f)
            {
                //Set the nudge vector to BaseNudge times the sign (- or +) of the velocity of that particular axis
                VectorToNudge.x = -BaseNudge * ((transform.position.x > other.transform.position.x) ? (1.0f) : (-1.0f));
            }
            if (direction.y != 0.0f)
            {
                VectorToNudge.y = -BaseNudge * ((transform.position.y < other.gameObject.transform.position.y) ? (-1.0f) : (0.0f));
            }

            other.gameObject.GetComponent<TargetScript>().NudgeTarget(VectorToNudge);
        }
    }
		
	public void BackIn(){
		if (outOfField) {
			myRB.gravityScale = 0f;
			direction = new Vector3 (direction.x, direction.y * -1f, 0f);
			outOfField = false;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.name == "ElectroField") {
			JumpOut ();
		}
	}

	public void JumpOut(){
		outOfField = true;
		myRB.velocity = new Vector3 (0f, 0f, 0f);
		myRB.gravityScale = 1f;
		myRB.bodyType = RigidbodyType2D.Dynamic;
		myRB.AddForce (direction*100f);
	}
}
