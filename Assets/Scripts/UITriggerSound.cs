using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITriggerSound : MonoBehaviour {

    public AudioSource sfx1;
    public AudioSource sfx2;
    public AudioSource sfx3;
    public AudioSource sfx4;
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
    public AudioSource nextDynamicMusicCue;

    public Text debugText;

    public float lastDynamicCueTime;

    // Use this for initialization
    void Start () {
        dynamic_intensity = (float)0.0;
        dynamic_state = null;
        dynamic_impulse = (float)0.0;
        lastDynamicCueTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > lastDynamicCueTime + 1.0) // suppress dynamic step until at least 1 seconds pass
        {
            stepDynamicMusic();
            lastDynamicCueTime = Time.time;
        }

        if (nextMusicCue != null || nextDynamicMusicCue != null)
        {
            bool canCue = false;

            if (curMusicCue == null) // bypass cue playing check
            {
                canCue = true;
            }
            else if (curMusicCue.isPlaying)
            {
                if (curMusicCue.timeSamples < 22050 / 8)
                {
                    // allow the cuechange to take place if the next cue is not the same
                    canCue = true;
                }
            } else if (!curMusicCue.isPlaying)
            {
                curMusicCue.Stop();
                canCue = true;
            }

            if (canCue)
            {
                if (nextDynamicMusicCue != null)
                {
                    nextMusicCue = nextDynamicMusicCue;
                    nextDynamicMusicCue = null;
                }
                if (nextMusicCue != curMusicCue)
                {
                    if (curMusicCue != null)
                        curMusicCue.Stop();
                    nextMusicCue.Play();
                    curMusicCue = nextMusicCue;
                    nextMusicCue = null;
                }
            }

        }
	}

    public float dynamic_intensity;
    public string dynamic_state;
    public float dynamic_impulse;

    public void cueDynamicBegin()
    {
        dynamic_state = "load";
        dynamic_intensity = (float)0.0;
        dynamic_impulse = (float)0.0;
        nextDynamicMusicCue = music1; // load
    }
    public void cueDynamicWin()
    {
        dynamic_state = null;
        dynamic_intensity = (float)0.0;
        dynamic_impulse = (float)0.0;
        nextDynamicMusicCue = music6; // lose
    }
    public void cueDynamicLose()
    {
        dynamic_state = null;
        dynamic_intensity = (float)0.0;
        dynamic_impulse = (float)0.0;
        nextDynamicMusicCue = music7; // lose
    }
    public void dynamicImpulseIntense()
    {
        dynamic_impulse += 1.5f;
    }
    public void dynamicImpulseCalm()
    {
        dynamic_impulse -= 1.5f;
    }
    public void dynamicImpulseIntense2()
    {
        dynamic_impulse += 0.013f;
    }
    public void dynamicImpulseCalm2()
    {
        dynamic_impulse -= 0.01f;
    }
    public void stepDynamicMusic()
    {
        dynamic_intensity += dynamic_impulse;
        dynamic_impulse = 0.0f;
        if (dynamic_state != null)
        {
            dynamic_intensity = (float)(dynamic_intensity * 0.99);
            if (dynamic_state == "load")
            {
                nextDynamicMusicCue = music1; // loading
                dynamic_state = "gameplay";
                dynamic_intensity = 0.0f;
            }
            else if (dynamic_state == "gameplay")
            {
                if (dynamic_intensity > 2.0)
                {
                    nextDynamicMusicCue = music5; // intense
                    dynamic_intensity = 2.0f;
                }
                else if (dynamic_intensity > 1.0)
                    nextDynamicMusicCue = music4; // challenging
                else if (dynamic_intensity > -1.0)
                    nextDynamicMusicCue = music3; // 
                else if (dynamic_intensity < -2.0)
                {
                    nextDynamicMusicCue = music2; // relaxed
                    dynamic_intensity = -2.0f;
                }
            }
        }
        if (debugText != null)
            debugText.text = "Intensity: " + dynamic_intensity.ToString();
    }

    public void cueSFX1()
    {
        sfx1.Play();    
    }
    public void cueSFX2()
    {
        sfx2.Play();
    }
    public void cueSFX3()
    {
        sfx3.Play();
    }
    public void cueSFX4()
    {
        sfx4.Play();
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
