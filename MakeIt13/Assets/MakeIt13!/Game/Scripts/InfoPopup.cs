using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPopup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ThemeIn()
    {
        GetComponent<Animator>().Play("ThemeIn");
    }

    public void ThemeOut()
    {
        GetComponent<Animator>().Play("ThemeOut");
    }
}
