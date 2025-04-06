using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static EventDefine;

public class GameShowAdsManager : MonoBehaviour
{
    public static GameShowAdsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(DisplayBannerWithDelay());
    }

    private IEnumerator DisplayBannerWithDelay()
    {
        yield return new WaitForSeconds(1f);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    private void OnEnable()
    {
        EventDispatcher.Add<EventDefine.OnAdsGame>(OnAdsGame);
    }

    private void OnDisable()
    {
        EventDispatcher.Remove<EventDefine.OnAdsGame>(OnAdsGame);
    }

    private void OnAdsGame(IEventParam param)
    {
        // Ẩn Banner Ads trước khi hiển thị Interstitial Ads
        AdsManager.Instance.bannerAds.HideBannerAd();

        // Hiển thị Interstitial Ads
        AdsManager.Instance.interstitialAds.ShowInterstitialAd();

        // Sau khi Interstitial Ads kết thúc, hiển thị lại Banner Ads
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }
}
