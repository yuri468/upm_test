using System;
using System.Threading;
using Metica;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Metica.Ads
{
public class InitCallbackProxy : AndroidJavaProxy
{
    private readonly SynchronizationContext _unitySyncContext;
    private readonly Action<MeticaInitResponse>? _callback;

    public InitCallbackProxy(Action<MeticaInitResponse>? callback)
        : base("com.metica.MeticaInitCallback")
    {
        _unitySyncContext = SynchronizationContext.Current;
        MeticaAdsImpl.Log.D(() => $"MeticaAdsInitCallback created");
        _callback = callback;
    }

    public void onInit(AndroidJavaObject initResponseJavaObject)
    {
        MeticaAdsImpl.Log.D(() => $"InitCallbackProxy onInit");
        var smartFloorsJavaObject = initResponseJavaObject.Call<AndroidJavaObject>("getSmartFloors");
        var smartFloors = smartFloorsJavaObject?.ToMeticaSmartFloors();

        MeticaAdsImpl.Log.D(() => $"InitCallbackProxy smartFloorsObj = {smartFloors}");
        var userId = initResponseJavaObject.Call<string>("getUserId");
        MeticaAdsImpl.Log.D(() => $"InitCallbackProxy userId = {userId}");

        var response = new MeticaInitResponse(smartFloors, userId);
        _unitySyncContext.Post(_ => _callback?.Invoke(response), null);
    }
}
}
