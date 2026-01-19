using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdManager : MonoBehaviour
{
    public static BannerAdManager Instance;

    private BannerView bannerView;

    [Header("AdMob")]
    [SerializeField]
    private string bannerAdUnitId = "ca-app-pub-8530302013109448/3496815230";

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
        MobileAds.Initialize(initStatus => { LoadBanner(); });
    }

    public void LoadBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

     
        AdSize customSize = new AdSize(468, 60);
        bannerView = new BannerView(bannerAdUnitId, customSize, AdPosition.Bottom);


        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }

    public void ShowBanner()
    {
        if (bannerView != null)
            bannerView.Show();
    }

    public void HideBanner()
    {
        if (bannerView != null)
            bannerView.Hide();
    }

    void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
