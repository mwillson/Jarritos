﻿using System.Collections;
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

    public void onClick()
    {
        SceneManager.LoadScene("Scenes/marktest", LoadSceneMode.Single);
    }

    public void soundTest()
    {
        SceneManager.LoadScene("Scenes/soundboard", LoadSceneMode.Single);
    }

}