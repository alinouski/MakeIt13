using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    private Animator anim;

    public GameObject endlessGame, timedGame, tutorialEndless, tutorialTimed, menu;
    private bool settingsBool = true;

    public new AudioManager audio;

    private bool paused = false;

    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

	}

    // Update is called once per frame
    void Update()
    {
    }


    public void Pause(bool pause)
    {
        if (pause) { endlessGame.GetComponent<Animator>().Play("PauseMenuIn"); timedGame.GetComponent<Animator>().Play("PauseMenuIn"); }
        else { timedGame.GetComponent<Animator>().Play("PauseMenuOut"); endlessGame.GetComponent<Animator>().Play("PauseMenuOut"); }

        paused = pause;
    }

    public void PlayEndless()
    {
        if (menu.activeSelf) anim.Play("MenuOut");
        Invoke("InvokePlayEndless", 0.75f);
    }

    public void PlayTimed()
    {
        if (menu.activeSelf) anim.Play("MenuOut");
        Invoke("InvokePlayTimed", 0.75f);
    }

    void InvokePlayEndless()
    {
        if (!PlayerPrefs.HasKey("firstGame")) {
            tutorialEndless.SetActive(true);
            PlayerPrefs.SetInt("firstGame", 1);
#if UNITY_ANDROID
            Analytics.CustomEvent("new user Android");
#elif UNITY_IOS
            Analytics.CustomEvent("new user IOS");
#endif
        }
        else
        {
            endlessGame.SetActive(true); tutorialEndless.SetActive(false);
        }

        gameObject.SetActive(false);

        if (paused)
        {
            endlessGame.GetComponent<Animator>().Play("PauseMenuOut");
            paused = true;
        }

    }

    void InvokePlayTimed()
    {
        if (!PlayerPrefs.HasKey("firstGame")) {
            tutorialTimed.SetActive(true);
            PlayerPrefs.SetInt("firstGame", 1);
#if UNITY_ANDROID
            Analytics.CustomEvent("new user Android");
#elif UNITY_IOS
            Analytics.CustomEvent("new user IOS");
#endif
        }
        else
        {
            timedGame.SetActive(true); tutorialTimed.SetActive(false);
        }

        gameObject.SetActive(false);

        if (paused)
        {
            timedGame.GetComponent<Animator>().Play("PauseMenuOut");
            paused = true;
        }

    }


    public void RemoveAds()
    {
        PlayerPrefs.SetInt("show_ads", 1);
    }

    public void Rate(GameObject g)
    {
        Application.OpenURL("market://details?id=com.fmgames.upto13");
        g.SetActive(false);
        PlayerPrefs.SetInt("shared", 1);
    }

    public void Settings()
    {
        if (settingsBool)
        {
            GameObject.Find("Settings").GetComponent<Animator>().Play("SettingsIn");
        }
        else GameObject.Find("Settings").GetComponent<Animator>().Play("SettingsOut");

        settingsBool = !settingsBool;
    }

    public void ToggleSound(Image img)
    {
        if (PlayerPrefs.GetInt("sound", 0) == 0) //Sound on
        {
            PlayerPrefs.SetInt("sound", 1); //toggle off
            audio.MuteToggle();
        }else
        {
            PlayerPrefs.SetInt("sound", 0); //toggle on
            audio.MuteToggle();
        }
    }

    public void BackToMenu()
    {
        endlessGame.SetActive(false);
        timedGame.SetActive(false);
        menu.SetActive(true);
    }
}
