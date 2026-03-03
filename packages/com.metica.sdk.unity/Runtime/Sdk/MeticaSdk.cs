#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Metica.Network;
using Metica.Core;
using Metica.Model;
using Metica.Unity;
using Metica.Ads;
#if METICA_ANALYTICS
using Metica.Analytics;
using Metica.Analytics.Abstractions;
#endif

namespace Metica
{
    public class MeticaSdk
    {
        static MeticaSdk()
        {
            RegisterCoreServices();
        }

        /// <summary>
        /// Registers core services (ILog, IDeviceInfoProvider) in the Registry.
        /// Called from the static constructor and from MeticaUnityHandlers to
        /// re-register after Registry is wiped on Play Mode entry without Domain Reload.
        /// </summary>
        internal static void RegisterCoreServices()
        {
            Registry.RegisterIfNull<IDeviceInfoProvider>(new DeviceInfoProvider());
            Registry.RegisterIfNull<ILog>(new Logger("MeticaUnityPlugin"));
        }
        

        #region Fields
        
        private static MeticaSdk? Sdk { get; set; }

        public static string Version { get => "2.2.1"; }

        internal static string? UserId { get; set; }
        public static string? ApiKey { get; private set; }
        public static string? AppId { get; private set; }

        private readonly IHttpService _http;
        private readonly OfferManager _offerManager;
        private readonly ConfigManager _configManager;
        private readonly EventManager _eventManager;
        private const string EndpointDev = "https://api-gateway.dev.metica.com";
        private const string EndpointProd = "https://api-gateway.prod-eu.metica.com";

        private static readonly PlatformDelegate PlatformDelegate = CreatePlatformDelegate();

        #endregion Fields

        #region Methods

        public static void SetLogEnabled(bool logEnabled)
        {
            var logger = Registry.Resolve<ILog>();
            logger.CurrentLogLevel = logEnabled ? LogLevel.Debug : LogLevel.Error;
            PlatformDelegate.SetLogEnabled(logEnabled);
        }
        /// <summary>
        /// Utility, (Unity specific) method that should not be used directly.
        /// Resets static properties to null.
        /// </summary>
        public static void ResetStaticFields()
        {
            UserId = null;
            ApiKey = null;
            AppId = null;
        }

        private static void CheckConfig(MeticaInitConfig config)
        {
            if (string.IsNullOrEmpty(config.ApiKey) || string.IsNullOrEmpty(config.AppId))
            {
                Log.Error(() => "The given SDK configuration is not valid. Please make sure all fields are filled.");
                throw new InvalidOperationException("MeticaSDK cannot initialize with invalid configuration - ApiKey and AppId are required.");
            }
        }

        /// <summary>
        /// Initializes the SDK with an optional callback for completion notification.
        /// </summary>
        /// <param name="config">Metica SDK configuration object.</param>
        /// <param name="mediationInfo">Optional mediation configuration.</param>
        /// <param name="callback">Optional callback invoked when initialization completes.</param>
        public static void Initialize(
            MeticaInitConfig config,
            MeticaMediationInfo? mediationInfo,
            Action<MeticaInitResponse>? callback = null)
        {
            CheckConfig(config);

            if (Sdk != null)
            {
                // TODO: MET-4482 Allow multiple initialize calls
                Log.Warning(() => "Metica SDK reinitialized. This means a new initialization was done on top of a previous one.");
            }
            Sdk = new MeticaSdk(config);

            PlatformDelegate.Initialize(config, mediationInfo, callback);
        }

        /// <summary>
        /// Registers services and initializes all SDK components.
        /// </summary>
        [Obsolete("Use Initialize(config, mediationInfo, callback) instead.")]
        public static Task<MeticaInitResponse> InitializeAsync(
            MeticaInitConfig config,
            MeticaMediationInfo? mediationInfo)
        {
            var tcs = new TaskCompletionSource<MeticaInitResponse>();
            Initialize(config, mediationInfo, response => tcs.SetResult(response));
            return tcs.Task;
        }

        /// <summary>
        /// ORIGINAL SDK INITIALIZATION
        /// </summary>
        /// <param name="config">Metica SDK configuration object.</param>
        private MeticaSdk(MeticaInitConfig config)
        {
            _http = new HttpServiceDotnet(
                requestTimeoutSeconds: 60,
                cacheGCTimeoutSeconds: 10,
                cacheTTLSeconds: 60
                ).WithPersistentHeaders(new Dictionary<string, string> { { "X-API-Key", config.ApiKey } });
            // Initialize an OfferManager
            _offerManager = new OfferManager(_http, $"{EndpointProd}/offers/v1/apps/{config.AppId}");
            // Initialize a ConfigManager
            _configManager = new ConfigManager(_http, $"{EndpointProd}/configs/v1/apps/{config.AppId}");
            // Initialize an EventManager with _offerManager as IMeticaAttributesProvider


            _eventManager = new EventManager(_http, $"{EndpointProd}/ingest/v1/events", _offerManager);
            // Set the CurrentUserId with the initial value given in the configuration

            // - - - - - - - - - - -

            UserId = config.UserId;
            ApiKey = config.ApiKey;
            AppId = config.AppId;
        }

        public static async ValueTask DisposeAsync()
            => await Sdk.SdkDisposeAsync();

        private async ValueTask SdkDisposeAsync()
        {
            if (_eventManager != null) await _eventManager.DisposeAsync();
            if (_offerManager != null) await _offerManager.DisposeAsync();
            if (_configManager != null) await _configManager.DisposeAsync();
            _http?.Dispose();
            // TODO: Implement MeticaAds dispose if needed
            Sdk = null;
        }

        #endregion Methods

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Analytics
#if METICA_ANALYTICS

        public static IMeticaAnalytics Analytics { get; } = new MeticaAnalyticsImpl(PlatformDelegate);

#endif
        #endregion Analytics

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Ads

        public static MeticaAds Ads { get; } = new MeticaAdsImpl(PlatformDelegate);

        #endregion Ads

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Offers

        public static class Offers
        {
            public static async Task<OfferResult> GetOffersAsync(string[] placements, Dictionary<string, object> userData = null)
                => await Sdk._offerManager.GetOffersAsync(UserId, placements, userData);
        }

        #endregion Offers

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region SmartConfig

        public static class SmartConfig
        {
            public static async Task<ConfigResult> GetConfigsAsync(List<string> configKeys = null, Dictionary<string, object> userProperties = null)
                => await Sdk._configManager.GetConfigsAsync(UserId, configKeys, userProperties);
        }

        #endregion SmartConfig


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Events

        public static class Events
        {
            public static void LogLoginEvent(Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventAsync(
                    UserId,
                    AppId,
                    EventTypes.Login,
                    null,
                    customPayload);

            public static void LogInstallEvent(Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventAsync(
                    UserId,
                    AppId,
                    EventTypes.Install,
                    null,
                    customPayload);

            public static void LogOfferPurchaseEvent(string placementId, string offerId, string currencyCode, double totalAmount, Dictionary<string, object> customPayload = null)
                => _ = Sdk._eventManager.QueueEventWithMeticaAttributesAsync(
                    UserId,
                    AppId,
                    placementId,
                    offerId,
                    EventTypes.OfferPurchase,
                    new() {
                    { nameof(currencyCode), currencyCode },
                    { nameof(totalAmount), totalAmount },
                    },
                    customPayload);

            public static void LogOfferPurchaseEventWithProductId(string productId, string currencyCode, double totalAmount, Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventWithProductId(
                    UserId,
                    AppId,
                    productId,
                    EventTypes.OfferPurchase,
                    new() {
                    { nameof(currencyCode), currencyCode },
                    { nameof(totalAmount), totalAmount },
                    },
                    customPayload);

            public static void LogOfferInteractionEvent(string placementId, string offerId, string interactionType, Dictionary<string, object> customPayload = null)
                => _ = Sdk._eventManager.QueueEventWithMeticaAttributesAsync(
                    UserId,
                    AppId,
                    placementId,
                    offerId,
                    EventTypes.OfferInteraction,
                    new() { { nameof(interactionType), interactionType } },
                    customPayload);

            public static void LogOfferInteractionEventWithProductId(string productId, string interactionType, Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventWithProductId(
                    UserId,
                    AppId,
                    productId,
                    EventTypes.OfferInteraction,
                    new() { { nameof(interactionType), interactionType } },
                    customPayload);

            public static void LogOfferImpressionEvent(string placementId, string offerId, Dictionary<string, object> customPayload = null)
                => _ = Sdk._eventManager.QueueEventWithMeticaAttributesAsync(
                    UserId,
                    AppId,
                    placementId,
                    offerId,
                    EventTypes.OfferImpression,
                    null,
                    customPayload);

            public static void LogOfferImpressionEventWithProductId(string productId, Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventWithProductId(
                    UserId,
                    AppId,
                    productId,
                    EventTypes.OfferImpression,
                    null,
                    customPayload);

            public static void LogAdRevenueEvent(string placement, string type, string source, string currencyCode, double totalAmount, Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventAsync(
                    UserId,
                    AppId,
                    EventTypes.AdRevenue,
                    new() {
                    { nameof (placement), placement },
                    { nameof (type), type },
                    { nameof (source), source },
                    { nameof (currencyCode), currencyCode },
                    { nameof (totalAmount), totalAmount },
                    },
                    customPayload);

            public static void LogFullStateUserUpdateEvent(Dictionary<string, object> userStateAttributes, Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventAsync(
                    UserId,
                    AppId,
                    EventTypes.FullStateUpdate,
                    new() { { nameof(userStateAttributes), userStateAttributes }, },
                    customPayload);

            public static void LogPartialStateUserUpdateEvent(Dictionary<string, object> userStateAttributes, Dictionary<string, object> customPayload = null)
                => Sdk._eventManager.QueueEventAsync(
                    UserId,
                    AppId,
                    EventTypes.PartialStateUpdate,
                    new() { { nameof(userStateAttributes), userStateAttributes } },
                    customPayload);

            public static void LogCustomEvent(string customEventType, Dictionary<string, object> customPayload = null)
            {
                if (EventTypes.IsEventType(customEventType))
                {
                    Log.Error(() => $"{customEventType} cannot be used with {nameof(LogCustomEvent)}. Please use an event type that is not a core event. See documentation at https://docs.metica.com/integration#core-events.");
                    return;
                }
                Sdk._eventManager.QueueEventAsync(
                UserId,
                AppId,
                customEventType,
                null,
                customPayload);
            }
            public static void RequestDispatchEvents()
            {
                _ = Sdk._eventManager.RequestDispatchEvents();
            }
        }

        #endregion Events
        
        private static PlatformDelegate CreatePlatformDelegate()
        {
            // IMPORTANT: DO NOT IMPORT THE QUALIFIER.
            // Otherwise, Jetbrains Rider could remove unused qualifiers when committing.
            PlatformDelegate UnityPlayerDelegate() => new Metica.Ads.UnityPlayer.UnityPlayerDelegate();

#if UNITY_EDITOR
            // Check for Unity Editor first since the editor also responds to the currently selected platform.
            return UnityPlayerDelegate();
#elif UNITY_ANDROID
            return new Metica.Ads.Android.AndroidDelegate(Metica.Ads.AndroidUnityBridge.UnityBridgeClass);
#elif UNITY_IPHONE || UNITY_IOS
            return new Metica.Ads.IOS.IOSDelegate();
#else
            return UnityPlayerDelegate();
#endif
        }
    }
}
