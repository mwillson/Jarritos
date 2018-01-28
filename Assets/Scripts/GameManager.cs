using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	GameObject ball, electrofield;
	public GameObject ballPrefab, targetPrefab;
	public List<GameObject> targets;
	public int level;

	// Use this for initialization
	void Start () {
		level = 0;
		targets = new List<GameObject> ();
		electrofield = GameObject.Find ("ElectroField");
		NewLevel (1, 4);
		//ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RespawnBall(){
		if (ball != null) {
			Destroy (ball);
		}
		ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
	}

	public void GameOver(){
		SceneManager.LoadScene ("title");
	}

	public void NewLevel(int lvl, int numtargets){
		level = lvl;
		Debug.Log ("New Level:"+lvl);
		electrofield.GetComponent<BoxCollider2D> ().offset = new Vector2(electrofield.GetComponent<BoxCollider2D> ().offset.x, 0);
		electrofield.GetComponent<BoxCollider2D> ().size = new Vector2(electrofield.GetComponent<BoxCollider2D> ().size.x, 1.4f);
		SpawnTargets (numtargets);
		RespawnBall ();
	}

	public void SpawnTargets(int num){
		foreach (GameObject tar in targets) {
			Destroy (tar);
		}
		targets.Clear();
		for (int i = 0; i < num; i++) {
			float xpos = Random.Range (-.9f, .9f);
			float ypos = Random.Range (-.6f, .8f);
			GameObject newTarget = Instantiate (targetPrefab, new Vector3 (xpos, ypos, 0f), transform.rotation);
			targets.Add (newTarget);
		}
	}
}
