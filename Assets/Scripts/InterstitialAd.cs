using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterstitialAdManager : MonoBehaviour
{
    public static InterstitialAdManager Instance;

    private InterstitialAd interstitialAd;
    private bool isShowingAd;

    [Header("AdMob")]
    [SerializeField]
    private string interstitialAdUnitId =
        "ca-app-pub-3940256099942544/1033173712"; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(interstitialAdUnitId, request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.Log("Interstitial failed to load: " + error);
                    return;
                }

                interstitialAd = ad;
                RegisterCallbacks();
                Debug.Log("Interstitial loaded");
            });
    }

    void RegisterCallbacks()
    {
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            isShowingAd = true;
            Debug.Log("Interstitial opened");
        };

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial closed");
            isShowingAd = false;
            LoadInterstitial();
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial failed to show: " + error);
            isShowingAd = false;
            LoadInterstitial();
        };
    }

    public void ShowInterstitialIfReady()
    {
        if (isShowingAd)
            return;

        if (interstitialAd == null || !interstitialAd.CanShowAd())
        {
            Debug.Log("Interstitial not ready");
            return;
        }

        Time.timeScale = 1f;
        interstitialAd.Show();
    }

    public void TryShowAdForLevel(int levelNumber)
    {
        if (levelNumber >= 10 && (levelNumber - 10) % 4 == 0)
        {
            ShowInterstitialIfReady();
        }
    }
}
