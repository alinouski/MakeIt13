using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

    public AudioClip[] mergeSounds;

    public Sprite musicOn, musicOff;

    public GameObject sound;
    public void MuteToggle()
    {
        if (efxSource.mute == true)
        {
            efxSource.mute = false;
            musicSource.mute = false;

            sound.GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            efxSource.mute = true;
            musicSource.mute = true;

            sound.GetComponent<Image>().sprite = musicOff;
        }
    }

    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = clip;

        //Play the clip.
        //efxSource.Play();
    }

    public void PlayMerged()
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = mergeSounds[Random.Range(0, mergeSounds.Length)];

        //Play the clip.
        //efxSource.Play();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("sound") == 1) { MuteToggle(); }
    }
}
