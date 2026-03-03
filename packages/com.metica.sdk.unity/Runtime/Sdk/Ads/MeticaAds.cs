#nullable enable

namespace Metica.Ads
{
    /// <summary>
    /// Public interface for Metica Ads functionality.
    /// Ensures all ad methods are consistently exposed through MeticaSdk.Ads.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface MeticaAds
    {
        /// <summary>
        /// Access to AppLovin MAX-specific functionality.
        /// </summary>
        MeticaApplovinFunctions Max { get; }

        /// <summary>
        /// Sets whether the user has provided consent for data collection.
        /// </summary>
        void SetHasUserConsent(bool hasUserConsent);

        /// <summary>
        /// Sets whether the user has opted out of data selling.
        /// </summary>
        void SetDoNotSell(bool doNotSell);

        /// <summary>
        /// Creates and loads a banner ad with the specified configuration.
        /// </summary>
        void CreateBanner(string adUnitId, MeticaAdViewConfiguration configuration);

        /// <summary>
        /// Shows a previously created banner ad.
        /// </summary>
        void ShowBanner(string adUnitId);

        /// <summary>
        /// Hides a banner ad (can be shown again later).
        /// </summary>
        void HideBanner(string adUnitId);

        /// <summary>
        /// Loads a banner ad.
        /// </summary>
        void LoadBanner(string adUnitId);

        /// <summary>
        /// Starts automatic refresh for a banner ad.
        /// </summary>
        void StartBannerAutoRefresh(string adUnitId);

        /// <summary>
        /// Stops automatic refresh for a banner ad.
        /// </summary>
        void StopBannerAutoRefresh(string adUnitId);

        /// <summary>
        /// Destroys a banner ad and frees resources.
        /// </summary>
        void DestroyBanner(string adUnitId);

        /// <summary>
        /// Sets the custom data for a banner ad.
        /// </summary>
        void SetBannerCustomData(string adUnitId, string? customData);

        /// <summary>
        /// Sets the extra parameter for a banner ad.
        /// </summary>
        void SetBannerExtraParameter(string adUnitId, string key, string? value);

        /// <summary>
        /// Sets the local extra parameter for a banner ad.
        /// </summary>
        void SetBannerLocalExtraParameter(string adUnitId, string key, object? value);

        /// <summary>
        /// Sets the local extra parameter for a banner ad.
        /// </summary>
        void SetBannerLocalExtraParameterJson(string adUnitId, string key, string? value);

        /// <summary>
        /// Sets the placement for a banner ad.
        /// Placements are used for analytics and reporting. Call before LoadBanner.
        /// </summary>
        void SetBannerPlacement(string adUnitId, string? placement);

        /// <summary>
        /// Sets the width for a banner ad.
        /// </summary>
        void SetBannerWidth(string adUnitId, float widthDp);

        /// <summary>
        /// Sets the background color for a banner ad.
        /// </summary>
        void SetBannerBackgroundColor(string adUnitId, string hexColorCode);

        /// <summary>
        /// Creates and loads an MREC ad with the specified configuration.
        /// </summary>
        void CreateMrec(string adUnitId, MeticaAdViewConfiguration configuration);

        /// <summary>
        /// Shows a previously created MREC ad.
        /// </summary>
        void ShowMrec(string adUnitId);

        /// <summary>
        /// Hides an MREC ad (can be shown again later).
        /// </summary>
        void HideMrec(string adUnitId);

        /// <summary>
        /// Loads an MREC ad.
        /// </summary>
        void LoadMrec(string adUnitId);

        /// <summary>
        /// Starts automatic refresh for an MREC ad.
        /// </summary>
        void StartMrecAutoRefresh(string adUnitId);

        /// <summary>
        /// Stops automatic refresh for an MREC ad.
        /// </summary>
        void StopMrecAutoRefresh(string adUnitId);

        /// <summary>
        /// Destroys an MREC ad and frees resources.
        /// </summary>
        void DestroyMrec(string adUnitId);

        /// <summary>
        /// Sets the placement for an MREC ad.
        /// Placements are used for analytics and reporting. Call before LoadMrec.
        /// </summary>
        void SetMrecPlacement(string adUnitId, string? placement);

        /// <summary>
        /// Sets the custom data for a MREC ad.
        /// </summary>
        void SetMrecCustomData(string adUnitId, string? customData);

        /// <summary>
        /// Sets the extra parameter for a MREC ad.
        /// </summary>
        void SetMrecExtraParameter(string adUnitId, string key, string? value);

        /// <summary>
        /// Sets the local extra parameter for a MREC ad.
        /// </summary>
        void SetMrecLocalExtraParameter(string adUnitId, string key, object? value);

        /// <summary>
        /// Sets the local extra parameter for a MREC ad.
        /// </summary>
        void SetMrecLocalExtraParameterJson(string adUnitId, string key, string? value);

        /// <summary>
        /// Loads an interstitial ad.
        /// </summary>
        void LoadInterstitial(string adUnitId);

        /// <summary>
        /// Shows an interstitial ad with optional placement and custom data.
        /// </summary>
        void ShowInterstitial(string adUnitId, string? placementId = null, string? customData = null);

        /// <summary>
        /// Sets the extra parameter for an interstitial ad.
        /// </summary>
        void SetInterstitialExtraParameter(string adUnitId, string key, string? value);

        /// <summary>
        /// Sets the local extra parameter for an interstitial ad.
        /// </summary>
        void SetInterstitialLocalExtraParameter(string adUnitId, string key, object? value);

        /// <summary>
        /// Checks if an interstitial ad is ready to be shown.
        /// </summary>
        bool IsInterstitialReady(string adUnitId);

        /// <summary>
        /// Loads a rewarded ad.
        /// </summary>
        void LoadRewarded(string adUnitId);

        /// <summary>
        /// Shows a rewarded ad with optional placement and custom data.
        /// </summary>
        void ShowRewarded(string adUnitId, string? placementId = null, string? customData = null);

        /// <summary>
        /// Sets the extra parameter for a rewarded ad.
        /// </summary>
        void SetRewardedAdExtraParameter(string adUnitId, string key, string? value);

        /// <summary>
        /// Sets the local extra parameter for a rewarded ad.
        /// </summary>
        void SetRewardedAdLocalExtraParameter(string adUnitId, string key, object? value);

        /// <summary>
        /// Checks if a rewarded ad is ready to be shown.
        /// </summary>
        bool IsRewardedReady(string adUnitId);
    }
}
