using System;
using System.Threading;
using UnityEngine;

namespace Metica.Ads
{
    public class BannerCallbackProxy : AndroidJavaProxy
    {
        private readonly SynchronizationContext _unitySyncContext;

        public event Action<MeticaAd> AdLoadSuccess;
        public event Action<MeticaAdError> AdLoadFailed;
        public event Action<MeticaAd> AdClicked;
        public event Action<MeticaAd> AdRevenuePaid;

        public BannerCallbackProxy()
            : base("com.metica.ads.MeticaAdsAdViewCallback")
        {
            _unitySyncContext = SynchronizationContext.Current;
            MeticaAdsImpl.Log.D(() => $"BannerCallbackProxy created");
        }

        public void onAdLoadSuccess(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdLoadSuccess callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdLoadSuccess?.Invoke(meticaAd), null);
        }

        public void onAdLoadFailed(AndroidJavaObject meticaAdErrorObject)
        {
            var meticaAdError = meticaAdErrorObject.ToMeticaAdError();
            MeticaAdsImpl.Log.D(() => $"onAdLoadFailed callback received, error={meticaAdError}");
            _unitySyncContext.Post(_ => AdLoadFailed?.Invoke(meticaAdError), null);
        }

        public void onAdClicked(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdClicked callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdClicked?.Invoke(meticaAd), null);
        }

        public void onAdRevenuePaid(AndroidJavaObject meticaAdObject)
        {
            var meticaAd = meticaAdObject.ToMeticaAd();
            MeticaAdsImpl.Log.D(() => $"onAdRevenuePaid callback received for adUnitId={meticaAd.adUnitId}");
            _unitySyncContext.Post(_ => AdRevenuePaid?.Invoke(meticaAd), null);
        }
    }
}
