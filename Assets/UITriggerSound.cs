using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITriggerSound : MonoBehaviour {

    public AudioSource sfx;
    public AudioSource music1;
    public AudioSource music2;
    public AudioSource music3;
    public AudioSource music4;
    public AudioSource music5;
    public AudioSource music6;
    public AudioSource music7;
    public AudioSource music8;

    public AudioSource curMusicCue;
    public AudioSource nextMusicCue;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (nextMusicCue != null)
        {

            bool canCue = false;

            if (curMusicCue == null)
                canCue = true;
            else if (curMusicCue.isPlaying && nextMusicCue != curMusicCue)
            {
                if (curMusicCue.timeSamples < 22050/8)
                {
                    curMusicCue.Stop();
                    canCue = true;
                }
            } else if (!curMusicCue.isPlaying)
            {
                curMusicCue.Stop();
                canCue = true;
            }

            if (canCue)
            {
                nextMusicCue.Play();
                curMusicCue = nextMusicCue;
                nextMusicCue = null;
            }

        }
	}

    public void cueSFX()
    {
        sfx.Play();    
    }
    public void cueMusic1()
    {
        nextMusicCue = music1;
    }
    public void cueMusic2()
    {
        nextMusicCue = music2;
    }
    public void cueMusic3()
    {
        nextMusicCue = music3;
    }
    public void cueMusic4()
    {
        nextMusicCue = music4;
    }
    public void cueMusic5()
    {
        nextMusicCue = music5;
    }
    public void cueMusic6()
    {
        nextMusicCue = music6;
    }
    public void cueMusic7()
    {
        nextMusicCue = music7;
    }
    public void cueMusic8()
    {
        nextMusicCue = music8;
    }
    public void backToTitle()
    {
        SceneManager.LoadScene("Scenes/title", LoadSceneMode.Single);
    }

}
