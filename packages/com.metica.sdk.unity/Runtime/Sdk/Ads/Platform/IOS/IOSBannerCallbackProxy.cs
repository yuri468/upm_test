using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using Metica.Ads;

namespace Metica.Ads.IOS
{
    #if UNITY_IOS || UNITY_IPHONE
    public class IOSBannerCallbackProxy
    {
        private static readonly ConcurrentDictionary<string, IOSBannerCallbackProxy> _instances = new();
        private readonly SynchronizationContext _unitySyncContext;

        public event Action<MeticaAd> AdLoadSuccess;
        public event Action<MeticaAdError> AdLoadFailed;
        public event Action<MeticaAd> AdClicked;
        public event Action<MeticaAd> AdRevenuePaid;

        public IOSBannerCallbackProxy()
        {
            _unitySyncContext = SynchronizationContext.Current;
            MeticaAdsImpl.Log.D(() => $"BannerCallbackProxy created");
        }

        private void InvokeOnMainThread(Action action)
        {
            if (_unitySyncContext != null)
                _unitySyncContext.Post(_ => action(), null);
            else
                action();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdLoadSuccessDelegate(IntPtr meticaAdPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdLoadFailedDelegate(IntPtr meticaAdErrorPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdClickedDelegate(IntPtr meticaAdPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdRevenuePaidDelegate(IntPtr meticaAdPtr);

        [DllImport("__Internal")]
        private static extern void ios_createBannerOrMrecAdWithPosition(
            string adUnitId,
            string position,
            string adFormat,
            OnAdLoadSuccessDelegate onSuccess,
            OnAdLoadFailedDelegate onFailure,
            OnAdClickedDelegate onClicked,
            OnAdRevenuePaidDelegate onRevenuePaid
        );

        [DllImport("__Internal")]
        private static extern void ios_createBannerOrMrecAdWithCoords(
            string adUnitId,
            double xCoordinate,
            double yCoordinate,
            string adFormat,
            OnAdLoadSuccessDelegate onSuccess,
            OnAdLoadFailedDelegate onFailure,
            OnAdClickedDelegate onClicked,
            OnAdRevenuePaidDelegate onRevenuePaid
        );

        public void CreateBannerOrMrecWithPosition(string adUnitId, string position, string adFormat)
        {
            _instances.TryAdd(adUnitId, this);
            ios_createBannerOrMrecAdWithPosition(
                adUnitId,
                position,
                adFormat,
                OnAdLoadSuccess,
                OnAdLoadFailed,
                OnAdClicked,
                OnAdRevenuePaid
            );
        }

        public void CreateBannerOrMrecWithCoords(string adUnitId, double xCoordinate, double yCoordinate, string adFormat)
        {
            _instances.TryAdd(adUnitId, this);
            ios_createBannerOrMrecAdWithCoords(
                adUnitId,
                xCoordinate,
                yCoordinate,
                adFormat,
                OnAdLoadSuccess,
                OnAdLoadFailed,
                OnAdClicked,
                OnAdRevenuePaid
            );
        }

        private static IOSBannerCallbackProxy GetInstance(string adUnitId)
        {
            _instances.TryGetValue(adUnitId, out var instance);
            return instance;
        }

        public static void RemoveInstance(string adUnitId)
        {
            _instances.TryRemove(adUnitId, out _);
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdLoadSuccessDelegate))]
        private static void OnAdLoadSuccess(IntPtr meticaAdPtr)
        {
            var json = Marshal.PtrToStringUTF8(meticaAdPtr);

            if (string.IsNullOrEmpty(json))
            {
                MeticaAdsImpl.Log.D(() => $"Empty JSON received");
                return;
            }

            try
            {
                var meticaAd = JsonUtility.FromJson<MeticaAd>(json);
                var instance = GetInstance(meticaAd.adUnitId);
                MeticaAdsImpl.Log.D(() => $"onAdLoadSuccess callback received for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdLoadSuccess?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdLoadFailedDelegate))]
        private static void OnAdLoadFailed(IntPtr meticaAdErrorPtr)
        {
            var json = Marshal.PtrToStringUTF8(meticaAdErrorPtr);

            if (string.IsNullOrEmpty(json))
            {
                MeticaAdsImpl.Log.D(() => $"Empty JSON received");
                return;
            }

            try
            {
                var meticaAdError = JsonUtility.FromJson<MeticaAdError>(json);
                var instance = GetInstance(meticaAdError.adUnitId);
                MeticaAdsImpl.Log.D(() => $"onAdLoadFailed callback received, error={meticaAdError.message}");
                instance?.InvokeOnMainThread(() => instance.AdLoadFailed?.Invoke(meticaAdError));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdClickedDelegate))]
        private static void OnAdClicked(IntPtr meticaAdPtr)
        {
            var json = Marshal.PtrToStringUTF8(meticaAdPtr);

            if (string.IsNullOrEmpty(json))
            {
                MeticaAdsImpl.Log.D(() => $"Empty JSON received");
                return;
            }

            try
            {
                var meticaAd = JsonUtility.FromJson<MeticaAd>(json);
                var instance = GetInstance(meticaAd.adUnitId);
                MeticaAdsImpl.Log.D(() => $"onAdClicked callback received for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdClicked?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdRevenuePaidDelegate))]
        private static void OnAdRevenuePaid(IntPtr meticaAdPtr)
        {
            var json = Marshal.PtrToStringUTF8(meticaAdPtr);

            if (string.IsNullOrEmpty(json))
            {
                MeticaAdsImpl.Log.D(() => $"Empty JSON received");
                return;
            }

            try
            {
                var meticaAd = JsonUtility.FromJson<MeticaAd>(json);
                var instance = GetInstance(meticaAd.adUnitId);
                MeticaAdsImpl.Log.D(() => $"onAdRevenuePaid callback received for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdRevenuePaid?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }
    }
    #endif
}
