#nullable enable

using System;
using Metica;
using System.Collections.Generic;

namespace Metica.Ads.UnityPlayer
{
    internal class UnityPlayerDelegate : PlatformDelegate
    {
        // AppLovin-specific functionality
        public MeticaApplovinFunctions Max { get; } = new UnityPlayerApplovinFunctions();

        // Mock events - these won't actually fire in editor
        public event Action<MeticaAd> BannerAdLoadSuccess;
        public event Action<MeticaAdError> BannerAdLoadFailed;
        public event Action<MeticaAd> BannerAdClicked;
        public event Action<MeticaAd> BannerAdRevenuePaid;
        public event Action<MeticaAd> MrecAdLoadSuccess;
        public event Action<MeticaAdError> MrecAdLoadFailed;
        public event Action<MeticaAd> MrecAdClicked;
        public event Action<MeticaAd> MrecAdRevenuePaid;
        public event Action<MeticaAd> InterstitialAdLoadSuccess;
        public event Action<MeticaAdError> InterstitialAdLoadFailed;
        public event Action<MeticaAd> InterstitialAdShowSuccess;
        public event Action<MeticaAd, MeticaAdError> InterstitialAdShowFailed;
        public event Action<MeticaAd> InterstitialAdHidden;
        public event Action<MeticaAd> InterstitialAdClicked;
        public event Action<MeticaAd> InterstitialAdRevenuePaid;

        public event Action<MeticaAd> RewardedAdLoadSuccess;
        public event Action<MeticaAdError> RewardedAdLoadFailed;
        public event Action<MeticaAd> RewardedAdShowSuccess;
        public event Action<MeticaAd, MeticaAdError> RewardedAdShowFailed;
        public event Action<MeticaAd> RewardedAdHidden;
        public event Action<MeticaAd> RewardedAdClicked;
        public event Action<MeticaAd> RewardedAdRewarded;
        public event Action<MeticaAd> RewardedAdRevenuePaid;

        public void Initialize(MeticaInitConfig config, MeticaMediationInfo? mediationInfo, Action<MeticaInitResponse>? callback)
        {
            MeticaAdsImpl.Log.D(() => "Mock initialization - always returns HoldoutDueToError");
            var response = new MeticaInitResponse(new MeticaSmartFloors(MeticaUserGroup.HOLDOUT, false), config.UserId);
            callback?.Invoke(response);
        }

        public void SetLogEnabled(bool logEnabled)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetLogEnabled: {logEnabled}");
        }

        public void SetHasUserConsent(bool value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetHasUserConsent: {value}");
        }

        public void SetDoNotSell(bool value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetDoNotSell: {value}");
        }

        public void CreateBanner(string bannerAdUnitId, MeticaAdViewConfiguration configuration)
        {
            MeticaAdsImpl.Log.D(() => "Mock CreateBanner called");
        }

        public void ShowBanner(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock ShowBanner called");
        }

        public void HideBanner(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock HideBanner called");

        }

        public void LoadBanner(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock LoadBanner called");
            var error = new MeticaAdError("Ads not supported in Unity Editor", adUnitId);
            BannerAdLoadFailed?.Invoke(error);
        }

        public void StartBannerAutoRefresh(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock StartBannerAutoRefresh called");
        }

        public void StopBannerAutoRefresh(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock StopBannerAutoRefresh called");
        }

        public void DestroyBanner(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock DestroyBanner called");
        }

        public void SetBannerCustomData(string adUnitId, string? customData)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetBannerCustomData called with customData: {customData}");
        }

        public void SetBannerExtraParameter(string adUnitId, string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetBannerExtraParameterForKey called with key: {key}, value: {value}");
        }

        public void SetBannerLocalExtraParameter(string adUnitId, string key, object? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetBannerLocalExtraParameterForKey called with key: {key}, value: {value}");
        }

        public void SetBannerLocalExtraParameterJson(string adUnitId, string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetBannerLocalExtraParameterJson called with key: {key}, value: {value}");
        }

        public void SetBannerPlacement(string adUnitId, string? placement)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetBannerPlacement called with placement: {placement}");
        }

        public void SetBannerWidth(string adUnitId, float widthDp)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetBannerWidth called with widthDp: {widthDp}");
        }

        public void SetBannerBackgroundColor(string adUnitId, string hexColorCode)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetBannerBackgroundColor called with hexColorCode: {hexColorCode}");
        }

        public void CreateMrec(string adUnitId, MeticaAdViewConfiguration configuration)
        {
            MeticaAdsImpl.Log.D(() => "Mock CreateMrec called");
        }

        public void ShowMrec(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock ShowMrec called");
        }

        public void HideMrec(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock HideMrec called");
        }

        public void LoadMrec(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock LoadMrec called");
            var error = new MeticaAdError("Ads not supported in Unity Editor", adUnitId);
            MrecAdLoadFailed?.Invoke(error);
        }

        public void StartMrecAutoRefresh(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock StartMrecAutoRefresh called");
        }

        public void StopMrecAutoRefresh(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock StopMrecAutoRefresh called");
        }

        public void DestroyMrec(string adUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock DestroyMrec called");
        }

        public void SetMrecCustomData(string adUnitId, string? customData)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetMrecCustomData called with customData: {customData}");
        }

        public void SetMrecExtraParameter(string adUnitId, string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetMrecExtraParameterForKey called with key: {key}, value: {value}");
        }

        public void SetMrecLocalExtraParameter(string adUnitId, string key, object? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetMrecLocalExtraParameterForKey called with key: {key}, value: {value}");
        }

        public void SetMrecLocalExtraParameterJson(string adUnitId, string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetMrecLocalExtraParameterJson called with key: {key}, value: {value}");
        }

        public void SetMrecPlacement(string adUnitId, string placement)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetMrecPlacement called with placement: {placement}");
        }

        public void LoadInterstitial(string interstitialAdUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock LoadInterstitial called");
            var error = new MeticaAdError("Ads not supported in Unity Editor", interstitialAdUnitId);
            InterstitialAdLoadFailed?.Invoke(error);
        }

        public void ShowInterstitial(string interstitialAdUnitId, string? placementId, string? customData)
        {
            MeticaAdsImpl.Log.D(() => $"Mock ShowInterstitial called with placementId={placementId}, customData={customData}");
            var ad = new MeticaAd(interstitialAdUnitId, null, null, placementId, "INTERSTITIAL", null, null);
            var error = new MeticaAdError("Ads not supported in Unity Editor", interstitialAdUnitId);
            InterstitialAdShowFailed?.Invoke(ad, error);
        }

        public void SetInterstitialExtraParameter(string interstitialAdUnitId, string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetInterstitialExtraParameter called with key: {key}, value: {value}");
        }

        public void SetInterstitialLocalExtraParameter(string interstitialAdUnitId, string key, object? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetInterstitialLocalExtraParameter called with key: {key}, value: {value}");
        }

        public bool IsInterstitialReady(string interstitialAdUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock IsInterstitialReady - always returns false");
            return false;
        }

        public void LoadRewarded(string rewardedAdUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock LoadRewarded called");
            var error = new MeticaAdError("Ads not supported in Unity Editor", rewardedAdUnitId);
            RewardedAdLoadFailed?.Invoke(error);
        }

        public void ShowRewarded(string rewardedAdUnitId, string? placementId, string? customData)
        {
            MeticaAdsImpl.Log.D(() => $"Mock ShowRewarded called with placementId={placementId}, customData={customData}");
            var ad = new MeticaAd(rewardedAdUnitId, null, null, placementId, "REWARDED", null, null);
            var error = new MeticaAdError("Ads not supported in Unity Editor", rewardedAdUnitId);
            RewardedAdShowFailed?.Invoke(ad, error);
        }

        public void SetRewardedAdExtraParameter(string rewardedAdUnitId, string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetRewardedAdExtraParameter called with key: {key}, value: {value}");
        }

        public void SetRewardedAdLocalExtraParameter(string rewardedAdUnitId, string key, object? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetRewardedAdLocalExtraParameter called with key: {key}, value: {value}");
        }

        public bool IsRewardedReady(string rewardedAdUnitId)
        {
            MeticaAdsImpl.Log.D(() => "Mock IsRewardedReady - always returns false");
            return false;
        }

#if METICA_ANALYTICS
        public void LogPurchaseEvent(string productId, string currency, double amount, string status, string? errorCode, string? referenceId, Dictionary<string, object>? customPayload)
        {
            MeticaAdsImpl.Log.D(() => "Mock LogPurchaseEvent called");
        }

        public void LogSessionStartEvent(Dictionary<string, object>? customPayload)
        {
            MeticaAdsImpl.Log.D(() => "Mock LogSessionStartEvent called");
        }

        public void LogInstallEvent(Dictionary<string, object>? customPayload)
        {
            MeticaAdsImpl.Log.D(() => "Mock LogInstallEvent called");
        }

        public void LogImpressionEvent(double value, string type, string mediator, string source, string? placement, Dictionary<string, object>? customPayload)
        {
            MeticaAdsImpl.Log.D(() => "Mock LogImpressionEvent called");
        }

        public void LogFullStateUpdateEvent(Dictionary<string, object> attributes)
        {
            MeticaAdsImpl.Log.D(() => "Mock LogFullStateUpdateEvent called");
        }

        public void LogPartialStateUpdateEvent(Dictionary<string, object> attributes)
        {
            MeticaAdsImpl.Log.D(() => "Mock LogPartialStateUpdateEvent called");
        }

        public void LogCustomEvent(string eventName, Dictionary<string, object>? properties)
        {
            MeticaAdsImpl.Log.D(() => "Mock LogCustomEvent called");
        }
#endif
    }
}
