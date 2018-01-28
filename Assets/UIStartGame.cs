using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public UITriggerSound soundManager;

    public void onClick()
    {
        StartCoroutine(start2());
    }

    public void soundTest()
    {
        StartCoroutine(soundtest2());
    }

    IEnumerator start2()
    {
        soundManager.cueSFX4();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Scenes/marktest", LoadSceneMode.Single);
    }
    IEnumerator soundtest2()
    {
        soundManager.cueSFX1();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Scenes/soundboard", LoadSceneMode.Single);
    }

}
