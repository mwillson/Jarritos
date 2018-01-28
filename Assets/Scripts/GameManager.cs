using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	GameObject ball, electrofield;
	public GameObject ballPrefab, targetPrefab;
	public List<GameObject> targets;
	public int level;
	ElectroFieldScript efscript;
    public UITriggerSound soundManager;

	// Use this for initialization
	void Start () {
		level = 0;
		targets = new List<GameObject> ();
		electrofield = GameObject.Find ("ElectroField");
		//ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
		efscript = electrofield.GetComponent<ElectroFieldScript> ();
		NewLevel (1, 4);

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
		if (ball != null) {
			Destroy (ball);
		}
		ball = Instantiate (ballPrefab, new Vector3(0f,0f), transform.rotation);
        if (soundManager != null)
        {
            soundManager.cueSFX2();
        }
    }

    public void GameOver(){
        StartCoroutine(gameover2());
	}
    IEnumerator gameover2()
    {
        soundManager.cueDynamicLose();
        while(soundManager.curMusicCue != null && soundManager.curMusicCue.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene("title", LoadSceneMode.Single);
    }

    public void NewLevel(int lvl, int numtargets){
		level = lvl;
		Debug.Log ("New Level:"+lvl);
		string lvlString = (lvl + 1).ToString();
		GameObject.Find ("NextLvl").GetComponent<Text> ().text = "Next Lvl : "+lvlString+" - - - - - - - - - - - - - - - - - - - - - - - Next Lvl : "+lvlString;
		electrofield.GetComponent<BoxCollider2D> ().offset = new Vector2(electrofield.GetComponent<BoxCollider2D> ().offset.x, 0);
		electrofield.GetComponent<BoxCollider2D> ().size = new Vector2(electrofield.GetComponent<BoxCollider2D> ().size.x, 1.4f);
		efscript.ScaleRate = 0f;
		efscript.StartGrowTimer (0f, 0f);
		efscript.ShrinkRate += .0001f;
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
			float scaleFactor = 1.2f - Random.Range (0f, level / 20f);
			newTarget.transform.localScale *= scaleFactor;
			newTarget.GetComponent<TargetScript> ().SecondsToGrow = 1f - (level/30f) + (1/scaleFactor);
			targets.Add (newTarget);
		}
	}
}
