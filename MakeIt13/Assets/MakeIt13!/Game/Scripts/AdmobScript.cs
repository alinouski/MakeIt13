using UnityEngine;
using System.Collections;

public class AdmobScript : MonoBehaviour
{
    //private int themeIdToUnlock;

    //bool FUCKTHISSHIT = false;

    //float timer, timeToShow = 190f;
    // Use this for initialization
    void Start()
    {
    }

    private void Update()
    {
        //if (FUCKTHISSHIT)
        //{
        //    FUCKTHISSHIT = false;
        //    foreach (Theme t in FindObjectOfType<ThemeManager>().themes) if (t.id == themeIdToUnlock) t.PurchaseSuccessful();
        //}

        //timer += Time.deltaTime;
        //if (timer > timeToShow)
        //{
        //    timer = 0;
        //    ShowInterstitialAd();
        //}
    }

    public void ShowBanner()
    {
        //if (Enhance.IsBannerAdReady())
        //Enhance.ShowBannerAdWithPosition(Enhance.Position.BOTTOM);
    }

    public void ShowInterstitialAd()
    {
        if (PlayerPrefs.GetInt("show_ads") == 1) return;
        //Show Ad
        //if (Enhance.IsInterstitialReady())
        //{
        //    Enhance.ShowInterstitialAd();
        //}

    }


    public void ShowRewardedAd(int themeId)
    {
        //themeIdToUnlock = themeId;
        //if (Enhance.IsRewardedAdReady())
        //{
        //    Enhance.ShowRewardedAd(HandleRewardBasedVideoRewarded, OnRewardDeclined, OnRewardUnavailable);
        //}
    }
    //private void HandleRewardBasedVideoRewarded(Enhance.RewardType rewardType, int rewardValue)
    //{
    //    FUCKTHISSHIT = true;
    //}

    private void OnRewardDeclined()
    {
        // Reward is declined (user closed the ad for example)
    }

    private void OnRewardUnavailable()
    {
        // Reward is unavailable (network error for example)
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void HandleOnQuit(object sender, System.EventArgs args)
    {
        Application.Quit();
    }
}