using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

    //The amount that the ElectroField should grow when this target is hit
    public float GrowAmountOnHit;
    //The duration for which the above grow amount should be applied
    public float SecondsToGrow;

    private float StackedSeconds;

    private bool isCollidingWithOtherTarget;

    public float LowerXBound;
    public float UpperXBound;
    public float LowerYBound;
    public float UpperYBound;
    public float BaseNudgeAmount = 0.05f;

    //Reference to the ElectroField in the game world; defined in Start()
    public GameObject ElectroObject;

    public void NudgeTarget(Vector2 NudgeVector)
    {
        if (transform.position.x + NudgeVector.x > LowerXBound && transform.position.x + NudgeVector.x < UpperXBound)
        {
            if (transform.position.y + NudgeVector.y > LowerYBound && transform.position.y + NudgeVector.y < UpperYBound)
            {
                if (!isCollidingWithOtherTarget)
                {
                    Vector3 TranslateVector = new Vector3(NudgeVector.x, NudgeVector.y, 0.0f);
                    transform.Translate(TranslateVector);
                }
            }
        }
    }

    //Called when this target is hit by the ball
    public void OnTargetHit()
    {
        //If we can get the ElectroFieldScript component
        if (ElectroObject.GetComponent<ElectroFieldScript>())
        {
            StartCoroutine(ElectroObject.GetComponent<ElectroFieldScript>().StartGrowTimer(SecondsToGrow, GrowAmountOnHit));
        }
    }

	// Use this for initialization
	void Start ()
    {
        //Get the ElectroField from the game world
        ElectroObject = GameObject.Find("ElectroField");
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<TargetScript>())
        {
            isCollidingWithOtherTarget = true;
        }
        else
            isCollidingWithOtherTarget = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
