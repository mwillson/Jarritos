using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

    //The amount that the ElectroField should grow when this target is hit
    public float GrowAmountOnHit;
    //The duration for which the above grow amount should be applied
    public float SecondsToGrow;

    private float StackedSeconds;

    public bool isNudgeable = true;
    public float BaseNudgeAmount = 0.05f;
    public float XNudgeThreshold;
    public float YNudgeThreshold;

    private bool moveRight = true;

    private bool moveUp = true;

    public float XMovementSpeed;

    public float YMovementSpeed;

    private bool isCollidingWithOtherTarget;

    public float LowerXBound;
    public float UpperXBound;
    public float LowerYBound;
    public float UpperYBound;


    //Reference to the ElectroField in the game world; defined in Start()
    public GameObject ElectroObject;

    public UITriggerSound soundManager;

    public bool isBreakable;
    public int numHitsToBreak = 3;
    public float TimeBetweenBreakAndDestroy;
    private int numHitsTaken;
    public Sprite[] BreakSprites;
    public Sprite FinalBreakSprite;

    public void MoveTarget()
    {
        if (XMovementSpeed > 0f)
        {
            if (moveRight)
            {
                transform.Translate(new Vector3(XMovementSpeed, 0.0f, 0.0f));
                if (transform.position.x + XMovementSpeed >= UpperXBound)
                    moveRight = false;

            }
            else if (!moveRight)
            {
                transform.Translate(new Vector3(-XMovementSpeed, 0.0f, 0.0f));
                if (transform.position.x - XMovementSpeed <= LowerXBound)
                    moveRight = true;
            }
        }
        if (YMovementSpeed > 0f)
        {
            if (moveUp)
            {
                transform.Translate(new Vector3(0.0f, YMovementSpeed, 0.0f));
                if (transform.position.y + YMovementSpeed >= UpperYBound)
                    moveUp = false;
            }
            else if (!moveUp)
            {
                transform.Translate(new Vector3(0.0f, -YMovementSpeed, 0.0f));
                if (transform.position.y - YMovementSpeed <= LowerYBound)
                    moveUp = true;
            }
        }
    }

    public void NudgeTarget(Vector2 NudgeVector)
    {
        if (isNudgeable)
        {
            if (transform.position.x + NudgeVector.x > LowerXBound && transform.position.x + NudgeVector.x < UpperXBound)
            {
                if (!isCollidingWithOtherTarget)
                {
                    Vector3 TranslateVector = new Vector3(NudgeVector.x, 0.0f, 0.0f);
                    transform.Translate(TranslateVector);
                }
            }

            if (transform.position.y + NudgeVector.y > LowerYBound && transform.position.y + NudgeVector.y < UpperYBound)
            {
                if (!isCollidingWithOtherTarget)
                {
                    Vector3 TranslateVector = new Vector3(0.0f, NudgeVector.y, 0.0f);
                    transform.Translate(TranslateVector);
                }
            }
        }
    }

    //Called when this target is hit by the ball
    public void OnTargetHit()
    {
        //If we can get the ElectroFieldScript component (sometimes we have a collision on frame 1 before Start() is called)
        if (ElectroObject != null && ElectroObject.GetComponent<ElectroFieldScript>())
        {
            if (soundManager != null)
            {
                soundManager.cueSFX3();
            }
            StartCoroutine(ElectroObject.GetComponent<ElectroFieldScript>().StartGrowTimer(SecondsToGrow, GrowAmountOnHit));
        }
    }

    //Adds a break/damage to the target
    public void AddHitToTarget()
    {
        if (isBreakable)
        {
            numHitsTaken++;
            if (numHitsTaken < BreakSprites.Length && BreakSprites[numHitsTaken] != null)
                GetComponent<SpriteRenderer>().sprite = BreakSprites[numHitsTaken];
            if (numHitsTaken >= numHitsToBreak)
                BreakTarget();
        }
    }

    public void BreakTarget()
    {
        if (FinalBreakSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = FinalBreakSprite;
        }
        BreakTargetWait(TimeBetweenBreakAndDestroy);
        Destroy(gameObject);
    }

    public IEnumerator BreakTargetWait(float InSeconds)
    {
        yield return new WaitForSeconds(InSeconds);
    }

    // Use this for initialization
    void Start ()
    {
        //Get the ElectroField from the game world
        ElectroObject = GameObject.Find("ElectroField");
        soundManager = GameObject.Find("SoundController").GetComponent<UITriggerSound>();

        moveRight = true;
        moveUp = true;
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
        if (XMovementSpeed > 0f || YMovementSpeed > 0f)
        {
            MoveTarget();
            print(moveRight);
        }
	}
}
