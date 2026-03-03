#nullable enable

using UnityEngine;

namespace Metica.Ads.Android
{
    internal class AndroidApplovinFunctions : MeticaApplovinFunctions
    {
        private readonly AndroidJavaObject _maxObject;

        public AndroidApplovinFunctions(AndroidJavaObject maxObject)
        {
            _maxObject = maxObject;
        }

        public void SetExtraParameter(string key, string? value)
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.setExtraParameter method with key: {key}, value: {value}");
            _maxObject.Call("setExtraParameter", key, value);
            MeticaAdsImpl.Log.D(() => $"Android Max.setExtraParameter method called");
        }

        public double GetAdaptiveBannerHeight(double width)
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.getAdaptiveBannerHeight method with width: {width}");
            var result = _maxObject.Call<double>("getAdaptiveBannerHeight", width);
            MeticaAdsImpl.Log.D(() => $"Android Max.getAdaptiveBannerHeight method called");
            return result;
        }

        public bool IsTablet() 
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.isTablet method");
            var result = _maxObject.Call<bool>("isTablet");
            MeticaAdsImpl.Log.D(() => $"Android Max.isTablet method called");
            return result;
        }

        public bool IsMuted() 
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.isMuted method");
            var result = _maxObject.Call<bool>("isMuted");
            MeticaAdsImpl.Log.D(() => $"Android Max.isMuted method called");
            return result;
        }

        public void SetMuted(bool muted) 
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.setMuted method");
            _maxObject.Call("setMuted", muted);
            MeticaAdsImpl.Log.D(() => $"Android Max.setMuted method called");
        }

        public void ShowMediationDebugger()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.showMediationDebugger method");
            _maxObject.Call("showMediationDebugger");
            MeticaAdsImpl.Log.D(() => $"Android Max.showMediationDebugger method called");
        }

        public void ShowCmpForExistingUser()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.showCmpForExistingUser method");
            _maxObject.Call("showCmpForExistingUser");
            MeticaAdsImpl.Log.D(() => $"Android Max.showCmpForExistingUser method called");
        }

        public bool HasUserConsent()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.hasUserConsent method");
            var result = _maxObject.Call<bool>("hasUserConsent");
            MeticaAdsImpl.Log.D(() => $"Android Max.hasUserConsent returned: {result}");
            return result;
        }

        public bool IsUserConsentSet()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.isUserConsentSet method");
            var result = _maxObject.Call<bool>("isUserConsentSet");
            MeticaAdsImpl.Log.D(() => $"Android Max.isUserConsentSet returned: {result}");
            return result;
        }

        public bool IsSuccessfullyInitialized()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.isSuccessfullyInitialized method");
            var result = _maxObject.Call<bool>("isSuccessfullyInitialized");
            MeticaAdsImpl.Log.D(() => $"Android Max.isSuccessfullyInitialized returned: {result}");
            return result;
        }

        public string CountryCode()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.CountryCode method");
            var result = _maxObject.Call<string>("getCountryCode");
            MeticaAdsImpl.Log.D(() => $"Android Max.CountryCode returned: {result}");
            return result;
        }

        public MaxSdkBase.ConsentDialogState ConsentDialogState()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.consentDialogState method");
            var geographyObject = _maxObject.Call<AndroidJavaObject>("getConsentDialogState");
            var ordinal = geographyObject.Call<int>("ordinal");
            MeticaAdsImpl.Log.D(() => $"Android Max.consentDialogState returned ordinal: {ordinal}");
            return (MaxSdkBase.ConsentDialogState)ordinal;
        }

        public MaxSdkBase.ConsentFlowUserGeography GetConsentFlowUserGeography()
        {
            MeticaAdsImpl.Log.D(() => $"About to call Android Max.getConsentFlowUserGeography method");
            var geographyObject = _maxObject.Call<AndroidJavaObject>("getConsentFlowUserGeography");
            var ordinal = geographyObject.Call<int>("ordinal");
            MeticaAdsImpl.Log.D(() => $"Android Max.getConsentFlowUserGeography returned ordinal: {ordinal}");
            return (MaxSdkBase.ConsentFlowUserGeography)ordinal;
        }
    }
}
