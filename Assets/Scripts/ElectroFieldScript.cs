﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroFieldScript : MonoBehaviour {

    //The scale rate continuously applied in Update, manipulated by the Target objects
    public float ScaleRate;

    //The rate at which the field should shrink, applied to ScaleRate as a negative
    public float ShrinkRate;

    private float StackedSeconds;

    public float MaxStackedSeconds = 10.0f;

    public float MaxGrowthRate = 0.0005f;

    public bool InertialGrowth;

    public UITriggerSound soundManager;

    public float NearLosingScale;
    public float NearWinningScale;

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

        GetComponent<SpriteRenderer> ().size = new Vector2(1f,Mathf.Max(0f, GetComponent<BoxCollider2D> ().size.y + GetComponent<BoxCollider2D>().offset.y*2f));

        if (GetComponent<SpriteRenderer> ().size.y <= 0.00001f) {
			GameObject.FindObjectOfType<GameManager>().GameOver ();
		}

        if (soundManager != null)
        {
            if (GetComponent<BoxCollider2D>().size.y > NearWinningScale)
            {
                soundManager.dynamicIntensityWinning();
            }
            else if (GetComponent<BoxCollider2D>().size.y < NearLosingScale)
            {
                soundManager.dynamicIntensityLosing();
            }
            else
            {
                soundManager.dynamicIntensityNeutral();
            }
        }
    }

    public IEnumerator StartGrowTimer(float InSeconds, float GrowAmountOnHit)
    {
        //Temporarily set the ElectroField to grow by GrowAmount
        if (ScaleRate <= 0.0f)
        {
            if (soundManager != null)
            {
                //soundManager.dynamicImpulseCalm();
            }
            if (InertialGrowth)
                ScaleRate += GrowAmountOnHit;
            else
                ScaleRate = GrowAmountOnHit;
            StackedSeconds = InSeconds;
            //Set a timer
            yield return new WaitForSeconds(StackedSeconds);
        }

        else
        {
			if (ScaleRate + GrowAmountOnHit <= MaxGrowthRate)
				ScaleRate += GrowAmountOnHit;
			if (StackedSeconds + InSeconds <= MaxStackedSeconds)
				StackedSeconds += InSeconds;
            //Set a timer
            yield return new WaitForSeconds(StackedSeconds);
            StackedSeconds -= InSeconds;
        }

        //When the timer is up, set the ElectroField to shrink again
        if (StackedSeconds <= InSeconds)
        {
            ScaleRate = -ShrinkRate;
            if (soundManager != null)
            {
                //soundManager.dynamicImpulseIntense();
            }
        }
    }

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.GetComponent<BallMover>() != null) {
			other.GetComponent<BallMover> ().JumpOut ();
            if (soundManager != null)
            {
                soundManager.cueSFX3();
            }
        }
    }

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.GetComponent<BallMover>() != null) {
			other.GetComponent<BallMover> ().BackIn();
        }
    }
}
