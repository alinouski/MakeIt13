using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

    public GameObject menu, bg;
    // Use this for initialization
    void Start()
    {
        Invoke("StartGame", 3.3f);
        Invoke("BG", 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGame()
    {
        menu.SetActive(true);
        gameObject.SetActive(false);
    }

    void BG()
    {
        bg.SetActive(true);
    }
}
