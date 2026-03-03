using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using Metica.Ads;

namespace Metica.Ads.IOS
{
    #if UNITY_IOS || UNITY_IPHONE
    public class IOSLoadCallback
    {
        private static readonly ConcurrentDictionary<long, IOSLoadCallback> _instances = new();
        private static long _nextId;
        private readonly SynchronizationContext _unitySyncContext;

        public event Action<MeticaAd> AdLoadSuccess;
        public event Action<MeticaAdError> AdLoadFailed;

        public IOSLoadCallback()
        {
            _unitySyncContext = SynchronizationContext.Current;
            MeticaAdsImpl.Log.D(() => $"LoadCallbackProxy created");
        }

        private void InvokeOnMainThread(Action action)
        {
            if (_unitySyncContext != null)
                _unitySyncContext.Post(_ => action(), null);
            else
                action();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdLoadSuccessDelegate(IntPtr meticaAdPtr, long context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnAdLoadFailedDelegate(IntPtr meticaAdErrorPtr, long context);

        [DllImport("__Internal")]
        private static extern void ios_loadInterstitial(string adUnitId, OnAdLoadSuccessDelegate onSuccess,
            OnAdLoadFailedDelegate onFailure, long context);

        [DllImport("__Internal")]
        private static extern void ios_loadRewarded(string adUnitId, OnAdLoadSuccessDelegate onSuccess,
            OnAdLoadFailedDelegate onFail, long context);

        public void LoadInterstitial(string adUnitId)
        {
            var id = Interlocked.Increment(ref _nextId);
            _instances[id] = this;
            ios_loadInterstitial(
                adUnitId,
                OnAdLoadSuccess,
                OnAdLoadFailed,
                id
            );
        }

        public void LoadRewarded(string adUnitId)
        {
            var id = Interlocked.Increment(ref _nextId);
            _instances[id] = this;
            ios_loadRewarded(
                adUnitId,
                OnAdLoadSuccess,
                OnAdLoadFailed,
                id
            );
        }

        private static IOSLoadCallback GetAndRemove(long context)
        {
            _instances.TryRemove(context, out var instance);
            return instance;
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdLoadSuccessDelegate))]
        private static void OnAdLoadSuccess(IntPtr meticaAdPtr, long context)
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
                MeticaAdsImpl.Log.D(() => $"onAdLoadSuccess callback received for adUnitId={meticaAd.adUnitId}");
                instance?.InvokeOnMainThread(() => instance.AdLoadSuccess?.Invoke(meticaAd));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(OnAdLoadFailedDelegate))]
        private static void OnAdLoadFailed(IntPtr meticaAdErrorPtr, long context)
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
                var instance = GetAndRemove(context);
                MeticaAdsImpl.Log.D(() => $"onAdLoadFailed callback received, error={meticaAdError.message}");
                instance?.InvokeOnMainThread(() => instance.AdLoadFailed?.Invoke(meticaAdError));
            }
            catch (Exception e)
            {
                MeticaAdsImpl.Log.D(() => $"JSON decode failed: " + e);
            }
        }

    }
    #endif
}
