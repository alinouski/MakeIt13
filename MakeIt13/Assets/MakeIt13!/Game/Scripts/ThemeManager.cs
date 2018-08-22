using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour {

    public int currentThemeId;
    public GameObject currentBackground;

    public ColoredObject[] coloredObjects;

    public Theme[] themes;

    public InfoPopup infos;
	// Use this for initialization
	void Start () {
        currentThemeId = -1;
        foreach (Theme t in themes) { t.LoadTheme(); }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ThemeIn()
    {
        GetComponent<Animator>().Play("ThemeIn");

        if (infos.GetComponentInChildren<ScrollRect>() != null)
        infos.GetComponent<Animator>().Play("ThemeOut");
    }

    public void ThemeOut()
    {
        GetComponent<Animator>().Play("ThemeOut");
    }
}
