namespace Metica.Ads
{
    /// <summary>
    /// Interface for AppLovin-specific functionality in the Metica SDK.
    /// Provides access to privacy settings and consent flow information.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface MeticaApplovinFunctions
    {
        /// <summary>
        /// Sets an extra parameter for the ad.
        /// </summary>
        /// <param name="key">The key for the extra parameter.</param>
        /// <param name="value">The value for the extra parameter.</param>
        void SetExtraParameter(string key, string? value);

        /// <summary>
        /// Gets the adaptive banner height for a given width.
        /// </summary>
        /// <param name="width">The width of the banner.</param>
        /// <returns>The adaptive banner height.</returns>
        double GetAdaptiveBannerHeight(double width);

        /// <summary>
        /// Checks whether the device is a tablet.
        /// </summary>
        /// <returns>True if the device is a tablet, false otherwise.</returns>
        bool IsTablet();

        /// <summary>
        /// Checks whether the audio is muted.
        /// </summary>
        /// <returns>True if the audio is muted, false otherwise.</returns>
        bool IsMuted();

        /// <summary>
        /// Sets whether the audio is muted.
        /// </summary>
        /// <param name="muted">True if the audio is muted, false otherwise.</param>
        void SetMuted(bool muted);

        /// <summary>
        /// Shows the mediation debugger.
        /// </summary>
        void ShowMediationDebugger();

        /// <summary>
        /// Shows the CMP flow to an existing user.
        /// The user's current consent will be reset before the CMP alert is shown.
        /// </summary>
        void ShowCmpForExistingUser();

        /// <summary>
        /// Checks whether the user has provided consent for data collection.
        /// </summary>
        /// <returns>True if the user has consented, false otherwise</returns>
        bool HasUserConsent();

        /// <summary>
        /// Checks whether the user consent status has been explicitly set.
        /// </summary>
        /// <returns>True if consent status has been set (either granted or denied), false if not yet determined</returns>
        bool IsUserConsentSet();

        /// <summary>
        /// Gets the geographical location category of the user for consent flow purposes.
        /// </summary>
        /// <returns>The user's geographical category as determined by AppLovin's consent flow</returns>
        MaxSdkBase.ConsentFlowUserGeography GetConsentFlowUserGeography();

        /// <summary>
        /// Checks whether the SDK has been initialized successfully.
        /// </summary>
        /// <returns>True if the SDK has been initialized successfully, false otherwise</returns>
        bool IsSuccessfullyInitialized();

        /// <summary>
        /// Gets the country code registered in MAX.
        /// </summary>
        /// <returns>The country code registered in MAX</returns>
        string CountryCode();

        /// <summary>
        /// Gets the consent dialog state from MAX.
        /// </summary>
        /// <returns>The consent dialog state from MAX</returns>
        MaxSdkBase.ConsentDialogState ConsentDialogState();
    }
}
