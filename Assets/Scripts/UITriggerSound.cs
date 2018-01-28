using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITriggerSound : MonoBehaviour {

    public AudioSource sfx1;
    public AudioSource sfx2;
    public AudioSource[] sfx3; // bounce
    public AudioSource sfx4;
    public AudioSource[] sfx5; // win
    public AudioSource sfx6;
    public AudioSource sfx7;
    public AudioSource sfx8;
    public AudioSource sfx9;
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
    public bool rimshotTrigger;
    public float rimshotCueTime;

    // Use this for initialization
    void Start () {
        dynamic_intensity = (float)0.0;
        dynamic_state = null;
        dynamic_impulse = (float)0.0;
        dynamic_level = 1;
        lastDynamicCueTime = 0.0f;
        rimshotTrigger = false;
        rimshotCueTime = -1;
        generatePattern();
	}
	
	// Update is called once per frame
	void Update () {

        if (curMusicCue == null || Time.time > lastDynamicCueTime + curMusicCue.clip.length * 0.9) // dynamic step
        {
            stepDynamicMusic();
            lastDynamicCueTime = Time.time;
        }
        if (rimshotCueTime >= 0 && rimshotCueTime < Time.time)
        {
            sfx6.Play();
            rimshotCueTime = -1;
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
                if (curMusicCue != null && rimshotTrigger)
                {
                    rimshotCueTime = Time.time + curMusicCue.clip.length * 3 / 4;
                    rimshotTrigger = false;
                }
            }

        }
	}

    public float dynamic_intensity;
    public string dynamic_state;
    public float dynamic_impulse;
    public int dynamic_level;
    public int dynamic_pattern_idx;
    public AudioSource[][] pattern;

    public void cueDynamicBegin()
    {
        dynamic_state = "load";
        dynamic_intensity = (float)0.0;
        dynamic_impulse = (float)0.0;
        dynamic_pattern_idx = 0;
        nextDynamicMusicCue = music1; // load
    }
    public void cueDynamicWin()
    {
        dynamic_state = null;
        dynamic_intensity = (float)0.0;
        dynamic_impulse = (float)0.0;
        nextDynamicMusicCue = music7; // win
    }
    public void cueDynamicLose()
    {
        dynamic_state = null;
        dynamic_intensity = (float)0.0;
        dynamic_impulse = (float)0.0;
        nextDynamicMusicCue = music6; // lose
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

    public void dynamicIntensityWinning()
    {
        dynamic_intensity = -1.0f;
    }

    public void dynamicIntensityLosing()
    {
        dynamic_intensity = 1.0f;
    }

    public void dynamicIntensityNeutral()
    {
        dynamic_intensity = 0.0f;
    }

    public void generatePattern()
    {
        pattern = new AudioSource[3][];

        pattern[0] = new AudioSource[] { music1, music1, music1, music1, music1, music1, music1, music1 };
        pattern[1] = new AudioSource[] { music1, music1, music1, music1, music1, music1, music1, music1 };
        pattern[2] = new AudioSource[] { music1, music1, music1, music1, music1, music1, music1, music1 };

        AudioSource[] winningoptions = new AudioSource[] { music1, music3, music3, music4, music5, music5};
        AudioSource[] losingoptions = new AudioSource[] { music1, music2, music2, music3, music4, music4};
        AudioSource[] options = new AudioSource[] { music1, music1, music2, music3, music3, music4, music5 };
        for (int intense=0; intense < 3; intense++)
        {
            for (int idx = 0; idx < 8; idx++)
            {
                if (intense == 0)
                {
                    int musidx = Random.Range(0, losingoptions.Length);
                    pattern[intense][idx] = losingoptions[musidx];
                }
                if (intense == 1)
                {
                    int musidx = Random.Range(0, options.Length);
                    pattern[intense][idx] = options[musidx];
                }
                if (intense == 2)
                {
                    int musidx = Random.Range(0, winningoptions.Length);
                    pattern[intense][idx] = winningoptions[musidx];
                }
            }
        }
    }

    public void stepDynamicMusic()
    {
        //dynamic_intensity += dynamic_impulse;
        dynamic_impulse = 0.0f;
        if (dynamic_state != null)
        {
            //dynamic_intensity = (float)(dynamic_intensity * 0.99);
            if (dynamic_state == "load")
            {
                nextDynamicMusicCue = music8; // drums
                dynamic_state = "gameplay";
                dynamic_intensity = 0.0f;
            }
            else if (dynamic_state == "gameplay")
            {
                if (dynamic_pattern_idx >= pattern.Length)
                    dynamic_pattern_idx = 0;
                if (dynamic_intensity < 0f)
                {
                    nextDynamicMusicCue = pattern[2][dynamic_pattern_idx];
                }
                else if (dynamic_intensity > 0f)
                {
                    nextDynamicMusicCue = pattern[0][dynamic_pattern_idx];
                }
                else
                {
                    nextDynamicMusicCue = pattern[1][dynamic_pattern_idx];
                }
                dynamic_pattern_idx += 1;
                if (Random.Range(0,3)==0)
                {
                    rimshotTrigger = true;
                }
                /*
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
                */
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
        sfx3[Random.Range(0, sfx3.Length)].Play();
    }
    public void cueSFX4()
    {
        sfx4.Play();
    }
    public void cueSFX5()
    {
        sfx5[Random.Range(0, sfx5.Length)].Play();
    }
    public void cueSFX6()
    {
        rimshotTrigger = true;
    }
    public void cueSFX7()
    {
        sfx7.Play();
    }
    public void cueSFX8()
    {
        sfx8.Play();
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
