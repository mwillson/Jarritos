using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	GameObject ball;
	public GameObject ballPrefab;
    public UITriggerSound soundManager;

	// Use this for initialization
	void Start () {
		ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
        if (soundManager != null)
        {
            soundManager.cueDynamicBegin();
        }
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(ball.transform.localPosition.y);
        if (ball.transform.localPosition.y > 0.25)
        {
            soundManager.dynamicImpulseCalm2();
        }
        if (ball.transform.localPosition.y < -0.25)
        {
            soundManager.dynamicImpulseIntense2();
        }

    }

    public void RespawnBall(){
		Destroy (ball);
		ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
    }

    public void GameOver(){
        soundManager.cueDynamicLose();
        SceneManager.LoadScene ("title");
	}
}
