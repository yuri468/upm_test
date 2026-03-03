//  https://stackoverflow.com/questions/55492214/the-annotation-for-nullable-reference-types-should-only-be-used-in-code-within-a

#nullable enable

using System;
using System.Runtime.InteropServices;
using Metica;
using Metica.Ads;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metica.Ads.IOS
{
#if UNITY_IOS || UNITY_IPHONE
internal class IOSDelegate : PlatformDelegate
{
    // AppLovin-specific functionality
    public MeticaApplovinFunctions Max { get; } = new IOSApplovinFunctions();

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

    [DllImport("__Internal")]
    private static extern void ios_setLogEnabled(bool value);
    [DllImport("__Internal")]
    private static extern bool ios_isRewardedReady(string adUnitId);
    [DllImport("__Internal")]
    private static extern bool ios_isInterstitialReady(string adUnitId);
    [DllImport("__Internal")]
    private static extern void ios_setHasUserConsent(bool value);
    [DllImport("__Internal")]
    private static extern void ios_setDoNotSell(bool value);

    [DllImport("__Internal")]
    private static extern void ios_showBannerOrMrecAd(string adUnitId);
    [DllImport("__Internal")]
    private static extern void ios_hideBannerOrMrecAd(string adUnitId);
    [DllImport("__Internal")]
    private static extern void ios_destroyBannerOrMrecAd(string adUnitId);
    [DllImport("__Internal")]
    private static extern void ios_loadBannerOrMrecAd(string adUnitId);
    [DllImport("__Internal")]
    private static extern void ios_startBannerOrMrecAutoRefresh(string adUnitId);
    [DllImport("__Internal")]
    private static extern void ios_stopBannerOrMrecAutoRefresh(string adUnitId);
    [DllImport("__Internal")]
    private static extern void ios_setBannerOrMrecCustomData(string adUnitId, string? customData);
    [DllImport("__Internal")]
    private static extern void ios_setBannerOrMrecExtraParameter(string adUnitId, string key, string? value);
    [DllImport("__Internal")]
    private static extern void ios_setBannerOrMrecLocalExtraParameter(string adUnitId, string key, string? value);
    [DllImport("__Internal")]
    private static extern void ios_setBannerOrMrecPlacement(string adUnitId, string? placement);
    [DllImport("__Internal")]
    private static extern void ios_setBannerWidth(string adUnitId, float widthDp);
    [DllImport("__Internal")]
    private static extern void ios_setBannerBackgroundColor(string adUnitId, string hexColorCode);

    [DllImport("__Internal")]
    private static extern void ios_setInterstitialExtraParameter(string interstitialAdUnitId, string key, string? value);
    [DllImport("__Internal")]
    private static extern void ios_setInterstitialLocalExtraParameter(string interstitialAdUnitId, string key, string? value);
    [DllImport("__Internal")]
    private static extern void ios_setRewardedAdExtraParameter(string rewardedAdUnitId, string key, string? value);
    [DllImport("__Internal")]
    private static extern void ios_setRewardedAdLocalExtraParameter(string rewardedAdUnitId, string key, string? value);

#if METICA_ANALYTICS
    [DllImport("__Internal")]
    private static extern void ios_logPurchaseEvent(string productId, string currency, double amount, string status, string? errorCode, string? referenceId, string? customPayload);
    [DllImport("__Internal")]
    private static extern void ios_logSessionStartEvent(string? customPayload);
    [DllImport("__Internal")]
    private static extern void ios_logInstallEvent(string? customPayload);
    [DllImport("__Internal")]
    private static extern void ios_logImpressionEvent(double value, string type, string mediator, string source, string? placement, string? customPayload);
    [DllImport("__Internal")]
    private static extern void ios_logFullStateUpdateEvent(string attributes);
    [DllImport("__Internal")]
    private static extern void ios_logPartialStateUpdateEvent(string attributes);
    [DllImport("__Internal")]
    private static extern void ios_logCustomEvent(string eventName, string? properties);
#endif

    public void SetLogEnabled(bool logEnabled)
    {
        MeticaAdsImpl.Log.D(() => $"SetLogEnabled called with: {logEnabled}");
        ios_setLogEnabled(logEnabled);
    }

    public void SetHasUserConsent(bool hasUserConsent)
    {
        MeticaAdsImpl.Log.D(() => $"SetHasUserConsent called with: {hasUserConsent}");
        ios_setHasUserConsent(hasUserConsent);
    }

    public void SetDoNotSell(bool doNotSell)
    {
        MeticaAdsImpl.Log.D(() => $"SetDoNotSell called with: {doNotSell}");
        ios_setDoNotSell(doNotSell);
    }

    public void CreateBanner(string adUnitId, MeticaAdViewConfiguration configuration)
    {
        var callback = new IOSBannerCallbackProxy();
        callback.AdLoadSuccess += BannerAdLoadSuccess;
        callback.AdLoadFailed += BannerAdLoadFailed;
        callback.AdClicked += (meticaAd) => BannerAdClicked?.Invoke(meticaAd);
        callback.AdRevenuePaid += (meticaAd) => BannerAdRevenuePaid?.Invoke(meticaAd);

        MeticaAdsImpl.Log.D(() => $"About to call iOS createBanner method");

        if (configuration.Position.HasValue) 
        {
            var position = configuration.Position.Value.ToString();
            callback.CreateBannerOrMrecWithPosition(adUnitId, position, "BANNER");
        } 
        else
        {
            callback.CreateBannerOrMrecWithCoords(adUnitId, configuration.XCoordinate.Value, configuration.YCoordinate.Value, "BANNER");
        }

        MeticaAdsImpl.Log.D(() => $"iOS createBanner method called");
    }

    public void ShowBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS showBanner method");
        ios_showBannerOrMrecAd(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS showBanner method called");
    }

    public void HideBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS hideBanner method");
        ios_hideBannerOrMrecAd(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS hideBanner method called");
    }

    public void DestroyBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS destroyBanner method");
        ios_destroyBannerOrMrecAd(adUnitId);
        IOSBannerCallbackProxy.RemoveInstance(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS destroyBanner method called");
    }

    public void LoadBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS loadBanner method");
        ios_loadBannerOrMrecAd(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS loadBanner method called");
    }

    public void StartBannerAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS startBannerAutoRefresh method");
        ios_startBannerOrMrecAutoRefresh(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS startBannerAutoRefresh method called");
    }

    public void StopBannerAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS stopBannerAutoRefresh method");
        ios_stopBannerOrMrecAutoRefresh(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS stopBannerAutoRefresh method called");
    }

    public void SetBannerCustomData(string adUnitId, string? customData)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setBannerCustomData method");
        ios_setBannerOrMrecCustomData(adUnitId, customData);
        MeticaAdsImpl.Log.D(() => $"iOS setBannerCustomData method called");
    }

    public void SetBannerExtraParameter(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setBannerExtraParameter method");
        ios_setBannerOrMrecExtraParameter(adUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"iOS setBannerExtraParameter method called");
    }

    public void SetBannerLocalExtraParameter(string adUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setBannerLocalExtraParameter method");
        string? json = IOSValueEncoder.ToJson(value);
        ios_setBannerOrMrecLocalExtraParameter(adUnitId, key, json);
        MeticaAdsImpl.Log.D(() => $"iOS setBannerLocalExtraParameter method called");
    }

    public void SetBannerLocalExtraParameterJson(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setBannerLocalExtraParameterJson method");
        ios_setBannerOrMrecLocalExtraParameter(adUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"iOS setBannerLocalExtraParameterJson method called");
    }

    public void SetBannerPlacement(string adUnitId, string? placement)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setBannerPlacement method with placement: {placement}");
        ios_setBannerOrMrecPlacement(adUnitId, placement);
        MeticaAdsImpl.Log.D(() => $"iOS setBannerPlacement method called");
    }

    public void SetBannerWidth(string adUnitId, float widthDp)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setBannerWidth method with widthDp: {widthDp}");
        ios_setBannerWidth(adUnitId, widthDp);
        MeticaAdsImpl.Log.D(() => $"iOS setBannerWidth method called");
    }

    public void SetBannerBackgroundColor(string adUnitId, string hexColorCode)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setBannerBackgroundColor method with hexColorCode: {hexColorCode}");
        ios_setBannerBackgroundColor(adUnitId, hexColorCode);
        MeticaAdsImpl.Log.D(() => $"iOS setBannerBackgroundColor method called");
    }

    public void CreateMrec(string adUnitId, MeticaAdViewConfiguration configuration)
    {
        var callback = new IOSBannerCallbackProxy();
        callback.AdLoadSuccess += MrecAdLoadSuccess;
        callback.AdLoadFailed += MrecAdLoadFailed;
        callback.AdClicked += (meticaAd) => MrecAdClicked?.Invoke(meticaAd);
        callback.AdRevenuePaid += (meticaAd) => MrecAdRevenuePaid?.Invoke(meticaAd);

        MeticaAdsImpl.Log.D(() => $"About to call iOS createMrec method");

        if (configuration.Position.HasValue) 
        {
            var position = configuration.Position.Value.ToString();
            callback.CreateBannerOrMrecWithPosition(adUnitId, position, "MREC");
        } 
        else
        {
            callback.CreateBannerOrMrecWithCoords(adUnitId, configuration.XCoordinate.Value, configuration.YCoordinate.Value, "MREC");
        }

        MeticaAdsImpl.Log.D(() => $"iOS createMrec method called");
    }

    public void ShowMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS showMrec method");
        ios_showBannerOrMrecAd(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS showMrec method called");
    }

    public void HideMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS hideMrec method");
        ios_hideBannerOrMrecAd(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS hideMrec method called");
    }

    public void LoadMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS loadMrec method");
        ios_loadBannerOrMrecAd(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS loadMrec method called");
    }

    public void StartMrecAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS startMrecAutoRefresh method");
        ios_startBannerOrMrecAutoRefresh(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS startMrecAutoRefresh method called");
    }

    public void StopMrecAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS stopMrecAutoRefresh method");
        ios_stopBannerOrMrecAutoRefresh(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS stopMrecAutoRefresh method called");
    }

    public void SetMrecCustomData(string adUnitId, string? customData)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setMrecCustomData method");
        ios_setBannerOrMrecCustomData(adUnitId, customData);
        MeticaAdsImpl.Log.D(() => $"iOS setMrecCustomData method called");
    }

    public void SetMrecExtraParameter(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setMrecExtraParameter method");
        ios_setBannerOrMrecExtraParameter(adUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"iOS setMrecExtraParameter method called");
    }

    public void SetMrecLocalExtraParameter(string adUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setMrecLocalExtraParameter method");
        string? json = IOSValueEncoder.ToJson(value);
        ios_setBannerOrMrecLocalExtraParameter(adUnitId, key, json);
        MeticaAdsImpl.Log.D(() => $"iOS setMrecLocalExtraParameter method called");
    }

    public void SetMrecLocalExtraParameterJson(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setMrecLocalExtraParameterJson method");
        ios_setBannerOrMrecLocalExtraParameter(adUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"iOS setMrecLocalExtraParameterJson method called");
    }

    public void SetMrecPlacement(string adUnitId, string? placement)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setMrecPlacement method with placement: {placement}");
        ios_setBannerOrMrecPlacement(adUnitId, placement);
        MeticaAdsImpl.Log.D(() => $"iOS setMrecPlacement method called");
    }

    public void DestroyMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS destroyMrec method");
        ios_destroyBannerOrMrecAd(adUnitId);
        IOSBannerCallbackProxy.RemoveInstance(adUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS destroyMrec method called");
    }

    public void Initialize(MeticaInitConfig config, MeticaMediationInfo? mediationInfo, Action<MeticaInitResponse>? callback)
    {
        var iosCallback = new IOSInitializeCallback(callback);
        iosCallback.InitializeSDK(config.ApiKey, config.AppId, config.UserId, mediationInfo?.Key);
    }

    // Interstitial methods
    public void LoadInterstitial(string interstitialAdUnitId)
    {
        var callback = new IOSLoadCallback();
        callback.AdLoadSuccess += (meticaAd) => InterstitialAdLoadSuccess?.Invoke(meticaAd);
        callback.AdLoadFailed += (error) => InterstitialAdLoadFailed?.Invoke(error);
            
        MeticaAdsImpl.Log.D(() => $"About to call iOS loadInterstitial method");
        callback.LoadInterstitial(interstitialAdUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS loadInterstitial method called");
    }

    public void ShowInterstitial(string interstitialAdUnitId, string? placementId, string? customData)
    {
        var callback = new IOSShowCallbackProxy();

        callback.AdShowSuccess += (ad) => InterstitialAdShowSuccess?.Invoke(ad);
        callback.AdShowFailed += (ad, error) => InterstitialAdShowFailed?.Invoke(ad, error);
        callback.AdHidden += (ad) => InterstitialAdHidden?.Invoke(ad);
        callback.AdClicked += (ad) => InterstitialAdClicked?.Invoke(ad);
        callback.AdRevenuePaid += (ad) => InterstitialAdRevenuePaid?.Invoke(ad);

        MeticaAdsImpl.Log.D(() => $"About to call iOS showInterstitial method");
        callback.ShowInterstitial(interstitialAdUnitId, placementId, customData);
        MeticaAdsImpl.Log.D(() => $"iOS showInterstitial method called");
    }

    public void SetInterstitialExtraParameter(string interstitialAdUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setInterstitialExtraParameter method");
        ios_setInterstitialExtraParameter(interstitialAdUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"iOS setInterstitialExtraParameter method called");
    }

    public void SetInterstitialLocalExtraParameter(string interstitialAdUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setInterstitialLocalExtraParameter method");
        string? json = IOSValueEncoder.ToJson(value);
        ios_setInterstitialLocalExtraParameter(interstitialAdUnitId, key, json);
        MeticaAdsImpl.Log.D(() => $"iOS setInterstitialLocalExtraParameter method called");
    }

    public bool IsInterstitialReady(string interstitialAdUnitId)
    {
        return ios_isInterstitialReady(interstitialAdUnitId);
    }

    // Rewarded methods
    public void LoadRewarded(string rewardedAdUnitId)
    {
        var callback = new IOSLoadCallback();
        callback.AdLoadSuccess += (meticaAd) => RewardedAdLoadSuccess?.Invoke(meticaAd);
        callback.AdLoadFailed += (error) => RewardedAdLoadFailed?.Invoke(error);
     
        MeticaAdsImpl.Log.D(() => $"About to call iOS loadRewarded method");
        callback.LoadRewarded(rewardedAdUnitId);
        MeticaAdsImpl.Log.D(() => $"iOS loadRewarded method called");
    }

    public void ShowRewarded(string rewardedAdUnitId, string? placementId, string? customData)
    {
        var callback = new IOSShowCallbackProxy();
        callback.AdShowSuccess += (ad) => RewardedAdShowSuccess?.Invoke(ad);
        callback.AdShowFailed += (ad, error) => RewardedAdShowFailed?.Invoke(ad, error);
        callback.AdHidden += (ad) => RewardedAdHidden?.Invoke(ad);
        callback.AdClicked += (ad) => RewardedAdClicked?.Invoke(ad);
        callback.AdRewarded += (ad) => RewardedAdRewarded?.Invoke(ad);
        callback.AdRevenuePaid += (ad) => RewardedAdRevenuePaid?.Invoke(ad);
    
        MeticaAdsImpl.Log.D(() => $"About to call iOS showRewarded method");
        callback.ShowRewarded(rewardedAdUnitId, placementId, customData);
        MeticaAdsImpl.Log.D(() => $"iOS showRewarded method called");
    }

    public void SetRewardedAdExtraParameter(string rewardedAdUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setRewardedAdExtraParameter method");
        ios_setRewardedAdExtraParameter(rewardedAdUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"iOS setRewardedAdExtraParameter method called");
    }

    public void SetRewardedAdLocalExtraParameter(string rewardedAdUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS setRewardedAdLocalExtraParameter method");
        string? json = IOSValueEncoder.ToJson(value);
        ios_setRewardedAdLocalExtraParameter(rewardedAdUnitId, key, json);
        MeticaAdsImpl.Log.D(() => $"iOS setRewardedAdLocalExtraParameter method called");
    }

    public bool IsRewardedReady(string rewardedAdUnitId)
    {
        return ios_isRewardedReady(rewardedAdUnitId);
    }

#if METICA_ANALYTICS
    public void LogPurchaseEvent(string productId, string currency, double amount, string status, string? errorCode, string? referenceId, Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS logPurchaseEvent method");
        string? customPayloadJson = customPayload != null ? JsonConvert.SerializeObject(customPayload) : null;
        ios_logPurchaseEvent(productId, currency, amount, status, errorCode, referenceId, customPayloadJson);
        MeticaAdsImpl.Log.D(() => $"iOS logPurchaseEvent method called");
    }

    public void LogSessionStartEvent(Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS logSessionStartEvent method");
        string? customPayloadJson = customPayload != null ? JsonConvert.SerializeObject(customPayload) : null;
        ios_logSessionStartEvent(customPayloadJson);
        MeticaAdsImpl.Log.D(() => $"iOS logSessionStartEvent method called");
    }

    public void LogInstallEvent(Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS logInstallEvent method");
        string? customPayloadJson = customPayload != null ? JsonConvert.SerializeObject(customPayload) : null;
        ios_logInstallEvent(customPayloadJson);
        MeticaAdsImpl.Log.D(() => $"iOS logInstallEvent method called");
    }

    public void LogImpressionEvent(double value, string type, string mediator, string source, string? placement, Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS logImpressionEvent method");
        string? customPayloadJson = customPayload != null ? JsonConvert.SerializeObject(customPayload) : null;
        ios_logImpressionEvent(value, type, mediator, source, placement, customPayloadJson);
        MeticaAdsImpl.Log.D(() => $"iOS logImpressionEvent method called");
    }

    public void LogFullStateUpdateEvent(Dictionary<string, object> attributes)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS logFullStateUpdateEvent method");
        string attributesJson = JsonConvert.SerializeObject(attributes);
        ios_logFullStateUpdateEvent(attributesJson);
        MeticaAdsImpl.Log.D(() => $"iOS logFullStateUpdateEvent method called");
    }

    public void LogPartialStateUpdateEvent(Dictionary<string, object> attributes)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS logPartialStateUpdateEvent method");
        string attributesJson = JsonConvert.SerializeObject(attributes);
        ios_logPartialStateUpdateEvent(attributesJson);
        MeticaAdsImpl.Log.D(() => $"iOS logPartialStateUpdateEvent method called");
    }

    public void LogCustomEvent(string eventName, Dictionary<string, object>? properties)
    {
        MeticaAdsImpl.Log.D(() => $"About to call iOS logCustomEvent method");
        string? propertiesJson = properties != null ? JsonConvert.SerializeObject(properties) : null;
        ios_logCustomEvent(eventName, propertiesJson);
        MeticaAdsImpl.Log.D(() => $"iOS logCustomEvent method called");
    }
#endif
}
#endif
}