using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Theme : MonoBehaviour {

    public GameObject priceText;
    public int id;

    public GameObject background;
    public Image colorBackground;
    public new AudioClip audio;

    public ThemeManager themeManager;

    public bool isPurchased = false;

    //public IAPButton iapButton;
    public GameObject showVideoAd;
	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetInt("purchase_theme" + id.ToString()) == 1) {
            isPurchased = true;
            //if (iapButton != null)
            //{
            //    iapButton.priceText.text = "";
            //    iapButton.gameObject.SetActive(false);
            //}           
        }

        if (showVideoAd != null)
        {
            if (!isPurchased)
            {
                showVideoAd.SetActive(true);
            }
            else
            {
                showVideoAd.SetActive(false);
            }
        }
    }
	
    public void LoadTheme()
    {
        if (PlayerPrefs.GetInt("theme_current", 3) == id)
        {
            themeManager.currentThemeId = id;


            if (themeManager.currentBackground != null)
                themeManager.currentBackground.SetActive(false);
            if (background != null)
            {
                background.SetActive(true);
                themeManager.currentBackground = background;
            }
            else
            {
                Camera.main.backgroundColor = colorBackground.color;
            }

            if (audio != null)
            {
                FindObjectOfType<AudioManager>().musicSource.clip = audio;
                //FindObjectOfType<AudioManager>().musicSource.Play();
            }
            


            foreach (ColoredObject c in themeManager.coloredObjects)
            {
                c.ChangeColor(id);
            }

            PlayerPrefs.SetInt("theme_current", id);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateTheme()
    {
        if (id == themeManager.currentThemeId || !isPurchased) return;
        themeManager.currentThemeId = id;


        if (themeManager.currentBackground != null)
            themeManager.currentBackground.SetActive(false);
        if (background != null) {
            background.SetActive(true);
            themeManager.currentBackground = background;
        }        
        else
        {
            Camera.main.backgroundColor = colorBackground.color;
        }

        if (audio != null)
        {
            FindObjectOfType<AudioManager>().musicSource.clip = audio;
            FindObjectOfType<AudioManager>().musicSource.Play();
        }


        foreach (ColoredObject c in themeManager.coloredObjects)
        {
            Debug.Log(c.gameObject.name);
            c.ChangeColor(id);
        }

        PlayerPrefs.SetInt("theme_current", id);

        if (FindObjectOfType<GameManager>() != null)
        {
            foreach (Tile t in FindObjectOfType<GameManager>().AllTiles)
            {
                t.ThemeChange();
            }
        }
    }

    public void PurchaseSuccessful()
    {
        //Analytics.Transaction(iapButton.productId, 0.99m, "USD", null, null);

        if (showVideoAd == null)
        {
            PlayerPrefs.SetInt("show_ads", 1);
            PlayerPrefs.SetInt("purchase_theme" + id.ToString(), 1);
        }

        isPurchased = true;
        ActivateTheme();
        //if (iapButton != null)
        //{
        //    iapButton.priceText.text = ""; iapButton.gameObject.SetActive(false);
        //}

        if (showVideoAd != null)
            showVideoAd.SetActive(false);

        

        //FindObjectOfType<GPS>().LoadFromCloud(true);
    }
}
