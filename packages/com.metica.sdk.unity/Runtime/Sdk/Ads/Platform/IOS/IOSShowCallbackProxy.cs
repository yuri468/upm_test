using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using Metica.Ads;

namespace Metica.Ads.IOS
{
    #if UNITY_IOS || UNITY_IPHONE
    internal class IOSShowCallbackProxy
    {
        private static readonly ConcurrentDictionary<long, IOSShowCallbackProxy> _instances = new();
        private static long _nextId;
        private readonly SynchronizationContext _unitySyncContext;

        public event Action<MeticaAd> AdShowSuccess;
        public event Action<MeticaAd, MeticaAdError> AdShowFailed;
        public event Action<MeticaAd> AdHidden;
        public event Action<MeticaAd> AdClicked;
        public event Action<MeticaAd> AdRewarded;
        public event Action<MeticaAd> AdRevenuePaid;

        public IOSShowCallbackProxy()
        {
            _unitySyncContext = SynchronizationContext.Current;
            MeticaAdsImpl.Log.D(() => $"ShowCallbackProxy created");
        }

        private void InvokeOnMainThread(Action action)
        {
            if (_unitySyncContext != null)
                _unitySyncContext.Post(_ => action(), null);
            else
                action();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdShowSuccessDelegate(IntPtr meticaAdPtr, long context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdShowFailedDelegate(IntPtr meticaAdPtr, IntPtr meticaAdErrorPtr, long context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdHiddenDelegate(IntPtr meticaAdPtr, long context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdClickedDelegate(IntPtr meticaAdPtr, long context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdRewardedDelegate(IntPtr meticaAdPtr, long context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdRevenuePaidDelegate(IntPtr meticaAdPtr, long context);

        [DllImport("__Internal")]
        private static extern void ios_showInterstitial(
            string interstitialAdUnitId, string? placementId, string? customData,
            OnAdShowSuccessDelegate onShowSuccess, OnAdShowFailedDelegate onShowFailed,
            OnAdHiddenDelegate onAdHidden, OnAdClickedDelegate onAdClicked,
            OnAdRevenuePaidDelegate onAdRevenuePaid, long context);

        [DllImport("__Internal")]
        private static extern void ios_showRewarded(
            string rewardedAdUnitId, string? placementId, string? customData,
            OnAdShowSuccessDelegate onShowSuccess, OnAdShowFailedDelegate onShowFailed,
            OnAdHiddenDelegate onAdHidden, OnAdClickedDelegate onAdClicked,
            OnAdRewardedDelegate onAdRewarded, OnAdRevenuePaidDelegate onAdRevenuePaid,
            long context);

        public void ShowInterstitial(string interstitialAdUnitId, string? placementId, string? customData)
        {
            var id = Interlocked.Increment(ref _nextId);
            _instances[id] = this;
            ios_showInterstitial(
                interstitialAdUnitId,
                placementId,
                customData,
                OnAdShowSuccess,
                OnAdShowFailed,
                OnAdHidden,
                OnAdClicked,
                OnAdRevenuePaid,
                id
            );
        }

        public void ShowRewarded(string rewardedAdUnitId, string? placementId, string? customData)
        {
            var id = Interlocked.Increment(ref _nextId);
            _instances[id] = this;
            ios_showRewarded(
                rewardedAdUnitId,
                placementId,
                customData,
                OnAdShowSuccess,
                OnAdShowFailed,
                OnAdHidden,
                OnAdClicked,
                OnAdRewarded,
                OnAdRevenuePaid,
                id
            );
        }

        private static IOSShowCallbackProxy GetInstance(long context)
        {
            _instances.TryGetValue(context, out var instance);
            return instance;
        }

        private static IOSShowCallbackProxy GetAndRemove(long context)
        {
            _instances.TryRemove(context, out var instance);
            return instance;
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdShowSuccessDelegate))]
        private static void OnAdShowSuccess(IntPtr meticaAdPtr, long context)
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
                var instance = GetInstance(context);
                MeticaAdsImpl.Log.D(() => $"onAdShowSuccess for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdShowSuccess?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdShowFailedDelegate))]
        private static void OnAdShowFailed(IntPtr meticaAdPtr, IntPtr meticaAdErrorPtr, long context)
        {
            var json = Marshal.PtrToStringUTF8(meticaAdPtr);
            var jsonError = Marshal.PtrToStringUTF8(meticaAdErrorPtr);

            if (string.IsNullOrEmpty(json))
            {
                MeticaAdsImpl.Log.D(() => $"Empty MeticaAd JSON received");
                return;
            }

            if (string.IsNullOrEmpty(jsonError))
            {
                MeticaAdsImpl.Log.D(() => $"Empty MeticaAdError JSON received");
                return;
            }

            try
            {
                var meticaAd = JsonUtility.FromJson<MeticaAd>(json);
                var meticaAdError = JsonUtility.FromJson<MeticaAdError>(jsonError);
                var instance = GetAndRemove(context);
                MeticaAdsImpl.Log.D(() =>
                    $"onAdShowFailed for adUnitId={meticaAd.adUnitId}, error={meticaAdError.message}");
                instance?.InvokeOnMainThread(() => instance.AdShowFailed?.Invoke(meticaAd, meticaAdError));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdHiddenDelegate))]
        private static void OnAdHidden(IntPtr meticaAdPtr, long context)
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
                var instance = GetAndRemove(context);
                MeticaAdsImpl.Log.D(() => $"onAdHidden for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdHidden?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdClickedDelegate))]
        private static void OnAdClicked(IntPtr meticaAdPtr, long context)
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
                var instance = GetInstance(context);
                MeticaAdsImpl.Log.D(() => $"onAdClicked for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdClicked?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdRewardedDelegate))]
        private static void OnAdRewarded(IntPtr meticaAdPtr, long context)
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
                var instance = GetInstance(context);
                MeticaAdsImpl.Log.D(() => $"onAdRewarded for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdRewarded?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdRevenuePaidDelegate))]
        private static void OnAdRevenuePaid(IntPtr meticaAdPtr, long context)
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
                var instance = GetInstance(context);
                MeticaAdsImpl.Log.D(() => $"onAdRevenuePaid for adUnitId={meticaAd.adUnitId}");
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
