// LoadCallbackProxy.cs

using System;
using UnityEngine;

namespace Metica.Ads
{
public class InitResponseProxy
{

    public InitResponseProxy(AndroidJavaObject javaObject)
    {
        MeticaAdsImpl.Log.D(() => $"InitResponseProxy created");
        var smartFloorsObj = javaObject.Get<AndroidJavaObject>("smartFloors");
        MeticaAdsImpl.Log.D(() => $"InitResponseProxy smartFloorsObj = {smartFloorsObj}");
    }
}
}
