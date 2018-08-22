using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColor : MonoBehaviour {

    public Color[] colors;

    ThemeManager tm;
	// Use this for initialization
	void Awake () {
        tm = FindObjectOfType<ThemeManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        GetComponent<LineRenderer>().startColor = colors[tm.currentThemeId];
        GetComponent<LineRenderer>().endColor = colors[tm.currentThemeId];
    }
}
