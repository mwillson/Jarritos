using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	GameObject ball;
	public GameObject ballPrefab;

	// Use this for initialization
	void Start () {
		ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RespawnBall(){
		Destroy (ball);
		ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
	}
}
