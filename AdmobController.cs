using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsController : MonoBehaviour
{
    private string _adUnitId = "ca-app-pub-123123123";
    private string _adUnitIdRewarded = "ca-app-pub-123123123";
    private string _adUnitIdInterstitial = "ca-app-pub-123123123";

    private static AdsController instance;

    #region Singleton
    public static AdsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdsController>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("Ads Controller");
                    instance = singletonObject.AddComponent<AdsController>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion


    BannerView _bannerView;
    private RewardedAd rewardedAd;

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {

        });
    }


    #region  === Banner Ad ===


    public void AddBannerAd()
    {
        if (PlayerPrefs.GetInt("isPremium") != 1)
        {
            LoadAd();
        }

    }

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        if (_bannerView != null)
        {
            DestroyAd();
        }

        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);
        ListenToAdEvents();
    }

    public void LoadAd()
    {
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();

        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    private void ListenToAdEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    #endregion

    #region === Rewarded Ad ===

    public void RequestAndLoadRewaredAd(Action<bool> callbacks)
    {
        RequestRewardedAd();
        ShowRewardedAd(callbacks);
    }

    private void RequestRewardedAd()
    {
        var adRequest = new AdRequest();
        RewardedAd.Load(_adUnitIdRewarded, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }

    public void ShowRewardedAd(Action<bool> result)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("sukses", reward.Type, reward.Amount));

                RewardedBeforeLastFrame(result);
            });
        }
    }

    private void RewardedBeforeLastFrame(Action<bool> result)
    {
        StartCoroutine(RewardThePlayer(result));
    }

    private IEnumerator RewardThePlayer(Action<bool> result)
    {
        yield return new WaitForEndOfFrame();
        result?.Invoke(true);
    }

    #endregion


    #region === Interstitial Ad ===

    private InterstitialAd _interstitialAd;
    private Action<bool> _onInterstitialAdAction;

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitIdInterstitial, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
            });
    }

    public void ShowInterstitialAd(Action<bool> onInterstitialDone)
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _onInterstitialAdAction = onInterstitialDone;

            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            InterstitialBeforeLastFrame((bool isInterstitialDone) =>
            {
                _onInterstitialAdAction?.Invoke(isInterstitialDone);
            });

        };
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void InterstitialBeforeLastFrame(Action<bool> result)
    {
        StartCoroutine(InterstitialThePlayer(result));
    }

    private IEnumerator InterstitialThePlayer(Action<bool> result)
    {
        yield return new WaitForEndOfFrame();
        result?.Invoke(true);
    }

    #endregion

}
