#nullable enable

using System;
using System.Runtime.InteropServices;
using Metica.Ads;

namespace Metica.Ads.IOS
{
    #if UNITY_IOS || UNITY_IPHONE
    internal class IOSApplovinFunctions : MeticaApplovinFunctions
    {
        [DllImport("__Internal")]
        private static extern bool ios_hasUserConsent();
        [DllImport("__Internal")]
        private static extern bool ios_isUserConsentSet();
        [DllImport("__Internal")]
        private static extern int ios_getConsentFlowUserGeography();
        [DllImport("__Internal")]
        private static extern bool ios_isTablet();
        [DllImport("__Internal")]
        private static extern bool ios_isMuted();
        [DllImport("__Internal")]
        private static extern void ios_setMuted(bool muted);
        [DllImport("__Internal")]
        private static extern void ios_showCmpForExistingUser();
        [DllImport("__Internal")]
        private static extern void ios_showMediationDebugger();
        [DllImport("__Internal")]
        private static extern void ios_setExtraParameter(string key, string? value);
        [DllImport("__Internal")]
        private static extern double ios_getAdaptiveBannerHeight(double width);
        [DllImport("__Internal")]
        private static extern bool ios_isSuccessfullyInitialized();
        [DllImport("__Internal")]
        private static extern string ios_getCountryCode();
        [DllImport("__Internal")]
        private static extern int ios_getConsentDialogState();

        public void SetExtraParameter(string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.setExtraParameter method");
            ios_setExtraParameter(key, value);
            MeticaAdsImpl.Log.D(() => $"iOS Max.setExtraParameter method called");
        }

        public double GetAdaptiveBannerHeight(double width)
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.getAdaptiveBannerHeight method");
            var result = ios_getAdaptiveBannerHeight(width);
            MeticaAdsImpl.Log.D(() => $"iOS Max.getAdaptiveBannerHeight returned: {result}");
            return result;
        }

        public bool HasUserConsent()
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.hasUserConsent method");
            var result = ios_hasUserConsent();
            MeticaAdsImpl.Log.D(() => $"iOS Max.hasUserConsent returned: {result}");
            return result;
        }

        public bool IsTablet() {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.isTablet method");
            var result = ios_isTablet();
            MeticaAdsImpl.Log.D(() => $"iOS Max.isTablet returned: {result}");
            return result;
        }

        public bool IsMuted() {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.isMuted method");
            var result = ios_isMuted();
            MeticaAdsImpl.Log.D(() => $"iOS Max.isMuted returned: {result}");
            return result;
        }

        public void SetMuted(bool muted) {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.isMuted method");
            ios_setMuted(muted);
            MeticaAdsImpl.Log.D(() => $"iOS Max.setMuted method called");
        }

        public bool IsUserConsentSet()
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.isUserConsentSet method");
            var result = ios_isUserConsentSet();
            MeticaAdsImpl.Log.D(() => $"iOS Max.isUserConsentSet returned: {result}");
            return result;
        }

        public void ShowCmpForExistingUser() {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.showCmpForExistingUser method");
            ios_showCmpForExistingUser();
            MeticaAdsImpl.Log.D(() => $"iOS Max.showCmpForExistingUser method called");
        }

        public void ShowMediationDebugger() {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.showMediationDebugger method");
            ios_showMediationDebugger();
            MeticaAdsImpl.Log.D(() => $"iOS Max.showMediationDebugger method called");
        }

        public bool IsSuccessfullyInitialized()
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.isSuccessfullyInitialized method");
            var result = ios_isSuccessfullyInitialized();
            MeticaAdsImpl.Log.D(() => $"iOS Max.isSuccessfullyInitialized returned: {result}");
            return result;
        }

        public string CountryCode()
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.countryCode method");
            var result = ios_getCountryCode();
            MeticaAdsImpl.Log.D(() => $"iOS Max.countryCode returned: {result}");
            return result;
        }

        public MaxSdkBase.ConsentDialogState ConsentDialogState()
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.consentDialogState method");
            var ordinal = ios_getConsentDialogState();
            MeticaAdsImpl.Log.D(() => $"iOS Max.consentDialogState returned ordinal: {ordinal}");
            return (MaxSdkBase.ConsentDialogState)ordinal;
        }

        public MaxSdk.ConsentFlowUserGeography GetConsentFlowUserGeography()
        {
            MeticaAdsImpl.Log.D(() => $"About to call iOS Max.getConsentFlowUserGeography method");
            var ordinal = ios_getConsentFlowUserGeography();
            MeticaAdsImpl.Log.D(() => $"iOS Max.getConsentFlowUserGeography returned ordinal: {ordinal}");
            return (MaxSdkBase.ConsentFlowUserGeography)ordinal;
        }
    }
    #endif
}
