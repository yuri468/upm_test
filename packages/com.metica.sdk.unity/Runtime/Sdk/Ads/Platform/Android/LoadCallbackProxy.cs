using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;

namespace Metica.Ads
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LoadCallbackProxy : AndroidJavaProxy
    {
        private readonly SynchronizationContext _unitySyncContext;

        public event Action<MeticaAd> AdLoadSuccess;
        public event Action<MeticaAdError> AdLoadFailed;

        public LoadCallbackProxy()
            : base("com.metica.ads.MeticaAdsLoadCallback")
        {
            _unitySyncContext = SynchronizationContext.Current;
            MeticaAdsImpl.Log.D(() => $"LoadCallbackProxy created");
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
    }
}
