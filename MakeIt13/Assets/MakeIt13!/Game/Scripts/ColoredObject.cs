using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredObject : MonoBehaviour {

    public Color[] colors;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeColor(int n)
    {
        if (GetComponent<Image>() != null)
            GetComponent<Image>().color = colors[n];
        if (GetComponent<Text>() != null)
            GetComponent<Text>().color = colors[n];
    }
}
