#nullable enable

using System;
using System.Collections.Generic;
using Metica;

// TODO: move to parent, as it no longer just pertains to ads
namespace Metica.Ads
{
internal interface PlatformDelegate
{
    // AppLovin-specific functionality
    MeticaApplovinFunctions Max { get; }

    // Events for banner ad lifecycle callbacks
    public event Action<MeticaAd> BannerAdLoadSuccess;
    public event Action<MeticaAdError> BannerAdLoadFailed;
    public event Action<MeticaAd> BannerAdClicked;
    public event Action<MeticaAd> BannerAdRevenuePaid;

    // Events for MREC ad lifecycle callbacks
    public event Action<MeticaAd> MrecAdLoadSuccess;
    public event Action<MeticaAdError> MrecAdLoadFailed;
    public event Action<MeticaAd> MrecAdClicked;
    public event Action<MeticaAd> MrecAdRevenuePaid;

    // Events for interstitial ad lifecycle callbacks
    public event Action<MeticaAd> InterstitialAdLoadSuccess;
    public event Action<MeticaAdError> InterstitialAdLoadFailed;
    public event Action<MeticaAd> InterstitialAdShowSuccess;
    public event Action<MeticaAd, MeticaAdError> InterstitialAdShowFailed;
    public event Action<MeticaAd> InterstitialAdHidden;
    public event Action<MeticaAd> InterstitialAdClicked;
    public event Action<MeticaAd> InterstitialAdRevenuePaid;
            
    // Events for rewarded ad lifecycle callbacks
    public event Action<MeticaAd> RewardedAdLoadSuccess;
    public event Action<MeticaAdError> RewardedAdLoadFailed;
    public event Action<MeticaAd> RewardedAdShowSuccess;
    public event Action<MeticaAd, MeticaAdError> RewardedAdShowFailed;
    public event Action<MeticaAd> RewardedAdHidden;
    public event Action<MeticaAd> RewardedAdClicked;
    public event Action<MeticaAd> RewardedAdRewarded;
    public event Action<MeticaAd> RewardedAdRevenuePaid;

    void Initialize(
        MeticaInitConfig config,
        MeticaMediationInfo? mediationInfo,
        Action<MeticaInitResponse>? callback);
    void SetLogEnabled(bool logEnabled);
    void SetHasUserConsent(bool hasUserConsent);
    void SetDoNotSell(bool doNotSell);
            
    // Banner methods
    void CreateBanner(string bannerAdUnitId, MeticaAdViewConfiguration configuration);
    void ShowBanner(string adUnitId);
    void HideBanner(string adUnitId);
    void LoadBanner(string adUnitId);
    void StartBannerAutoRefresh(string adUnitId);
    void StopBannerAutoRefresh(string adUnitId);
    void DestroyBanner(string adUnitId);
    void SetBannerCustomData(string adUnitId, string? customData);
    void SetBannerExtraParameter(string adUnitId, string key, string? value);
    void SetBannerLocalExtraParameter(string adUnitId, string key, object? value);
    void SetBannerLocalExtraParameterJson(string adUnitId, string key, string? value);
    void SetBannerPlacement(string adUnitId, string? placement);
    void SetBannerWidth(string adUnitId, float widthDp);
    void SetBannerBackgroundColor(string adUnitId, string hexColorCode);

    // MREC methods
    void CreateMrec(string adUnitId, MeticaAdViewConfiguration configuration);
    void ShowMrec(string adUnitId);
    void HideMrec(string adUnitId);
    void LoadMrec(string adUnitId);
    void StartMrecAutoRefresh(string adUnitId);
    void StopMrecAutoRefresh(string adUnitId);
    void DestroyMrec(string adUnitId);
    void SetMrecCustomData(string adUnitId, string? customData);
    void SetMrecExtraParameter(string adUnitId, string key, string? value);
    void SetMrecLocalExtraParameter(string adUnitId, string key, object? value);
    void SetMrecLocalExtraParameterJson(string adUnitId, string key, string? value);
    void SetMrecPlacement(string adUnitId, string? placement);

    // Interstitial methods
    void LoadInterstitial(string interstitialAdUnitId);
    void ShowInterstitial(string interstitialAdUnitId, string? placementId, string? customData);
    void SetInterstitialExtraParameter(string interstitialAdUnitId, string key, string? value);
    void SetInterstitialLocalExtraParameter(string interstitialAdUnitId, string key, object? value);
    bool IsInterstitialReady(string interstitialAdUnitId);

    // Rewarded methods
    void LoadRewarded(string rewardedAdUnitId);
    void ShowRewarded(string rewardedAdUnitId, string? placementId, string? customData);
    void SetRewardedAdExtraParameter(string rewardedAdUnitId, string key, string? value);
    void SetRewardedAdLocalExtraParameter(string rewardedAdUnitId, string key, object? value);
    bool IsRewardedReady(string rewardedAdUnitId);

#if METICA_ANALYTICS
    // Analytics methods
    void LogPurchaseEvent(string productId, string currency, double amount, string status, string? errorCode, string? referenceId, Dictionary<string, object>? customPayload);
    void LogSessionStartEvent(Dictionary<string, object>? customPayload);
    void LogInstallEvent(Dictionary<string, object>? customPayload);
    void LogImpressionEvent(double value, string type, string mediator, string source, string? placement, Dictionary<string, object>? customPayload);
    void LogFullStateUpdateEvent(Dictionary<string, object> attributes);
    void LogPartialStateUpdateEvent(Dictionary<string, object> attributes);
    void LogCustomEvent(string eventName, Dictionary<string, object>? properties);
#endif
}
}
