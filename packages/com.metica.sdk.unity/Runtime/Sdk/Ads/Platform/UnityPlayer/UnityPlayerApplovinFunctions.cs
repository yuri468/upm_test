namespace Metica.Ads.UnityPlayer
{
    internal class UnityPlayerApplovinFunctions : MeticaApplovinFunctions
    {

        public void SetExtraParameter(string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetExtraParameter called - key: {key}, value: {value}");
        }

        public double GetAdaptiveBannerHeight(double width)
        {
            MeticaAdsImpl.Log.D(() => $"Mock GetAdaptiveBannerHeight called - width: {width}");
            return 0;
        }

        public bool IsTablet()
        {
            MeticaAdsImpl.Log.D(() => "Mock IsTablet called - returning false");
            return false;
        }

        public bool IsMuted()
        {
            MeticaAdsImpl.Log.D(() => "Mock IsMuted called - returning false");
            return false;
        }

        public void SetMuted(bool muted)
        {
            MeticaAdsImpl.Log.D(() => $"Mock SetMuted called - muted: {muted}");
        }

        public void ShowMediationDebugger()
        {
            MeticaAdsImpl.Log.D(() => "Mock ShowMediationDebugger called");
        }

        public void ShowCmpForExistingUser()
        {
            MeticaAdsImpl.Log.D(() => "Mock ShowCmpForExistingUser called");
        }

        public bool HasUserConsent()
        {
            MeticaAdsImpl.Log.D(() => "Mock HasUserConsent called - returning false");
            return false;
        }

        public bool IsUserConsentSet()
        {
            MeticaAdsImpl.Log.D(() => "Mock IsUserConsentSet called - returning false");
            return false;
        }

        public bool IsSuccessfullyInitialized()
        {
            MeticaAdsImpl.Log.D(() => "Mock IsSuccessfullyInitialized called - returning true");
            return true;
        }

        public string CountryCode()
        {
            MeticaAdsImpl.Log.D(() => "Mock CountryCode called - returning US");
            return "";
        }

        public MaxSdkBase.ConsentDialogState ConsentDialogState()
        {
            MeticaAdsImpl.Log.D(() => "Mock ConsentDialogState called - returning Unknown");
            return MaxSdkBase.ConsentDialogState.Unknown;
        }

        public MaxSdk.ConsentFlowUserGeography GetConsentFlowUserGeography()
        {
            MeticaAdsImpl.Log.D(() => "Mock GetConsentFlowUserGeography called - returning Unknown");
            return MaxSdk.ConsentFlowUserGeography.Unknown;
        }
    }
}
