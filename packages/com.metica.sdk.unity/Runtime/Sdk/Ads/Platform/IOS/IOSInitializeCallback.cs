using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using Metica;
using Metica.Ads;

namespace Metica.Ads.IOS
{
    #if UNITY_IOS || UNITY_IPHONE
    public class IOSInitializeCallback
    {
        private static Action<MeticaInitResponse>? _currentCallback;
        private static SynchronizationContext? _currentSyncContext;

        public IOSInitializeCallback(Action<MeticaInitResponse>? callback)
        {
            _currentSyncContext = SynchronizationContext.Current;
            MeticaAdsImpl.Log.D(() => $"MeticaAdsInitCallback created");
            _currentCallback = callback;
        }

        private static void InvokeOnMainThread(Action action)
        {
            if (_currentSyncContext != null)
                _currentSyncContext.Post(_ => action(), null);
            else
                action();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OnInitializeSuccessDelegate(IntPtr meticaAdsInitializationResponsePtr);

        [DllImport("__Internal")]
        private static extern void ios_sdkInitialize(
            string apiKey,
            string appId,
            string? userId,
            string mediationInfoKey,
            OnInitializeSuccessDelegate result
        );

        public void InitializeSDK(string apiKey, string appId, string? userId, string mediationInfoKey)
        {
            ios_sdkInitialize(
                apiKey,
                appId,
                userId,
                mediationInfoKey,
                OnInitialized
            );
        }

        [AOT.MonoPInvokeCallback(typeof(OnInitializeSuccessDelegate))]
        private static void OnInitialized(IntPtr meticaAdsInitializationResponsePtr)
        {
            var callback = _currentCallback;
            _currentCallback = null;

            if (callback == null)
            {
                MeticaAdsImpl.Log.D(() =>
                    $"IOSInitializeCallback OnInitialized called but _currentCallback is null (no callback provided)");
                return;
            }

            var meticaAdsInitializationResponseJson = Marshal.PtrToStringUTF8(meticaAdsInitializationResponsePtr);
            MeticaAdsImpl.Log.D(() => $"IOSInitializeCallback OnInitialized received JSON: {meticaAdsInitializationResponseJson}");

            if (string.IsNullOrEmpty(meticaAdsInitializationResponseJson))
            {
                MeticaAdsImpl.Log.E(() => $"IOSInitializeCallback OnInitialized received null or empty JSON");
                return;
            }

            try
            {
                // JsonUtility only deserializes public fields, not properties. Use a DTO with [Serializable] and public fields.
                var wrapper = JsonUtility.FromJson<MeticaInitResponseJsonWrapper>(meticaAdsInitializationResponseJson);
                if (wrapper == null)
                {
                    MeticaAdsImpl.Log.E(() =>
                        $"IOSInitializeCallback OnInitialized failed to parse JSON: {meticaAdsInitializationResponseJson}");
                    return;
                }

                var smartFloors = ParseSmartFloorsFromWrapper(wrapper, meticaAdsInitializationResponseJson);
                var userId = wrapper.userId;
                var response = new MeticaInitResponse(smartFloors, userId);

                MeticaAdsImpl.Log.D(() => $"IOSInitializeCallback smartFloorsObj = {smartFloors}, userId = {userId}");
                InvokeOnMainThread(() => callback.Invoke(response));
            }
            catch (Exception ex)
            {
                MeticaAdsImpl.Log.E(() =>
                    $"IOSInitializeCallback OnInitialized exception: {ex.Message}\nStack: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Parses SmartFloors from the wrapper. JsonUtility only fills public fields; userGroup is inferred from raw JSON.
        /// </summary>
        private static MeticaSmartFloors? ParseSmartFloorsFromWrapper(
            MeticaInitResponseJsonWrapper wrapper,
            string rawJson)
        {
            if (wrapper?.smartFloors == null)
            {
                MeticaAdsImpl.Log.D(() => "IOSInitializeCallback SmartFloors missing in JSON, using default (HOLDOUT, false)");
                return new MeticaSmartFloors(MeticaUserGroup.HOLDOUT, false);
            }

            var isSuccess = wrapper.smartFloors.isSuccess;
            var userGroup = MeticaUserGroup.HOLDOUT;
            if (!string.IsNullOrEmpty(rawJson))
            {
                if (rawJson.IndexOf("\"trial\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    userGroup = MeticaUserGroup.TRIAL;
                else if (rawJson.IndexOf("\"holdout\"", StringComparison.OrdinalIgnoreCase) >= 0)
                    userGroup = MeticaUserGroup.HOLDOUT;
            }

            return new MeticaSmartFloors(userGroup, isSuccess);
        }

        [Serializable]
        private class MeticaInitResponseJsonWrapper
        {
            public SmartFloorsJsonWrapper smartFloors;
            public string? userId;
        }

        [Serializable]
        private class SmartFloorsJsonWrapper
        {
            public bool isSuccess;
        }
    }
    #endif
}
