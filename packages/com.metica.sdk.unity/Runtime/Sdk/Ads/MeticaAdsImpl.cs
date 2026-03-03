#nullable enable

using Metica.Core;

// ReSharper disable once CheckNamespace
namespace Metica.Ads
{
internal sealed class MeticaAdsImpl : MeticaAds
{
    #region Fields

    private readonly PlatformDelegate _platformDelegate;
   
    // TODO: Create it in MeticaSdk and then pass it via constructor.
    internal static ILog Log => Registry.Resolve<ILog>();

    public MeticaApplovinFunctions Max => _platformDelegate.Max;

    #endregion Fields

    #region Initialization

    internal MeticaAdsImpl(PlatformDelegate platformDelegate)
    {
        _platformDelegate = platformDelegate;
        SetupCallbacks();
    }

    public void SetHasUserConsent(bool hasUserConsent)
    {
        _platformDelegate.SetHasUserConsent(hasUserConsent);
    }

    public void SetDoNotSell(bool doNotSell)
    {
        _platformDelegate.SetDoNotSell(doNotSell);
    }

    #endregion Initialization

    #region Banner Ads

    public void CreateBanner(string bannerAdUnitId, MeticaAdViewConfiguration configuration)
    {
        _platformDelegate.CreateBanner(bannerAdUnitId, configuration);
    }

    // Banner ad methods
    public void ShowBanner(string adUnitId)
    {
        _platformDelegate.ShowBanner(adUnitId);
    }

    public void HideBanner(string adUnitId)
    {
        _platformDelegate.HideBanner(adUnitId);
    }

    public void LoadBanner(string adUnitId)
    {
        _platformDelegate.LoadBanner(adUnitId);
    }

    public void StartBannerAutoRefresh(string adUnitId)
    {
        _platformDelegate.StartBannerAutoRefresh(adUnitId);
    }

    public void StopBannerAutoRefresh(string adUnitId)
    {
        _platformDelegate.StopBannerAutoRefresh(adUnitId);
    }

    public void DestroyBanner(string adUnitId)
    {
        _platformDelegate.DestroyBanner(adUnitId);
    }

    public void SetBannerCustomData(string adUnitId, string? customData)
    {
        _platformDelegate.SetBannerCustomData(adUnitId, customData);
    }

    public void SetBannerExtraParameter(string adUnitId, string key, string? value)
    {
        _platformDelegate.SetBannerExtraParameter(adUnitId, key, value);
    }

    public void SetBannerLocalExtraParameter(string adUnitId, string key, object? value)
    {
        _platformDelegate.SetBannerLocalExtraParameter(adUnitId, key, value);
    }

    public void SetBannerLocalExtraParameterJson(string adUnitId, string key, string? value)
    {
        _platformDelegate.SetBannerLocalExtraParameterJson(adUnitId, key, value);
    }

    public void SetBannerPlacement(string adUnitId, string? placement)
    {
        _platformDelegate.SetBannerPlacement(adUnitId, placement);
    }

    public void SetBannerWidth(string adUnitId, float widthDp)
    {
        _platformDelegate.SetBannerWidth(adUnitId, widthDp);
    }

    public void SetBannerBackgroundColor(string adUnitId, string hexColorCode)
    {
        _platformDelegate.SetBannerBackgroundColor(adUnitId, hexColorCode);
    }

    #endregion Banner Ads

    #region MREC Ads

    public void CreateMrec(string adUnitId, MeticaAdViewConfiguration configuration)
    {
        _platformDelegate.CreateMrec(adUnitId, configuration);
    }

    public void ShowMrec(string adUnitId)
    {
        _platformDelegate.ShowMrec(adUnitId);
    }

    public void HideMrec(string adUnitId)
    {
        _platformDelegate.HideMrec(adUnitId);
    }

    public void LoadMrec(string adUnitId)
    {
        _platformDelegate.LoadMrec(adUnitId);
    }

    public void StartMrecAutoRefresh(string adUnitId)
    {
        _platformDelegate.StartMrecAutoRefresh(adUnitId);
    }

    public void StopMrecAutoRefresh(string adUnitId)
    {
        _platformDelegate.StopMrecAutoRefresh(adUnitId);
    }

    public void DestroyMrec(string adUnitId)
    {
        _platformDelegate.DestroyMrec(adUnitId);
    }

    public void SetMrecCustomData(string adUnitId, string? customData)
    {
        _platformDelegate.SetMrecCustomData(adUnitId, customData);
    }

    public void SetMrecExtraParameter(string adUnitId, string key, string? value)
    {
        _platformDelegate.SetMrecExtraParameter(adUnitId, key, value);
    }

    public void SetMrecLocalExtraParameter(string adUnitId, string key, object? value)
    {
        _platformDelegate.SetMrecLocalExtraParameter(adUnitId, key, value);
    }

    public void SetMrecLocalExtraParameterJson(string adUnitId, string key, string? value)
    {
        _platformDelegate.SetMrecLocalExtraParameterJson(adUnitId, key, value);
    }

    public void SetMrecPlacement(string adUnitId, string? placement)
    {
        _platformDelegate.SetMrecPlacement(adUnitId, placement);
    }

    #endregion MREC Ads

    #region Interstitial Ads

    public void LoadInterstitial(string interstitialAdUnitId)
    {
        _platformDelegate.LoadInterstitial(interstitialAdUnitId);
    }

    public void ShowInterstitial(string interstitialAdUnitId, string? placementId = null, string? customData = null)
    {
        _platformDelegate.ShowInterstitial(interstitialAdUnitId, placementId, customData);
    }

    public void SetInterstitialExtraParameter(string interstitialAdUnitId, string key, string? value)
    {
        _platformDelegate.SetInterstitialExtraParameter(interstitialAdUnitId, key, value);
    }

    public void SetInterstitialLocalExtraParameter(string interstitialAdUnitId, string key, object? value)
    {
        _platformDelegate.SetInterstitialLocalExtraParameter(interstitialAdUnitId, key, value);
    }

    public bool IsInterstitialReady(string interstitialAdUnitId)
    {
        return _platformDelegate.IsInterstitialReady(interstitialAdUnitId);
    }

    #endregion Interstitial Ads

    #region Rewarded Ads

    public void LoadRewarded(string rewardedAdUnitId)
    {
        _platformDelegate.LoadRewarded(rewardedAdUnitId);
    }

    public void ShowRewarded(string rewardedAdUnitId, string? placementId = null, string? customData = null)
    {
        _platformDelegate.ShowRewarded(rewardedAdUnitId, placementId, customData);
    }

    public void SetRewardedAdExtraParameter(string rewardedAdUnitId, string key, string? value)
    {
        _platformDelegate.SetRewardedAdExtraParameter(rewardedAdUnitId, key, value);
    }

    public void SetRewardedAdLocalExtraParameter(string rewardedAdUnitId, string key, object? value)
    {
        _platformDelegate.SetRewardedAdLocalExtraParameter(rewardedAdUnitId, key, value);
    }

    public bool IsRewardedReady(string rewardedAdUnitId)
    {
        return _platformDelegate.IsRewardedReady(rewardedAdUnitId);
    }

    #endregion Rewarded Ads

    #region Private Methods

    private void SetupCallbacks()
    {
        // Banner ad callbacks
        _platformDelegate.BannerAdLoadSuccess += MeticaAdsCallbacks.Banner.OnAdLoadSuccessInternal;
        _platformDelegate.BannerAdLoadFailed += MeticaAdsCallbacks.Banner.OnAdLoadFailedInternal;
        _platformDelegate.BannerAdClicked += MeticaAdsCallbacks.Banner.OnAdClickedInternal;
        _platformDelegate.BannerAdRevenuePaid += MeticaAdsCallbacks.Banner.OnAdRevenuePaidInternal;

        // MREC ad callbacks
        _platformDelegate.MrecAdLoadSuccess += MeticaAdsCallbacks.Mrec.OnAdLoadSuccessInternal;
        _platformDelegate.MrecAdLoadFailed += MeticaAdsCallbacks.Mrec.OnAdLoadFailedInternal;
        _platformDelegate.MrecAdClicked += MeticaAdsCallbacks.Mrec.OnAdClickedInternal;
        _platformDelegate.MrecAdRevenuePaid += MeticaAdsCallbacks.Mrec.OnAdRevenuePaidInternal;

        // Interstitial ad callbacks
        _platformDelegate.InterstitialAdLoadSuccess += MeticaAdsCallbacks.Interstitial.OnAdLoadSuccessInternal;
        _platformDelegate.InterstitialAdLoadFailed += MeticaAdsCallbacks.Interstitial.OnAdLoadFailedInternal;
        _platformDelegate.InterstitialAdShowSuccess += MeticaAdsCallbacks.Interstitial.OnAdShowSuccessInternal;
        _platformDelegate.InterstitialAdShowFailed += MeticaAdsCallbacks.Interstitial.OnAdShowFailedInternal;
        _platformDelegate.InterstitialAdHidden += MeticaAdsCallbacks.Interstitial.OnAdHiddenInternal;
        _platformDelegate.InterstitialAdClicked += MeticaAdsCallbacks.Interstitial.OnAdClickedInternal;
        _platformDelegate.InterstitialAdRevenuePaid += MeticaAdsCallbacks.Interstitial.OnAdRevenuePaidInternal;

        // Rewarded ad callbacks
        _platformDelegate.RewardedAdLoadSuccess += MeticaAdsCallbacks.Rewarded.OnAdLoadSuccessInternal;
        _platformDelegate.RewardedAdLoadFailed += MeticaAdsCallbacks.Rewarded.OnAdLoadFailedInternal;
        _platformDelegate.RewardedAdShowSuccess += MeticaAdsCallbacks.Rewarded.OnAdShowSuccessInternal;
        _platformDelegate.RewardedAdShowFailed += MeticaAdsCallbacks.Rewarded.OnAdShowFailedInternal;
        _platformDelegate.RewardedAdHidden += MeticaAdsCallbacks.Rewarded.OnAdHiddenInternal;
        _platformDelegate.RewardedAdClicked += MeticaAdsCallbacks.Rewarded.OnAdClickedInternal;
        _platformDelegate.RewardedAdRewarded += MeticaAdsCallbacks.Rewarded.OnAdRewardedInternal;
        _platformDelegate.RewardedAdRevenuePaid += MeticaAdsCallbacks.Rewarded.OnAdRevenuePaidInternal;
    }

    #endregion Private Methods
}
}
