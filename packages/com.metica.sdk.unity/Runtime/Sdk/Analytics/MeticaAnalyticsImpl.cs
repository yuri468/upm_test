#if METICA_ANALYTICS
#nullable enable

using System.Collections.Generic;
using Metica.Ads;
using Metica.Analytics.Abstractions;

// ReSharper disable once CheckNamespace
namespace Metica.Analytics
{
internal sealed class MeticaAnalyticsImpl : IMeticaAnalytics
{
    private readonly PlatformDelegate _platformDelegate;

    internal MeticaAnalyticsImpl(PlatformDelegate platformDelegate)
    {
        _platformDelegate = platformDelegate;
    }

    public void LogPurchaseEvent(string productId, string currency, double amount, string status, string? errorCode,
        string? referenceId, Dictionary<string, object>? customPayload)
    {
        _platformDelegate.LogPurchaseEvent(productId, currency, amount, status, errorCode, referenceId, customPayload);
    }

    public void LogSessionStartEvent(Dictionary<string, object>? customPayload)
    {
        _platformDelegate.LogSessionStartEvent(customPayload);
    }

    public void LogInstallEvent(Dictionary<string, object>? customPayload)
    {
        _platformDelegate.LogInstallEvent(customPayload);
    }

    public void LogImpressionEvent(double value, string type, string mediator, string source, string? placement,
        Dictionary<string, object>? customPayload)
    {
        _platformDelegate.LogImpressionEvent(value, type, mediator, source, placement, customPayload);
    }

    public void LogFullStateUpdateEvent(Dictionary<string, object> attributes)
    {
        _platformDelegate.LogFullStateUpdateEvent(attributes);
    }

    public void LogPartialStateUpdateEvent(Dictionary<string, object> attributes)
    {
        _platformDelegate.LogPartialStateUpdateEvent(attributes);
    }

    public void LogCustomEvent(string eventName, Dictionary<string, object>? properties)
    {
        _platformDelegate.LogCustomEvent(eventName, properties);
    }
}
}
#endif
