//  https://stackoverflow.com/questions/55492214/the-annotation-for-nullable-reference-types-should-only-be-used-in-code-within-a

#nullable enable

using System;
using Metica;
using UnityEngine;
using System.Collections.Generic;

namespace Metica.Ads.Android
{
internal class AndroidDelegate : PlatformDelegate
{
    public AndroidDelegate(AndroidJavaClass unityBridgeClass)
    {
        _unityBridgeAndroidClass = unityBridgeClass;
    }


    private readonly AndroidJavaClass _unityBridgeAndroidClass;

    // AppLovin-specific functionality
    //
    // IMPORTANT: Max is lazily initialized to avoid accessing the native MeticaSdk.Ads before initialization.
    // If we eagerly initialize Max in the constructor, it will call UnityBridge.Mediation.getMax(),
    // which internally accesses MeticaSdk.Ads. This throws IllegalStateException if MeticaSdk.initialize()
    // hasn't been called yet on the native side.
    //
    // Lazy initialization ensures Max is only created when first accessed, which happens after
    // MeticaSdk.InitializeAsync() completes successfully.
    private readonly Lazy<MeticaApplovinFunctions> _max = new Lazy<MeticaApplovinFunctions>(() =>
    {
        var mediationClass = new AndroidJavaClass("com.metica.unity_bridge.UnityBridge$Mediation");
        var maxObject = mediationClass.CallStatic<AndroidJavaObject>("getMax");
        return new AndroidApplovinFunctions(maxObject);
    });

    public MeticaApplovinFunctions Max => _max.Value;

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

    public void SetLogEnabled(bool logEnabled)
    {
        MeticaAdsImpl.Log.D(() => $"SetLogEnabled called with: {logEnabled}");

        _unityBridgeAndroidClass.CallStatic("setLogEnabled", logEnabled);
    }

    public void SetHasUserConsent(bool hasUserConsent)
    {
        MeticaAdsImpl.Log.D(() => $"SetHasUserConsent called with: {hasUserConsent}");

        _unityBridgeAndroidClass.CallStatic("setHasUserConsent", hasUserConsent);
    }

    public void SetDoNotSell(bool doNotSell)
    {
        MeticaAdsImpl.Log.D(() => $"SetDoNotSell called with: {doNotSell}");

        _unityBridgeAndroidClass.CallStatic("setDoNotSell", doNotSell);
    }

    public void Initialize(MeticaInitConfig config, MeticaMediationInfo? mediationInfo, Action<MeticaInitResponse>? callback)
    {
        var callbackProxy = new InitCallbackProxy(callback);

        _unityBridgeAndroidClass.CallStatic("initialize",
            ToInitConfigJson(config),
            mediationInfo != null ? ToMediationInfoJson(mediationInfo) : null,
            callbackProxy);
    }

    private static string ToInitConfigJson(MeticaInitConfig config)
    {
        var dict = new Dictionary<string, object?>
        {
            ["apiKey"] = config.ApiKey,
            ["appId"] = config.AppId,
            ["userId"] = config.UserId ?? string.Empty,
        };

        return Newtonsoft.Json.JsonConvert.SerializeObject(dict);
    }

    private static string ToMediationInfoJson(MeticaMediationInfo mediationInfo)
    {
        var dict = new Dictionary<string, object?>
        {
            ["mediationType"] = mediationInfo.MediationType.ToString(),
            ["key"] = mediationInfo.Key,
            ["extraInformation"] = mediationInfo.ExtraInformation,
        };
        return Newtonsoft.Json.JsonConvert.SerializeObject(dict);
    }

    // Banner methods
    public void CreateBanner(string adUnitId, MeticaAdViewConfiguration configuration)
    {
        var callback = new BannerCallbackProxy();
        callback.AdLoadSuccess += BannerAdLoadSuccess;
        callback.AdLoadFailed += BannerAdLoadFailed;
        callback.AdClicked += (meticaAd) => BannerAdClicked?.Invoke(meticaAd);
        callback.AdRevenuePaid += (meticaAd) => BannerAdRevenuePaid?.Invoke(meticaAd);

        MeticaAdsImpl.Log.D(() => $"About to call Android createBanner method");

        if (configuration.Position.HasValue)
        {
            var positionName = configuration.Position.ToString();
            _unityBridgeAndroidClass.CallStatic("createBannerWithPosition", adUnitId, positionName, callback);
        }
        else
        {
            _unityBridgeAndroidClass.CallStatic("createBannerWithCoords", adUnitId, configuration.XCoordinate.Value, configuration.YCoordinate.Value, callback);
        }

        MeticaAdsImpl.Log.D(() => $"Android createBanner method called");
    }

    public void ShowBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android showBanner method");
        _unityBridgeAndroidClass.CallStatic("showBanner", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android showBanner method called");
    }

    public void HideBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android hideBanner method");
        _unityBridgeAndroidClass.CallStatic("hideBanner", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android hideBanner method called");
    }

    public void LoadBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android loadBanner method");
        _unityBridgeAndroidClass.CallStatic("loadBanner", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android loadBanner method called");
    }

    public void StartBannerAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android startBannerAutoRefresh method");
        _unityBridgeAndroidClass.CallStatic("startBannerAutoRefresh", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android startBannerAutoRefresh method called");
    }

    public void StopBannerAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android stopBannerAutoRefresh method");
        _unityBridgeAndroidClass.CallStatic("stopBannerAutoRefresh", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android stopBannerAutoRefresh method called");
    }

    public void DestroyBanner(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android destroyBanner method");
        _unityBridgeAndroidClass.CallStatic("destroyBanner", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android destroyBanner method called");
    }

    public void SetBannerCustomData(string adUnitId, string? customData)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setBannerCustomData method with customData: {customData}");
        _unityBridgeAndroidClass.CallStatic("setBannerCustomData", adUnitId, customData);
        MeticaAdsImpl.Log.D(() => $"Android setBannerCustomData method called");
    }

    public void SetBannerExtraParameter(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setBannerExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setBannerExtraParameter", adUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"Android setBannerExtraParameter method called");
    }

    public void SetBannerLocalExtraParameter(string adUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setBannerLocalExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setBannerLocalExtraParameter", adUnitId, key, value);    
        MeticaAdsImpl.Log.D(() => $"Android setBannerLocalExtraParameter method called");
    }

    public void SetBannerLocalExtraParameterJson(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setBannerLocalExtraParameterJson method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setBannerLocalExtraParameter", adUnitId, key, value);    
        MeticaAdsImpl.Log.D(() => $"Android setBannerLocalExtraParameterJson method called");
    }

    public void SetBannerPlacement(string adUnitId, string? placement)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setBannerPlacement method with placement: {placement}");
        _unityBridgeAndroidClass.CallStatic("setBannerPlacement", adUnitId, placement);
        MeticaAdsImpl.Log.D(() => $"Android setBannerPlacement method called");
    }

    public void SetBannerWidth(string adUnitId, float widthDp)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setBannerWidth method with widthDp: {widthDp}");
        _unityBridgeAndroidClass.CallStatic("setBannerWidth", adUnitId, widthDp);
        MeticaAdsImpl.Log.D(() => $"Android setBannerWidth method called");
    }

    public void SetBannerBackgroundColor(string adUnitId, string hexColorCode)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setBannerBackgroundColor method with hexColorCode: {hexColorCode}");
        _unityBridgeAndroidClass.CallStatic("setBannerBackgroundColor", adUnitId, hexColorCode);
        MeticaAdsImpl.Log.D(() => $"Android setBannerBackgroundColor method called");
    }

    // MREC methods
    public void CreateMrec(string adUnitId, MeticaAdViewConfiguration configuration)
    {
        var callback = new BannerCallbackProxy();
        callback.AdLoadSuccess += MrecAdLoadSuccess;
        callback.AdLoadFailed += MrecAdLoadFailed;
        callback.AdClicked += (meticaAd) => MrecAdClicked?.Invoke(meticaAd);
        callback.AdRevenuePaid += (meticaAd) => MrecAdRevenuePaid?.Invoke(meticaAd);

        MeticaAdsImpl.Log.D(() => $"About to call Android createMrec method");

        if (configuration.Position.HasValue)
        {
            var positionName = configuration.Position.ToString();
            _unityBridgeAndroidClass.CallStatic("createMrecWithPosition", adUnitId, positionName, callback);
        }
        else
        {
            _unityBridgeAndroidClass.CallStatic("createMrecWithCoords", adUnitId, configuration.XCoordinate.Value, configuration.YCoordinate.Value, callback);
        }

        MeticaAdsImpl.Log.D(() => $"Android createMrec method called");
    }

    public void ShowMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android showMrec method");
        _unityBridgeAndroidClass.CallStatic("showMrec", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android showMrec method called");
    }

    public void HideMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android hideMrec method");
        _unityBridgeAndroidClass.CallStatic("hideMrec", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android hideMrec method called");
    }

    public void LoadMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android loadMrec method");
        _unityBridgeAndroidClass.CallStatic("loadMrec", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android loadMrec method called");
    }

    public void StartMrecAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android startMrecAutoRefresh method");
        _unityBridgeAndroidClass.CallStatic("startMrecAutoRefresh", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android startMrecAutoRefresh method called");
    }

    public void StopMrecAutoRefresh(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android stopMrecAutoRefresh method");
        _unityBridgeAndroidClass.CallStatic("stopMrecAutoRefresh", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android stopMrecAutoRefresh method called");
    }

    public void DestroyMrec(string adUnitId)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android destroyMrec method");
        _unityBridgeAndroidClass.CallStatic("destroyMrec", adUnitId);
        MeticaAdsImpl.Log.D(() => $"Android destroyMrec method called");
    }

    public void SetMrecCustomData(string adUnitId, string? customData)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setMrecCustomData method with customData: {customData}");
        _unityBridgeAndroidClass.CallStatic("setMrecCustomData", adUnitId, customData);
        MeticaAdsImpl.Log.D(() => $"Android setMrecCustomData method called");
    }

    public void SetMrecExtraParameter(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setMrecExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setMrecExtraParameter", adUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"Android setMrecExtraParameter method called");
    }

    public void SetMrecLocalExtraParameter(string adUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setMrecLocalExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setMrecLocalExtraParameter", adUnitId, key, value);    
        MeticaAdsImpl.Log.D(() => $"Android setMrecLocalExtraParameter method called");
    }

    public void SetMrecLocalExtraParameterJson(string adUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setMrecLocalExtraParameterJson method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setMrecLocalExtraParameter", adUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"Android setMrecLocalExtraParameterJson method called");
    }

    public void SetMrecPlacement(string adUnitId, string? placement)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setMrecPlacement method with placement: {placement}");
        _unityBridgeAndroidClass.CallStatic("setMrecPlacement", adUnitId, placement);
        MeticaAdsImpl.Log.D(() => $"Android setMrecPlacement method called");
    }

    // Interstitial methods
    public void LoadInterstitial(string interstitialAdUnitId)
    {
        var callback = new LoadCallbackProxy();

        // Wire up all events
        callback.AdLoadSuccess += InterstitialAdLoadSuccess;
        callback.AdLoadFailed += InterstitialAdLoadFailed;

        MeticaAdsImpl.Log.D(() => $"About to call Android loadInterstitial method");
        _unityBridgeAndroidClass.CallStatic("loadInterstitial", interstitialAdUnitId, callback);
        MeticaAdsImpl.Log.D(() => $"Android loadInterstitial method called");
    }

    public void ShowInterstitial(string interstitialAdUnitId, string? placementId, string? customData)
    {
        var callback = new ShowCallbackProxy();

        // Wire up all events - now using MeticaAd objects directly
        callback.AdShowSuccess += (meticaAd) => InterstitialAdShowSuccess?.Invoke(meticaAd);
        callback.AdShowFailed += (meticaAd, error) => InterstitialAdShowFailed?.Invoke(meticaAd, error);
        callback.AdHidden += (meticaAd) => InterstitialAdHidden?.Invoke(meticaAd);
        callback.AdClicked += (meticaAd) => InterstitialAdClicked?.Invoke(meticaAd);
        callback.AdRevenuePaid += (meticaAd) => InterstitialAdRevenuePaid?.Invoke(meticaAd);

        MeticaAdsImpl.Log.D(() => $"About to call Android showInterstitial method with placementId={placementId}, customData={customData}");
        _unityBridgeAndroidClass.CallStatic("showInterstitial", interstitialAdUnitId, placementId, customData, callback);
        MeticaAdsImpl.Log.D(() => $"Android showInterstitial method called");
    }

    public void SetInterstitialExtraParameter(string interstitialAdUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setInterstitialExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setInterstitialExtraParameter", interstitialAdUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"Android setInterstitialExtraParameter method called");
    }

    public void SetInterstitialLocalExtraParameter(string interstitialAdUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setInterstitialLocalExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setInterstitialLocalExtraParameter", interstitialAdUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"Android setInterstitialLocalExtraParameter method called");
    }

    public bool IsInterstitialReady(string interstitialAdUnitId)
    {
        return _unityBridgeAndroidClass.CallStatic<bool>("isInterstitialReady", interstitialAdUnitId);
    }

    // Rewarded methods
    public void LoadRewarded(string rewardedAdUnitId)
    {
        var callback = new LoadCallbackProxy();

        // Wire up all events - now using MeticaAd and string directly
        callback.AdLoadSuccess += (meticaAd) => RewardedAdLoadSuccess?.Invoke(meticaAd);
        callback.AdLoadFailed += (error) => RewardedAdLoadFailed?.Invoke(error);

        MeticaAdsImpl.Log.D(() => $"About to call Android loadRewarded method");
        _unityBridgeAndroidClass.CallStatic("loadRewarded", rewardedAdUnitId, callback);
        MeticaAdsImpl.Log.D(() => $"Android loadRewarded method called");
    }

    public void ShowRewarded(string rewardedAdUnitId, string? placementId, string? customData)
    {
        var callback = new ShowCallbackProxy();

        // Wire up all events - now using MeticaAd objects directly
        callback.AdShowSuccess += (meticaAd) => RewardedAdShowSuccess?.Invoke(meticaAd);
        callback.AdShowFailed += (meticaAd, error) => RewardedAdShowFailed?.Invoke(meticaAd, error);
        callback.AdHidden += (meticaAd) => RewardedAdHidden?.Invoke(meticaAd);
        callback.AdClicked += (meticaAd) => RewardedAdClicked?.Invoke(meticaAd);
        callback.AdRewarded += (meticaAd) => RewardedAdRewarded?.Invoke(meticaAd);
        callback.AdRevenuePaid += (meticaAd) => RewardedAdRevenuePaid?.Invoke(meticaAd);

        MeticaAdsImpl.Log.D(() => $"About to call Android showRewarded method with placementId={placementId}, customData={customData}");
        _unityBridgeAndroidClass.CallStatic("showRewarded", rewardedAdUnitId, placementId, customData, callback);
        MeticaAdsImpl.Log.D(() => $"Android showRewarded method called");
    }

    public void SetRewardedAdExtraParameter(string rewardedAdUnitId, string key, string? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setRewardedAdExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setRewardedAdExtraParameter", rewardedAdUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"Android setRewardedAdExtraParameter method called");
    }

    public void SetRewardedAdLocalExtraParameter(string rewardedAdUnitId, string key, object? value)
    {
        MeticaAdsImpl.Log.D(() => $"About to call Android setRewardedAdLocalExtraParameter method with key: {key}, value: {value}");
        _unityBridgeAndroidClass.CallStatic("setRewardedAdLocalExtraParameter", rewardedAdUnitId, key, value);
        MeticaAdsImpl.Log.D(() => $"Android setRewardedAdLocalExtraParameter method called");
    }

    public bool IsRewardedReady(string rewardedAdUnitId)
    {
        return _unityBridgeAndroidClass.CallStatic<bool>("isRewardedReady", rewardedAdUnitId);
    }

#if METICA_ANALYTICS
    public void LogPurchaseEvent(string productId, string currency, double amount, string status, string? errorCode, string? referenceId, Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"LogPurchaseEvent: productId={productId}, currency={currency}, amount={amount}, status={status}, errorCode={errorCode}, referenceId={referenceId}");
        _unityBridgeAndroidClass.CallStatic("logPurchaseEvent", productId, currency, amount, status, errorCode ?? "", referenceId ?? "", ToJson(customPayload));
    }

    public void LogSessionStartEvent(Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"LogSessionStartEvent: customPayload={customPayload?.Count ?? 0} items");
        _unityBridgeAndroidClass.CallStatic("logSessionStartEvent", ToJson(customPayload));
    }

    public void LogInstallEvent(Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"LogInstallEvent: customPayload={customPayload?.Count ?? 0} items");
        _unityBridgeAndroidClass.CallStatic("logInstallEvent", ToJson(customPayload));
    }

    public void LogImpressionEvent(double value, string type, string mediator, string source, string? placement, Dictionary<string, object>? customPayload)
    {
        MeticaAdsImpl.Log.D(() => $"LogImpressionEvent: value={value}, type={type}, mediator={mediator}, source={source}, placement={placement}");
        _unityBridgeAndroidClass.CallStatic("logImpressionEvent", value, type, mediator, source, placement ?? "", ToJson(customPayload));
    }

    public void LogFullStateUpdateEvent(Dictionary<string, object> attributes)
    {
        MeticaAdsImpl.Log.D(() => $"LogFullStateUpdateEvent: attributes={attributes.Count} items");
        _unityBridgeAndroidClass.CallStatic("logFullStateUpdateEvent", ToJson(attributes));
    }

    public void LogPartialStateUpdateEvent(Dictionary<string, object> attributes)
    {
        MeticaAdsImpl.Log.D(() => $"LogPartialStateUpdateEvent: attributes={attributes.Count} items");
        _unityBridgeAndroidClass.CallStatic("logPartialStateUpdateEvent", ToJson(attributes));
    }

    public void LogCustomEvent(string eventName, Dictionary<string, object>? properties)
    {
        MeticaAdsImpl.Log.D(() => $"LogCustomEvent: eventName={eventName}, properties={properties?.Count ?? 0} items");
        _unityBridgeAndroidClass.CallStatic("logCustomEvent", eventName, ToJson(properties));
    }

    private static string? ToJson(Dictionary<string, object>? dict)
    {
        return dict == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(dict);
    }
#endif
}
}
