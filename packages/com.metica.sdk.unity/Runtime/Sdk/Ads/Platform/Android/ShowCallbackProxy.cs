using System;
using System.Threading;
using UnityEngine;

namespace Metica.Ads
{
    public class ShowCallbackProxy : AndroidJavaProxy
    {
        private readonly SynchronizationContext _unitySyncContext;

        public event Action<MeticaAd> AdShowSuccess;
        public event Action<MeticaAd, MeticaAdError> AdShowFailed;
        public event Action<MeticaAd> AdHidden;
        public event Action<MeticaAd> AdClicked;
        public event Action<MeticaAd> AdRewarded;
        public event Action<MeticaAd> AdRevenuePaid;

        public ShowCallbackProxy()
            : base("com.metica.ads.MeticaAdsShowCallback")
        {
            _unitySyncContext = SynchronizationContext.Current;
            MeticaAdsImpl.Log.D(() => $"ShowCallbackProxy created");
        }

        public void onAdShowSuccess(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdShowSuccess callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdShowSuccess?.Invoke(meticaAd), null);
        }

        public void onAdShowFailed(AndroidJavaObject meticaAdObject, AndroidJavaObject meticaAdErrorObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            var meticaAdError = meticaAdErrorObject.ToMeticaAdError();
            MeticaAdsImpl.Log.D(() => $"onAdShowFailed callback received for adUnitId={meticaAd.adUnitId}, error={meticaAdError}");
            _unitySyncContext.Post(_ => AdShowFailed?.Invoke(meticaAd, meticaAdError), null);
        }

        public void onAdHidden(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdHidden callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdHidden?.Invoke(meticaAd), null);
        }

        public void onAdClicked(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdClicked callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdClicked?.Invoke(meticaAd), null);
        }

        public void onAdRewarded(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdRewarded callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdRewarded?.Invoke(meticaAd), null);
        }

        public void onAdRevenuePaid(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdRevenuePaid callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdRevenuePaid?.Invoke(meticaAd), null);
        }
    }
}
