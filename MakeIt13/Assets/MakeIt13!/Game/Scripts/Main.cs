using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject exitPopUp;
    public GameObject share_ratePopUp;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitPopUp.SetActive(!exitPopUp.activeInHierarchy);
        }
    }

    public void ShowShareRateWindow()
    {
        share_ratePopUp.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ExitPopUpHide()
    {
        exitPopUp.SetActive(false);
    }

    public void MailToMisi()
    {
        Application.OpenURL("mailto:mihaly.szekely2@gmail.com");
    }

    public void MailToAdam()
    {
        Application.OpenURL("mailto:zuzzu01@gmail.com");
    }

    public void MailToLuc()
    {
        Application.OpenURL("mailto:luclionofficial@gmail.com");
    }

    public void OpenFacebook()
    {
        Application.OpenURL("fb://facewebmodal/f?href=https://www.facebook.com/FMGamesStudio");
    }

    public void MailToKyle()
    {
        Application.OpenURL("mailto:publishing@yucline.com");
    }

    public void ShareScore()
    {
        FindObjectOfType<FBLog>().FacebookShare(FindObjectOfType<ScoreTracker>().Score);
    }
}
